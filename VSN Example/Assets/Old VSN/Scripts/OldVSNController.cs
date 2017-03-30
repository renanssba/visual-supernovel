using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OldVSNController : MonoBehaviour {
	
  private static OldVSNController instance;

  public GameState gameState = GameState.PlayingScript;
  public int lineCount;

  public Sprite bgTemp { get; set; }

  public CharacterList charList;

  public DialogBox dialogBox;
  public DialogScreen screen;
  public OldVSNScriptReader reader;

  public int[] choices;
  public string[] labelNames;
  private int testimonyNextWp;
  private string questionName;
  private bool waitForConfirmation;
  private string sendAnswerConnectionErrorWaypoint;
  private string sendAnswerDuplicateErrorWaypoint;
  public List<ScriptPosition> scriptStack;


  [System.Serializable]
  public struct ScriptPosition{
    public string scriptName;
    public int scriptLine;

    public ScriptPosition(string name, int line){
      scriptName = name;
      scriptLine = line;
    }
  }


  void Awake() {
    dialogBox.gameObject.SetActive(false);
    instance = this;
    scriptStack = new List<ScriptPosition>();
  }

  public static OldVSNController GetInstance() {
    return instance;
  }



  public enum GameState {
    PlayingScript,
    Dialog,
    Question,
    Collection,
    Idle,
    Menu,
    WaitingForConfirmation,
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

    if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) ||
       Input.GetKeyDown(KeyCode.Space)){
      ClickedScreen();
    }
  }



  public void StartVSN(){
    Debug.Log("Starting VSN");
    StartVSNScript("VSN_example_script");
  }

  public void StartVSNScript(string scriptToLoad){
    StartVSNScript(scriptToLoad, 0);
  }

  public void StartVSNScript(string scriptToLoad, string waypointToStart){
    StartVSNScript(scriptToLoad, 0);
    int lineToStart = OldVSNCommands.GetInstance().waypoints[waypointToStart];
    OldVSNScriptReader.GetInstance().GoToLine(lineToStart);
  }

  public void StartVSNScript(string scriptToLoad, int lineToLoad){
    //    dialogBox.gameObject.SetActive(true);
    screen.gameObject.SetActive(true);
    gameState = GameState.PlayingScript;


    TextAsset asset = Resources.Load<TextAsset>(scriptToLoad);
    if(asset == null){
      Debug.LogError("Error loading VSN Script: " + scriptToLoad);
    }
    reader.SetCurrentScript( asset );
    reader.scriptName = scriptToLoad;

    DebugMode.gameController = this;

    reader.LoadScript();
    OldVSNScriptReader.GetInstance().GoToLine(lineToLoad);
  }

  public void ResumeVSN() {
    gameState = GameState.PlayingScript;
    screen.gameObject.SetActive(true);
  }

  public void PauseVSN() {
    gameState = GameState.Paused;
    screen.gameObject.SetActive(false);
  }

  public void SaveScriptPositionInStack(){
    scriptStack.Add(new ScriptPosition(reader.scriptName, reader.currentLine));
  }

  public ScriptPosition LastScriptStackEntry(){
    ScriptPosition lastPosition = scriptStack[scriptStack.Count - 1];
    scriptStack.RemoveAt(scriptStack.Count - 1);

    return lastPosition;
  }

  public void ClearScriptPositionStack(){
    scriptStack.Clear();
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
    OldVSNScriptReader.GetInstance().GoToLine(choices [choiceIndex]);
    gameState = GameState.PlayingScript;
  }

  public void GoToSelectedWaypoint(int wp) {
    OldVSNScriptReader.GetInstance().GoToLine(wp);
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

  public void WaitForMenu(){
    gameState = GameState.Menu;
  }

  public void WaitForValidation(string connectionErrorWaypoint, string duplicateErrorWaypoint) {
    sendAnswerConnectionErrorWaypoint = connectionErrorWaypoint;
    sendAnswerDuplicateErrorWaypoint = duplicateErrorWaypoint;
    StartCoroutine("WaitForAnswerConfirm");
  }

  IEnumerator WaitForAnswerConfirm() {
    gameState = GameState.WaitingForConfirmation;
    while(gameState == GameState.WaitingForConfirmation){
      yield return null;
    }
    //gameState = GameState.PlayingScript;
  }

  public void SendAnswerConfirmation(){
    gameState = GameState.PlayingScript;
  }

  public void SendAnswerConnectionError(){
    if(OldVSNCommands.GetInstance().waypoints.ContainsKey(sendAnswerConnectionErrorWaypoint)) {
      int lineNumber = OldVSNCommands.GetInstance().waypoints[sendAnswerConnectionErrorWaypoint];
      OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
    }else{
      Debug.Log("ERROR SENDING ANSWER ERROR");
    }  
    gameState = GameState.PlayingScript;
  }

  public void SendAnswerDuplicateError(){
    if(OldVSNCommands.GetInstance().waypoints.ContainsKey(sendAnswerDuplicateErrorWaypoint)) {
      int lineNumber = OldVSNCommands.GetInstance().waypoints[sendAnswerDuplicateErrorWaypoint];
      OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
    }else{
      Debug.Log("ERROR SENDING ANSWER ERROR");
    }  
    gameState = GameState.PlayingScript;
  }




  public void State_Inventory() {
    gameState = GameState.Inventory;
  }

  public void ReloadLevel() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }


  public void ExitGame() {
    Application.Quit();		
  }
}
