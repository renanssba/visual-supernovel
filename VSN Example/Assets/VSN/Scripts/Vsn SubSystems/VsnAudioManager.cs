using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsnAudioManager : MonoBehaviour {

	public static VsnAudioManager instance;

	private AudioSource audioSource;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySfx(AudioClip audioClip){
		audioSource.PlayOneShot(audioClip);
	}
}
