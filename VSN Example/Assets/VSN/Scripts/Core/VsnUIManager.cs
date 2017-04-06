using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Command;
using TMPro;
using TMPro.Examples;
public class VsnUIManager : MonoBehaviour{

	public static VsnUIManager instance;

	public Image vsnMessagePanel;
	public TextMeshProUGUI vsnMessageText;
	public Button screenButton;
	public Image choicesPanel;
	public Button[] choicesButtons;
	public Text[] choicesTexts;

	void Awake(){
		instance = this;

		screenButton.onClick.AddListener (OnScreenButtonClick);

	}

	public void SetMessagePanel(bool value){
		vsnMessagePanel.gameObject.SetActive (value);
	}

	public void SetText(string msg){
		vsnMessageText.text = msg;
		StartCoroutine (vsnMessageText.GetComponent<TextConsoleSimulator>().RevealCharacters());
	}

	void OnScreenButtonClick (){
		Debug.Log ("Clicked on screen!");
		VsnController.instance.state = ExecutionState.PLAYING;
		SetMessagePanel (false);
	}

	private void AddChoiceButtonListener (Button button, string label){
		button.onClick.AddListener (() => {
			VsnCommand command = new GotoCommand();
			List<VsnArgument> arguments = new List<VsnArgument>();
			arguments.Add(new VsnString(label));

			command.InjectArguments(arguments);
			SetChoicesPanel(false, 0);
			command.Execute();
			VsnController.instance.state = ExecutionState.PLAYING;
		});
	}

	public void SetChoicesPanel (bool enable, int numberOfChoices){
		choicesPanel.gameObject.SetActive (enable);

		if (enable) {
			for (int i = 0; i < choicesButtons.Length; i++) {
				bool willSetActive = (i < numberOfChoices);
				choicesButtons [i].gameObject.SetActive (willSetActive);
			}
		}
	}

	public void SetChoicesTexts (string[] choices){
		for (int i = 0; i < choices.Length; i++) {
			if (choicesTexts [i].gameObject.activeInHierarchy) {
				choicesTexts [i].text = choices [i];
			}
		}
	}

	public void SetChoicesLabels (string[] labels){
		for (int i = 0 ; i < labels.Length ; i++){
			AddChoiceButtonListener (choicesButtons [i], labels [i]);
		}
	}
}

