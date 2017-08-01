using UnityEngine;
using System.Collections;
using TMPro;

public class VsnConsoleSimulator : MonoBehaviour {
  public TMP_Text TmpText;

  public int totalCharacters;

  Coroutine showLettersCoroutine = null;

  void Awake() {
    TmpText = gameObject.GetComponent<TMP_Text>();
  }


  public void StartShowingCharacters(){
    showLettersCoroutine = StartCoroutine(RevealCharacters());
  }

  public void FinishShowingCharacters(){
    VsnUIManager.instance.isTextAppearing = false;

    TmpText.maxVisibleCharacters = totalCharacters;
    TmpText.ForceMeshUpdate();
    StopCoroutine(showLettersCoroutine);
    showLettersCoroutine = null;
  }


  public IEnumerator RevealCharacters() {
    VsnUIManager.instance.isTextAppearing = true;
    TMP_TextInfo textInfo = TmpText.textInfo;
    int numberOfCharsToShow;
    float elapsedTime = 0f;
    TmpText.ForceMeshUpdate();

    numberOfCharsToShow = 0;
    totalCharacters = textInfo.characterCount;

    while(numberOfCharsToShow < totalCharacters) {
      TmpText.maxVisibleCharacters = numberOfCharsToShow;

      elapsedTime += Time.deltaTime;
      numberOfCharsToShow = (int)(elapsedTime * VsnUIManager.instance.charsToShowPerSecond);
      yield return null;
    }
    FinishShowingCharacters();
  }
}
