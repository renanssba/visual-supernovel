using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestionBox : MonoBehaviour {

  public bool talking { get; private set; }

  public bool skip { get; set; }

  public float textSpeed = 0.02f;
  public GameObject[] choiceButtons;
  public GameObject pointer;
  private string[] choiceTexts;
  private Text charName;
  private Text dialog;
  private GameController gameControl;
  private GameObject arrow;
  private Transform choiceBox;

  public void SetChoicesText(string[] new_choice_texts) {
    choiceTexts = new_choice_texts;
  }

  public void UpdateChoicesText() {

    for(int i = 0; i < choiceButtons.Length; i++) {
      pointer = choiceButtons[i];
      if(i < choiceTexts.Length) {
        choiceButtons[i].SetActive(true);
        choiceButtons[i].GetComponentInChildren<Text>().text = choiceTexts[i];
      } else {
        choiceButtons[i].SetActive(false);
      }
    }
    
    if(choiceTexts.Length == 2) {
      transform.position = new Vector3(0f, -0.7f, 0f);
    } else {
      transform.position = Vector3.zero;
    }
  }
}