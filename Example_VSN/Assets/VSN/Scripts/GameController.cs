using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
  public static GameController instance;

  public GameState gameState;
  public TextAsset[] scriptList = new TextAsset[4];
  public int chapterId = -1;
  public int lineStarted = -1;
  public int checkpointStarted = -1;
  public int currentLine = -1;
  public int lineCount;

  public Sprite bgTemp { get; set; }

//	  public Quit quit;
  public CharacterList charList;

  public DialogBox dialogBox;
  public DialogScreen screen;
  public ScriptReader reader;
//  public Inventory inventory;

  private int[] choices;
  private int testimonyNextWp;
  private string[] labelNames;
  private string questionName;

  void Awake() {
    instance = this;
  }

  public static GameController GetInstance() {
    return instance;
  }



  public enum GameState {
    PlayingScript,
    Dialog,
    Question,
    Collection,
    Waiting,
    Inventory,
    ChooseTopic
  }

	
  void Start() {
    dialogBox.gameObject.SetActive(false);
				
    reader.SetCurrentScript(scriptList[0]);

    DebugMode.gameController = this;
		
    reader.LoadScript(checkpointStarted);

    // load the character animations here
    //Persistence.DeleteCharacterAnims();
    //Persistence.GetInstance().CreateCharacterAnims();

    lineStarted = reader.checkpoints[checkpointStarted];
    ScriptReader.GetInstance().GoToLine(lineStarted);
		
    //GameObject.FindWithTag("MainCanvas").transform.Find("UI Elements").transform.Find("OptionsMenu").GetComponent<Options>().LoadOptions();
    gameState = GameState.PlayingScript;
  }

  void Update() {
//     TODO(renan) make it open the quit confirm window
//    if(Input.GetKeyDown(KeyCode.Escape)) 
//			Application.Quit();

//    if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
//      ClickedScreen();
		
    /*
    if(inventory.IsActive() == false)
      CheckToPlayScript();
     */

		CheckToPlayScript ();

    if(Input.GetKey(KeyCode.S) && Persistence.debugMode){
      ClickedScreen ();
    }   
  }

  public void ClickedScreen() {
    Debug.Log("clicked screen!");
    switch(gameState) {
      case GameState.Dialog:
        AdvanceDialog();
        break;
      case GameState.Question:
        SkipDialog();
        break;
    }
  }

  void CheckToPlayScript() {
    if(gameState == GameState.PlayingScript) {
      PlayScript();
    }
  }

  void PlayScript() {
    reader.ReadScript();
  }

  void AdvanceDialog() {
    if(SkipDialog())
      return;
    if(NextDialog())
      return;
  }

  public void ChooseOption(int value) {
    SkipDialog();
    AnswerQuestion(value);

    Analytics.CustomEvent("choiceEvent", new Dictionary<string, object>{ { questionName, labelNames[value] } });
  }

  public void SetQuestion(string question) {
    questionName = question;
  }

  public void SetLabelNames(string[] names) {
    labelNames = names;
  }

  void AnswerQuestion(int choiceIndex) {
    ScriptReader.GetInstance().GoToLine(choices[choiceIndex]);
    gameState = GameState.PlayingScript;
  }

  public void GoToSelectedWaypoint(int wp) {
    ScriptReader.GetInstance().GoToLine(wp);
    gameState = GameState.PlayingScript;
  }

  public bool SkipDialog() {
    if(dialogBox.talking) {
      dialogBox.SkipDialog();
      return true;
    }
    return false;
  }

  public bool NextDialog() {
    if(!dialogBox.talking) {
      //screen.StopAnimation();
      dialogBox.ShowIndicatorArrow(false);
      gameState = GameState.PlayingScript;
      return true;
    }
    return false;
  }

  public void SetQuestionState(int[] choicesindex) {
    choices = choicesindex;
    gameState = GameState.Question;
  }

  public void SetDialogState() {
    gameState = GameState.Dialog;
  }

  public void SetCollectionState() {
    gameState = GameState.Collection;
  }

  public void SetChooseTopicState() {
    gameState = GameState.ChooseTopic;
  }

  public void WaitForItem() {
    gameState = GameState.Waiting;
  }

  public void WaitSeconds(float time) {
    StartCoroutine("Wait", time);
  }

  IEnumerator Wait(float time) {
    gameState = GameState.Waiting;
    yield return new WaitForSeconds(time);
    gameState = GameState.PlayingScript;
  }

  public void State_Inventory() {
    gameState = GameState.Inventory;
  }

  public void ReloadLevel() {
    Persistence.Load(chapterId, 0);
  }

  public void AdvanceChapter() {
    if(chapterId < 4) {
      Persistence.SetChapterAccess(chapterId + 1, 1);
      Persistence.ReachCheckpoint(chapterId + 1, 0);
      Persistence.Load(chapterId + 1, 0);
    } else { // if playing the last chapter, go back to title screen      
    }
  }

  public void ExitGame() {
    Application.Quit();		
  }
}
