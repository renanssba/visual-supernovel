using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
  private static GameController instance;

  public GameState gameState = GameState.PlayingScript;
//  public TextAsset[] scriptList = new TextAsset[4];
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
    dialogBox.gameObject.SetActive(false);
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
    Idle,
    Inventory,
    ChooseTopic,
    Paused
  }

	
  void Start() {
    // load the character animations here
    //Persistence.DeleteCharacterAnims();
    //Persistence.GetInstance().CreateCharacterAnims();

    if(gameState == GameState.PlayingScript)
      StartVSN();
  }

  void Update() {
    CheckToPlayScript();
  }

  public void ClickedScreen() {
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
    
  }

  public void SetQuestion(string question) {
    questionName = question;
  }

  public void SetLabelNames(string[] names) {
    labelNames = names;
  }

  void AnswerQuestion(int choiceIndex) {
    ScriptReader.GetInstance().GoToLine(choices [choiceIndex]);
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
    gameState = GameState.Idle;
  }

  public void WaitSeconds(float time) {
    StartCoroutine("Wait", time);
  }

  IEnumerator Wait(float time) {
    gameState = GameState.Idle;
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
    Persistence.SetChapterAccess(chapterId + 1, 1);
    Persistence.ReachCheckpoint(chapterId + 1, 0);
    Persistence.Load(chapterId + 1, 0);
  }



  public void StartVSN(){
    StartVSNScript("VSN_example_script");
  }

  public void StartVSNScript(string scriptToLoad){
//    dialogBox.gameObject.SetActive(true);
    screen.gameObject.SetActive(true);
    gameState = GameState.PlayingScript;


    TextAsset asset = Resources.Load<TextAsset>(scriptToLoad);
    if(asset == null){
      Debug.LogError("Error loading VSN Script: " + scriptToLoad);
    }
    reader.SetCurrentScript( asset );



    DebugMode.gameController = this;

    reader.LoadScript(checkpointStarted);

    lineStarted = reader.checkpoints [checkpointStarted];
    ScriptReader.GetInstance().GoToLine(lineStarted);
  }

  public void ResumeVSN() {
    gameState = GameState.PlayingScript;
    screen.gameObject.SetActive(true);
  }

  public void PauseVSN() {
    gameState = GameState.Paused;
    screen.gameObject.SetActive(false);
  }

  public void ExitGame() {
    Application.Quit();		
  }
}
