using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsnAudioManager : MonoBehaviour {

	public static VsnAudioManager instance;


	void Awake(){
		if (instance == null){
			instance = this;
		}
	}

	public void PlaySfx(string sfxName){
		
	}

}
