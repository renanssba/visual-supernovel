using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class VSNScriptReader : MonoBehaviour {
	
  private static VSNScriptReader instance;

  public VSNCommands commandController;
  public string scriptName;
  public string[] vsnScriptContent;
  public int currentLine = -1;

  public TextAsset currentScript;
  private int totalLines;

  public void Awake() {
    instance = this;
  }

  public static VSNScriptReader GetInstance() {
    return instance;
  }


  public void SetCurrentScript(TextAsset newScript) {
    currentScript = newScript;
  }

  public void LoadScript() {
    //Debug.log("Load Script");
    //Debug.log("Chapter: "+Persistence.chapter_to_load+", Checkpoint: " +Persistence.checkpoint_to_load);

    Persistence.fastRead = true;
    StoreWaypointsAndCountLines();
    CreateScriptArray();
    Persistence.fastRead = false;
    Debug.Log("Script loaded");

    // initialize character animations
    //CreateCharacterAnims();
  }

  public void GoToLine(int line) {
    currentLine = line;
  }

  public int GetElseOrEndifLine() {
    int unclosedIfs = 0;
    for(int i = currentLine + 1; i < vsnScriptContent.Length; i++) {
      string line = vsnScriptContent[i];
      string command = VSNCommands.GetCommand(line);
      if(command == "if") {
        unclosedIfs++;
        continue;
      }
      if(command == "else" || command == "endif") {
        if(unclosedIfs == 0)
          return i;
        else {
          if(command == "endif") {
            unclosedIfs--;
          }
        }
      }   
    }

    Debug.LogError("NO ELSE/ENDIF FOUND");

    return -1;
  }

  public int GetEndifLine() {
    int unclosedIfs = 0;
    for(int i = currentLine + 1; i < vsnScriptContent.Length; i++) {
      string line = vsnScriptContent[i];
      string command = VSNCommands.GetCommand(line);
      if(command == "if") {
        unclosedIfs++;
        continue;
      }
      if(command == "endif") {
        if(unclosedIfs == 0)
          return i;
        else {
          unclosedIfs--;
        }
      }   
    }

    Debug.LogError("NO ENDIF FOUND");

    return -1;
  }

  public void ReadScript() {

//    Debug.Log("Called ReadScript with line: " + startingLine);
    currentLine++;

    while(currentLine < vsnScriptContent.Length) {
      string line = vsnScriptContent[currentLine];

      if(line.Length == 0) {
        goto nextIteration;
      }

      if(line[0] == '/' || line[0] == '*') {
        goto nextIteration;
      }

      commandController.CheckCommand(line, currentLine);
      if(commandController.CommandBreaksReading(line) == true) {
        return;
      }

      nextIteration:
      currentLine++;
    }
  }

  void CreateScriptArray() {

    TextReader reader;
    if(Persistence.debugMode && false) {
      FileStream scriptFile = new FileStream("script_debug.txt", FileMode.Open, FileAccess.Read);
      reader = new StreamReader(scriptFile);
    } else {
      reader = new StringReader(currentScript.text);
    }

    if(reader == null) {
      Debug.Log("Error loading Script file");
    }


    vsnScriptContent = new string[totalLines + 1];
    for(int i = 1; i < vsnScriptContent.Length; i++) {
      string line = reader.ReadLine();
      if(line != null && line.StartsWith("\t"))
        line = line.Trim();
      
      vsnScriptContent[i] = line;
    }

    reader.Close();
  }

  void StoreWaypointsAndCountLines() {
    Dictionary<string, int> waypoints = new Dictionary<string, int>();
    int lineCount = 1;
    bool canGetItem = true;
    string line = null;
    TextReader reader;

    if(Persistence.debugMode && false) {
      FileStream scriptFile = new FileStream("script_debug.txt", FileMode.Open, FileAccess.Read);
      reader = new StreamReader(scriptFile);
    } else {			
      reader = new StringReader(currentScript.text);
    }

    while((line = reader.ReadLine()) != null) {
//      Debug.Log("Starting line "+lineCount+".");

      if(line.Length <= 1) {
        lineCount++;
        continue;
      }

      string currentCommand = VSNCommands.GetCommand(line);
      if(currentCommand == "" || currentCommand == null){
        continue;
      }

      if(currentCommand == "waypoint") {
        string[] param = VSNCommands.GetParams(line);
        string waypointName = param[0];
        Debug.Log (waypointName + " in line "+lineCount);
        waypoints.Add(waypointName, lineCount);
      }
      if(currentCommand == "mouth_anim") {
        commandController.CheckCommand(line, lineCount);
      }
      if(currentCommand == "eye_blink_anim") {
        commandController.CheckCommand(line, lineCount);
      }

      lineCount++;
    }
    totalLines = lineCount;
		
    reader.Close();
    commandController.waypoints = waypoints;
  }


  public string PathForDocumentsFile(string filename) {
    if(Application.platform == RuntimePlatform.IPhonePlayer) {
      string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
      path = path.Substring(0, path.LastIndexOf('/'));
      return Path.Combine(Path.Combine(path, "Documents"), filename);
    } else if(Application.platform == RuntimePlatform.Android) {
      string resources = Path.Combine(Application.persistentDataPath, "Resources");
      string path = Path.Combine(resources, filename);
      return path;
    } else {
      string path = Application.dataPath + "/../" + filename;
      return path;
    }
  }
}
