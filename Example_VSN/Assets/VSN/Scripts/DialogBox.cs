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
  public GameController gameController;
  public GameObject box;
  public GameObject clickSymbol;
//  public Options options;
  
  private string dialogCache = "";
  private string currentDialogText;
  private int[,] pauseIndex = new int[0, 0];

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
  }

  public string GetCharacterName() {
    return charNameText.text;
  }

  public bool Say(string dialog) {

    if(talking) {
      return false;
    }
    talking = true;
    box.SetActive(true);
    if(CountChars(dialog, '@') > 0) {
      FindPause(dialog);
      dialog = RemovePause(dialog);
    }
    if(CountChars(dialog, '<') > 0) {
      sayRoutine = SayColoredDialog(dialog);
      StartCoroutine(sayRoutine);
    } else {
      sayRoutine = SayDialog(dialog);
      StartCoroutine(sayRoutine);
    }
    return true;
  }

  IEnumerator SayDialog(string dialog) {

    talking = true;
    string wrappedDialog = dialog;
    dialogCache = wrappedDialog;
    int i = 0;
    float pauseTime = 0f;
    while(i <= wrappedDialog.Length) {
      //	CheckForPause(i);


      currentDialogText = wrappedDialog.Substring(0, i);
      dialogText.text = currentDialogText;
      if(i % sfxInterval == 0 && dialogText.text.Length > 0)
        PlayDialogSfx();

      yield return new WaitForSeconds(textWaitTime + pauseTime);
      i++;
    }
    EndDialog();
  }

  IEnumerator SayColoredDialog(string dialog) {

    talking = true;
    string wrappedDialog = dialog;
    dialogCache = wrappedDialog;
    int[,] tagIndex = FindTags(wrappedDialog);
    string tagStart = "<Color=#008080ff>";
    string tagEnd = "</Color>";
    int c = 0;
    int i = 0;
    while(i <= wrappedDialog.Length) {
      //	CheckForPause(i);

      currentDialogText = wrappedDialog.Substring(0, i);

      if(c < tagIndex.GetLength(0)) {

        if(i > tagIndex[c, 0] && i < tagIndex[c, 1]) {
          i = tagIndex[c, 0] + 1;
          i += tagStart.Length;
          currentDialogText = wrappedDialog.Substring(0, i);
        } else if(i >= tagIndex[c, 2] && i < tagIndex[c, 3]) {
          i = tagIndex[c, 2];
          i += tagEnd.Length;
          currentDialogText = wrappedDialog.Substring(0, i);
        }

        if(i > tagIndex[c, 0] && i < tagIndex[c, 3]) {
          string tempText = currentDialogText.Insert(currentDialogText.Length, tagEnd);
          currentDialogText = tempText;
          dialogText.text = currentDialogText;
        } else {
          dialogText.text = currentDialogText;
        }

        if(i > tagIndex[c, 3]) {
          c++;
        }
      } else {
        dialogText.text = currentDialogText;
      }

      if(i % sfxInterval == 0 && dialogText.text.Length > 0)
        PlayDialogSfx();
      yield return new WaitForSeconds(textWaitTime);
      i++;
    }
    EndDialog();
  }

  public void SetSfxName(string sfx_name) {
//    AudioController.GetInstance().SetDialogSfxClip(sfx_name);
  }

  void PlayDialogSfx() {
//    AudioController.GetInstance().PlayDialogSound();
  }


  void CheckForPause(int index) {
    for(int i = 0; i < pauseIndex.GetLength(0); i++) {
      if(index == pauseIndex[i, 0]) {
        PauseDialog();
        return;
      }
    }
  }

  void PauseDialog() {
    StopCoroutine("SayDialog");
  }

  public void SkipDialog() {
    if(talking && dialogCache != "") {
      StopAllSay();
      dialogText.text = dialogCache;
      EndDialog();
    }
  }

  void StopAllSay() {
    StopCoroutine(sayRoutine);
    StopCoroutine("SayDialog");
    StopCoroutine("SayColoredDialog");
  }

  void EndDialog() {
    talking = false;
    if(gameController.gameState == GameController.GameState.Dialog){
      ShowIndicatorArrow(true);
    }
    gameController.charList.MakeAllCharsStopTalking();
    dialogCache = "";
    currentDialogText = "";
    dialogScreen.EndDialogEffect();
  }

  public void ShowIndicatorArrow(bool value) {
    clickSymbol.SetActive(value);
  }

  string RemoveTags(string text, int[,] tagIndex) {
	
    string newText = text;
    for(int i = tagIndex.GetLength(0) - 1; i >= 0; i--) {
      for(int j = tagIndex.GetLength(1) - 2; j >= 0; j -= 2) {
        if(j == 0) {
          newText = newText.Remove(tagIndex[i, 0], 17);
        } else if(j == 2) {
          newText = newText.Remove(tagIndex[i, 2], 8);
        }
        print(i + " " + j + " " + tagIndex[i, j] + " " + newText);
      }
    }
    print(newText);
    return newText;
  }

  int[,] FindTags(string text) {

    int tagCount = CountChars(text, '<') / 2;
    int[,] tagIndex = new int[tagCount, 4];
    char[] chars = new char[2];
    chars[0] = '<';
    chars[1] = '>';
    int m = 0;
    int n = 0;
    for(int i = text.IndexOfAny(chars); i > -1; i = text.IndexOfAny(chars, i + 1)) {

      if(m < tagIndex.GetLength(0)) {
        if(n < tagIndex.GetLength(1)) {
          if(n % 2 == 0)
            tagIndex[m, n] = i;
          else
            tagIndex[m, n] = i + 1;
          n++;
          if(n >= tagIndex.GetLength(1)) {
            m++;
            n = 0;
          }
        }
      }
    }
    return tagIndex;
  }

  int CountChars(string text, char c) {

    int charCount = 0;
    for(int i = 0; i < text.Length; i++) {
      if(text[i] == c) {
        charCount++;
      }
    }
    return charCount;
  }

  int[,] FindPause(string text) {

    int countChars = CountChars(text, '@');
    print(countChars);
    pauseIndex = new int[countChars, 2];
    char[] chars = new char[1];
    chars[0] = '@';
    int m = 0;
    int n = 0;
    for(int i = text.IndexOfAny(chars); i > -1; i = text.IndexOfAny(chars, i + 1)) {
			
      if(m < pauseIndex.GetLength(0)) {
        if(n < pauseIndex.GetLength(1)) {
          pauseIndex[m, 0] = i;
          pauseIndex[m, 1] = int.Parse(text.Substring(i + 1, 1));
          m++;
        }
      }
    }
    return pauseIndex;
  }

  string RemovePause(string text) {
		
    string newText = text;
    for(int i = pauseIndex.GetLength(0) - 1; i >= 0; i--) {
      newText = newText.Remove(pauseIndex[i, 0], 2);
    }
    return newText;
  }

  string WrapText(string text) {
		
    int lineLength = 60;
    for(var i = lineLength; i < text.Length; i += lineLength) {
      int returnSpot = text.LastIndexOf(" ", i);
      if(returnSpot >= 0) {
        text = text.Substring(0, returnSpot) + "\n" + text.Substring(returnSpot, text.Length - returnSpot);
        i = returnSpot;
      }
    }
    return " " + text;
  }
}
