using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsnUIManager : MonoBehaviour{

	public static VsnUIManager instance;

	public Image vsnMessagePanel;
	public Text vsnMessageText;
	public Button screenButton;

	void Awake(){
		instance = this;

		screenButton.onClick.AddListener (OnScreenButtonClick);
	}

	public void SetMessagePanel(bool value){
		vsnMessagePanel.gameObject.SetActive (value);
	}

	public void SetText(string msg){
		vsnMessageText.text = msg;
	}

	void OnScreenButtonClick (){
		Debug.Log ("Clicked on screen!");
		VsnController.instance.state = ExecutionState.PLAYING;
		SetMessagePanel (false);
	}
}

