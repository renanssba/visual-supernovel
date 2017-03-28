﻿using UnityEngine;
using System.Collections;

public class DebugMode : MonoBehaviour {

	public static VSNController gameController;

	void Start(){
		GameObject gc = GameObject.FindWithTag("GameController");
		if(gc){
			gameController = gc.GetComponent<VSNController>();
		}
	}

	void Update () {

		// Debug Mode Commands
		if( Persistence.debugMode){
			if( Input.GetKeyDown(KeyCode.F5) && gameController ){ 
				gameController.ReloadLevel();
      }

      if(Input.GetKey(KeyCode.S)) {
        FastForwardText();
      }
		}

    if(Input.GetKeyDown(KeyCode.F12) && Application.platform == RuntimePlatform.WindowsEditor){
//      AudioController.GetInstance().PlayClickSound();
			Persistence.SetDebugMode( !Persistence.debugMode );
		}
	}

  void FastForwardText(){
    VSNController.GetInstance().ClickedScreen();
  }

}
