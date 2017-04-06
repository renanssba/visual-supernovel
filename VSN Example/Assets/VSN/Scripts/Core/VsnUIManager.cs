using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Command;
using TMPro;
using TMPro.Examples;
using DG.Tweening;

public class VsnUIManager : MonoBehaviour{

	public static VsnUIManager instance;

	public Image vsnMessagePanel;
	public TextMeshProUGUI vsnMessageText;
	public TextMeshProUGUI vsnMessageTitle;
	public Image vsnMessageTitlePanel;
	public Button screenButton;
	public Image choicesPanel;
	public Image charactersPanel;
	public Button[] choicesButtons;
	public Text[] choicesTexts;

	public GameObject vsnCharacterPrefab;

	private List<VsnCharacter> characters;

	void Awake(){
		instance = this;

		screenButton.onClick.AddListener (OnScreenButtonClick);
		characters = new List<VsnCharacter> ();
	}

	public void SetMessagePanel(bool value){
		vsnMessagePanel.gameObject.SetActive (value);
	}

	public void SetText(string msg){
		if (string.IsNullOrEmpty(vsnMessageTitle.text)) {
			vsnMessageTitlePanel.gameObject.SetActive (false);
		} else {
			vsnMessageTitlePanel.gameObject.SetActive (true);
		}
		vsnMessageText.text = msg;
		StartCoroutine (vsnMessageText.GetComponent<TextConsoleSimulator>().RevealCharacters());
	}

	public void SetTextTitle (string messageTitle){
		vsnMessageTitle.text = messageTitle;
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

	public void CreateNewCharacter (Sprite characterSprite, string characterFilename, string characterLabel){
		GameObject vsnCharacterObject = Instantiate (vsnCharacterPrefab, charactersPanel.transform) as GameObject;
		vsnCharacterObject.transform.localScale = Vector3.one;
		VsnCharacter vsnCharacter = vsnCharacterObject.GetComponent<VsnCharacter> ();

		Vector2 newPosition = new Vector2 (0f, 200f);
		vsnCharacter.GetComponent<RectTransform> ().anchoredPosition = newPosition;

		vsnCharacter.GetComponent<Image> ().sprite = characterSprite;
		vsnCharacter.label = characterLabel;
		vsnCharacter.characterFilename = characterFilename;

		characters.Add (vsnCharacter);
	}

	public void MoveCharacterX(string characterLabel, float position, float duration){
		float screenPosition = GetCharacterScreenPositionX (position);
		VsnCharacter character = FindCharacterByLabel (characterLabel);

		if (character != null) {
			Vector2 newPosition = new Vector2 (screenPosition, character.GetComponent<RectTransform> ().anchoredPosition.y);
			character.GetComponent<RectTransform> ().DOAnchorPos (newPosition, duration);
		}

	}

	public void SetCharacterAlpha(string characterLabel, float value, float duration){
		VsnCharacter character = FindCharacterByLabel (characterLabel);

		if (character != null) {
			character.GetComponent<Image> ().DOFade (value, duration);
		}
	}

	private float GetCharacterScreenPositionX(float normalizedPositionX){
		int maxPoint = 500;
		int minPoint = -500;
		int totalPoints = Mathf.Abs (maxPoint) + Mathf.Abs (minPoint);

		if (normalizedPositionX < 0f)
			return minPoint;
		else if (normalizedPositionX > 1f)
			return maxPoint;

		float finalPositionX = normalizedPositionX * totalPoints - totalPoints/2f;
		VsnDebug.Log ("Normalized position: " + normalizedPositionX + ", final position: " + finalPositionX);
		return finalPositionX;
	}

	private VsnCharacter FindCharacterByLabel(string characterLabel){
		foreach (VsnCharacter character in characters) {
			if (character.label == characterLabel) {
				return character;
			}
		}
		return null;
	}
		
	public void FlipCharacterSprite(string characterLabel){
		VsnCharacter character = FindCharacterByLabel (characterLabel);

		Vector3 localScale = character.transform.localScale;
		character.transform.localScale = new Vector3 (localScale.x * -1, localScale.y, localScale.z);
	}


	public void ResetAllCharacters (){
		foreach (VsnCharacter character in characters) {
			Destroy (character.gameObject);
		}
	}
}

