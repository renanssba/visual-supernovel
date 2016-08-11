using UnityEngine;
using System.Collections;

public class DebugShow : MonoBehaviour {

	public GameObject debugText;

	void Awake(){
    UpdateRender();
	}

	public void UpdateRender() {
		if(Persistence.debugMode){
      debugText.SetActive(true);
		}else{
      debugText.SetActive(false);
		}
	}
}
