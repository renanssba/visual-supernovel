using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogBox : MonoBehaviour {

  public static DialogBox instance;

  public bool talking { get; private set; }

  public float textWaitTime;
  public int sfxInterval = 1;
  public float sfxPitch = 1;
  public string characterTalking;
  public IEnumerator sayRoutine;
  public Text charNameText;
  public Text dialogText;
  public DialogScreen dialogScreen;
  public GameObject box;
  public GameObject clickSymbol;
  public GameObject characterNameBox;
  public Text[] choiceTexts;
  public Button[] choiceButtons;

  //  private string dialogCache = "";
  private int[,] pauseIndex = new int[0, 0];
  private string taggedDialogString = "";
  private string untaggedDialogString = "";
  public float charsToShowPerSecond = 50f;


  public class TagPair{
    public string openTag;
    public string closeTag;

    public int openTagPos;
    public int closeTagPos;
  }


  void Awake(){
    instance = this;
    UpdateTextSpeed();
  }

  public static DialogBox GetInstance(){
    return instance;
  }

  public void UpdateTextSpeed(){
    // FIXME wait time
    textWaitTime = 0.035f;
  }

  public void EnableBox(bool value) {
    box.SetActive(value);
  }

  public void SetCharacterName(string characterName) {
//    AudioController.GetInstance().SetDialogPitch(characterName);
    charNameText.text = characterName;
    characterTalking = characterName;
    if(characterName == ""){
      characterNameBox.SetActive(false);
    }else{
      characterNameBox.SetActive(true);
    }
  }

  public string GetCharacterName() {
    return charNameText.text;
  }

  public void Say(string dialogString) {

    box.SetActive(true);

    sayRoutine = ShowDialog(dialogString);
    StartCoroutine(sayRoutine);
  }


  IEnumerator ShowDialog(string dialogString) {
    int charsToShow = 0;
    int stringOffset;
    float elapsedTime = 0f;
    string untaggedString;
    List<TagPair> tagPairs = new List<TagPair>();

    talking = true;
    taggedDialogString = dialogString;
    untaggedString = dialogString;


    // remove tag pairs
    while(true){
      TagPair newTagPair = TryGetTagPair(untaggedString);

      if( newTagPair == null ){
        break;
      }
      tagPairs.Add(newTagPair);
      untaggedString = untaggedDialogString;
    }

    string firstHalf, secondHalf;

    while(charsToShow < untaggedString.Length){
      firstHalf = untaggedString.Substring(0, charsToShow);
      secondHalf = untaggedString.Substring(charsToShow, untaggedString.Length - charsToShow);

      stringOffset = 0;
      for(int i = tagPairs.Count-1; i>=0; i--){
        stringOffset = 0;
        if(charsToShow > tagPairs[i].openTagPos){
          firstHalf = PutSubstringInIndex(firstHalf, tagPairs[i].openTagPos, tagPairs[i].openTag);
          stringOffset += tagPairs[i].openTag.Length;

          if(charsToShow > tagPairs[i].closeTagPos){
            firstHalf = PutSubstringInIndex(firstHalf, tagPairs[i].closeTagPos + stringOffset, tagPairs[i].closeTag);
          }else{
            firstHalf += tagPairs[i].closeTag;
          }
          stringOffset += tagPairs[i].closeTag.Length;
        }
      }

      dialogText.text = firstHalf + "<color=#00000000>" + secondHalf + "</color>";

      yield return null;
      //      charsToShow += 5;
      elapsedTime += Time.deltaTime;
      charsToShow = (int)(elapsedTime*charsToShowPerSecond);
    }
    dialogText.text = dialogString;


    //    while(charsToShow <= initialDialog.Length) {
    //      currentDialogText = initialDialog.Substring(0, charsToShow);
    //      dialogText.text = currentDialogText;
    //      if(charsToShow % sfxInterval == 0 && dialogText.text.Length > 0)
    //        PlayDialogSfx();
    //
    //      yield return new WaitForSeconds(textWaitTime + pauseTime);
    //      charsToShow++;
    //    }

    yield return null;

    EndDialog();
  }

  string PutSubstringInIndex(string initialString, int index, string stringToPut){
    return initialString.Substring(0, index) + 
      stringToPut +
      initialString.Substring(index, initialString.Length - index);
  }


  TagPair TryGetTagPair(string fullString){
    int openTagIndex, closeTagIndex;


    /// GET OPEN TAG

    openTagIndex = fullString.IndexOf('<');
    closeTagIndex = fullString.IndexOf('>');

    //return if no chars encountered
    if(openTagIndex == -1 ||
       closeTagIndex == -1) {
      return null;
    }

    string currentString = "";
    if(openTagIndex > 0){
      currentString += fullString.Substring(0, openTagIndex);
    }
    if(closeTagIndex < fullString.Length-1){
      currentString += fullString.Substring(closeTagIndex+1, fullString.Length-closeTagIndex-1);
    }
    Debug.Log("current string: " + currentString);

    TagPair tp = new TagPair();
    tp.openTag = fullString.Substring(openTagIndex, closeTagIndex-openTagIndex+1);
    tp.openTagPos = openTagIndex;



    /// GET CLOSE TAG

    openTagIndex = currentString.LastIndexOf('<');
    closeTagIndex = currentString.LastIndexOf('>');

    //return if no chars encountered
    if(openTagIndex == -1 ||
       closeTagIndex == -1) {
      return null;
    }

    untaggedDialogString = "";
    if(openTagIndex > 0){
      untaggedDialogString += currentString.Substring(0, openTagIndex);
    }
    if(closeTagIndex < fullString.Length-1){
      untaggedDialogString += currentString.Substring(closeTagIndex+1, currentString.Length-closeTagIndex-1);
    }
    Debug.Log("untagged string: " + untaggedDialogString);

    tp.closeTag = currentString.Substring(openTagIndex, closeTagIndex-openTagIndex+1);
    tp.closeTagPos = openTagIndex;

    return tp;
  }



  void EndDialog(){
    dialogText.text = taggedDialogString;

    talking = false;

    if(VSNController.GetInstance().gameState == VSNController.GameState.Dialog){
      ShowIndicatorArrow(true);
    }
    VSNController.GetInstance().charList.MakeAllCharsStopTalking();
    dialogScreen.EndDialogEffect();
  }

  public void ClearDialogBox(){
    dialogText.text = "";
  }


  public void SkipDialog(){
    if(talking) {
      StopCoroutine(sayRoutine);
      EndDialog();
    }
  }



  public void SetSfxName(string sfx_name) {
    //    AudioController.GetInstance().SetDialogSfxClip(sfx_name);
  }

  void PlayDialogSfx() {
    //    AudioController.GetInstance().PlayDialogSound();
  }


  public void ShowIndicatorArrow(bool value) {
    clickSymbol.SetActive(value);
  }



  public void SetChoicesText(string[] choiceStrings) {

    for(int i = 0; i < choiceButtons.Length; i++) {
      if(i < choiceStrings.Length) {
        choiceButtons[i].gameObject.SetActive(true);
        choiceTexts[i].text = choiceStrings[i];
      } else {
        choiceButtons[i].gameObject.SetActive(false);
      }
    }
  }
}
