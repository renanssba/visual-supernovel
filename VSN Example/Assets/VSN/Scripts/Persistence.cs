using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Persistence : MonoBehaviour {

  private static Persistence instance;

  public static int chapter_to_load = 1;
  public static int checkpoint_to_load = 0;
  public static bool debugMode = false;
  public static bool finishedLoadingAnims = false;
  public static bool fastRead = false;
  public static int matchVictories;

  public static string[] topicName;


  public TextAsset[] animScript;
  private static Dictionary<string, int> customVar = new Dictionary<string, int>();
  private static string customString;


  public static void ResetTopics() {
    for(int i = 0; i < 4; i++) {
      SetTopicActive("favor", i, "false");
      SetTopicActive("contrario", i, "false");
    }
  }


  public static bool SetNewTopic(string[] param) {
    string prefName;

    if(param.Length < 3) {
      //Debug.log("Script Error! Insuficient number of parameters");
      return false;
    }

    if(param[0] == "favor" || param[0] == "contrario") {
      prefName = param[0];
    } else {
      //Debug.log("Script Error! "+param[0]+" is not a valid topic list");
      return false;
    }

    if(IsTopicActive(prefName, int.Parse(param[1]))) {
      //Debug.logError("Script Error! "+param[0]+" is already set");
      return false;
    }

    SetTopicName(prefName, int.Parse(param[1]), param[2]);
    SetTopicActive(prefName, int.Parse(param[1]), "true");
    SetTopicRelevance(prefName, int.Parse(param[1]), "none");

    return true;
  }

  public static void SetTopicName(string listName, int key, string value) {
    PlayerPrefs.SetString(listName + key + "name", value);
  }

  public static void SetTopicActive(string listName, int key, string value) {
    PlayerPrefs.SetString(listName + key + "active", value);
  }

  public static void SetTopicRelevance(string listName, int key, string value) {
    PlayerPrefs.SetString(listName + key + "relevance", value);
  }

  public static bool IsTopicActive(string listName, int key) {
    if(PlayerPrefs.GetString(listName + key + "active") == "true") {
      return true;
    }
    return false;
  }

  public static string GetTopicName(string listName, int key) {
    return PlayerPrefs.GetString(listName + key + "name", "");
  }

  public static string GetTopicRelevance(string listName, int key) {
    return PlayerPrefs.GetString(listName + key + "relevance", "");
  }


  void Awake() {
    instance = this;
    LoadDebugMode();
  }

  public static Persistence GetInstance() {
    return instance;
  }

  public void SetCustomString(string value){
    customString = value;
  }

  public string GetCustomString(){
    return customString;
  }

  public int GetVariableValue(string key) {
    if(customVar.ContainsKey(key)) {
      return customVar[key];
    }
    return 0;
  }

  public void SaveGame() {
    ListAllVariables();
    if(!customVar.ContainsKey("hasSave"))
      customVar.Add("hasSave", 1);
    else
      customVar["hasSave"] = 1;
    foreach(KeyValuePair<string, int> entry in customVar) {
      if(entry.Key != "musicVolume" && entry.Key != "sfxVolume" && entry.Key != "textSpeed") {
        PlayerPrefs.SetInt(entry.Key, customVar[entry.Key]);
      }
      PlayerPrefs.SetInt("Chapter", chapter_to_load);
    }

    Debug.Log("Saving on day " + customVar["dia"]);
  }

  public bool LoadGame() {
    if(PlayerPrefs.HasKey("hasSave")) {
      Debug.Log("hasSave is in PlayerPrefs");
      customVar.Add("hasSave", PlayerPrefs.GetInt("hasSave"));
    } else {
      Debug.Log("hasSave is NOT in PlayerPrefs");
      return false;
    }

    if(customVar.ContainsKey("hasSave") && customVar["hasSave"] == 1) {
      chapter_to_load = PlayerPrefs.GetInt("Chapter");
      PlayerPrefs.SetString("checkpoint_name", "menu_dia");

      Debug.Log("Loaded on day " + customVar["dia"]);
      return true;
    } else {
      return false;
    }
  }

  public void SetVariableValue(string key, int value) {
    if(customVar.ContainsKey(key)) {
      customVar[key] = value;
    } else {
      customVar.Add(key, value);
    }
  }

  void ListAllVariables() {
    foreach(KeyValuePair<string, int> entry in customVar) {
      Debug.Log(entry.Key + " - " + customVar[entry.Key]);
    }
  }


  void Start() {
    DontDestroyOnLoad(transform.gameObject);
		
    // initialize registries
    Persistence.InitializeRegistries();

    // load all characters animations parallely
    //StartCoroutine("CreateCharacterAnimsCoroutine");
  }

  IEnumerator CreateCharacterAnimsCoroutine() {

    int lineCount = 1;
    string line;
    finishedLoadingAnims = false;
    TextReader animReader;
    if(Persistence.debugMode) {
      FileStream scriptFile = new FileStream("CharacterAnims.txt", FileMode.Open, FileAccess.Read);
      animReader = new StreamReader(scriptFile);
    } else {
      animReader = new StringReader(animScript[0].text);
    }
		
    while((line = animReader.ReadLine()) != null) {
      yield return new WaitForEndOfFrame();

      if(line.Length >= 10) {				
        if(line.Substring(0, 10).ToLower() == "mouth_anim") {
          VSNCommands.CheckAnimCommand(line, lineCount);
        }
      }
      if(line.Length >= 15) {				
        if(line.Substring(0, 15).ToLower() == "eye_blink_anim") {
          VSNCommands.CheckAnimCommand(line, lineCount);
        }
      }
			
      lineCount++;
    }
//		//Debug.log("FINISHED LOADING ANIMS!");
    finishedLoadingAnims = true;
		
    animReader.Close();
  }

  public void CreateCharacterAnims() {
//		
//    int lineCount = 1;
//    string line;
//    finishedLoadingAnims = false;
//    TextReader animReader;
//    if(Persistence.debugMode) {
//      FileStream scriptFile = new FileStream("CharacterAnims.txt", FileMode.Open, FileAccess.Read);
//      animReader = new StreamReader(scriptFile);
//    } else {
//      animReader = new StringReader(animScript[Persistence.chapter_to_load - 1].text);
//    }
//		
//    while((line = animReader.ReadLine()) != null) {			
//      if(!line.Contains("@")) {
//        lineCount++;
//        continue;
//      }
//      if(!(line.Substring(0, 1) == "@")) {
//        lineCount++;
//        continue;
//      }
//			
//      if(line.Length >= 11) {				
//        if(line.Substring(0, 11).ToLower() == "@mouth_anim") {
//          Command.CheckAnimCommand(line, lineCount);
//        }
//      }
//      if(line.Length >= 15) {				
//        if(line.Substring(0, 15).ToLower() == "@eye_blink_anim") {
//          Command.CheckAnimCommand(line, lineCount);
//        }
//      }
//			
//      lineCount++;
//    }
//    //		//Debug.log("FINISHED LOADING ANIMS!");
//    finishedLoadingAnims = true;
//		
//    animReader.Close();
  }

  public static void DeleteCharacterAnims() {
    CharacterAnimations.DeleteAnimations();
  }


  public static void SaveOptions(float musicVol, float sfxVol, float textSpeed) {

    PlayerPrefs.SetFloat("musicVolume", musicVol);
    PlayerPrefs.SetFloat("sfxVolume", sfxVol);
    PlayerPrefs.SetFloat("textSpeed", textSpeed);
    PlayerPrefs.Save();
  }

  public static float GetOptions_musicVolume() {

    return PlayerPrefs.GetFloat("musicVolume");
  }

  public static float GetOptions_sfxVolume() {

    return PlayerPrefs.GetFloat("sfxVolume");
  }

  public static float GetOptions_textSpeed() {

    return PlayerPrefs.GetFloat("textSpeed");
  }

  public static void SetLastPlayed(int chapterIndex, int checkpointIndex) {
	
    PlayerPrefs.SetInt("LastPlayedChapter", chapterIndex);
    PlayerPrefs.SetInt("LastPlayedCheckpoint", checkpointIndex);
    PlayerPrefs.Save();
  }

  public static int GetLastPlayedChapter() {

    return PlayerPrefs.GetInt("LastPlayedChapter");
  }

  public static int GetLastPlayedCheckpoint() {
		
    return PlayerPrefs.GetInt("LastPlayedCheckpoint");
  }

  public static bool SetChapterProgress(int chapterIndex, int latestCheckpoint) {
		
    string progressKey = "Chapter" + chapterIndex + "Progress";
    int last = -5;

    last = PlayerPrefs.GetInt(progressKey, -5); // -5 if the key does not exist

    if(last < latestCheckpoint) {
      PlayerPrefs.SetInt(progressKey, latestCheckpoint);
      PlayerPrefs.Save();
      return true;
    }
    return false;
  }

  public static int GetChapterProgress(int chapterIndex) {

    string progressKey = "Chapter" + chapterIndex + "Progress";
    return PlayerPrefs.GetInt(progressKey);
  }

  public static int GetScene(string sceneName) {

    switch(sceneName) {
      case "Credits1":
        return 0;
      case "Credits2":
        return 1;
      case "TitleScreen":
        return 2;
      case "Chapter":
        return 3;
      default:
//			//Debug.log("Scene Invalid.");
        return -1;
    }
  }

  public static void InitializeRegistries() {
    int i;
		
//		// initialize Options
//    if(!PlayerPrefs.HasKey("musicVolume")) {
//      SaveOptions(0.81f, 0.81f, 0.81f);
//    }
//		
//    PlayerPrefs.Save();
  }

  public static void SetDebugMode(bool activated) {
    // save persistence debug mode
    debugMode = activated;
    PlayerPrefs.SetInt("DebugMode", debugMode ? 1 : 0);
    UpdateIndicatorRender();
  }

  public static void LoadDebugMode() {
    // load persistence debug mode
    debugMode = (PlayerPrefs.GetInt("DebugMode", 0) == 1) ? true : false;
    UpdateIndicatorRender();
  }

  public static void UpdateIndicatorRender(){
    GameObject debugShow = GameObject.FindWithTag("DebugShow");
    if(debugShow != null){
      debugShow.GetComponent<DebugShow>().UpdateRender();
    }
  }
}
