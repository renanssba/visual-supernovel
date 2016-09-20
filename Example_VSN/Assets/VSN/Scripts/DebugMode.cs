using UnityEngine;
using System.Collections;

public class DebugMode : MonoBehaviour {

	public static GameController gameController;

	void Start(){
		GameObject gc = GameObject.FindWithTag("GameController");
		if(gc){
			gameController = gc.GetComponent<GameController>();
		}
	}

	void Update () {

		// Debug Mode Commands
		if( Persistence.debugMode){
			if( Input.GetKeyDown(KeyCode.F5) && gameController ){ 
				gameController.ReloadLevel();
			}
		}

    if(Input.GetKeyDown(KeyCode.F12)){
//      AudioController.GetInstance().PlayClickSound();
			Persistence.SetDebugMode( !Persistence.debugMode );
		}

    if(Input.GetKey(KeyCode.S) && Persistence.debugMode) {
      FastForwardText();
    }
	}

  void FastForwardText(){
    GameController.GetInstance().ClickedScreen();
  }

}
