using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VsnEffectManager : MonoBehaviour {
	
	public static VsnEffectManager instance;
	public Image flashScreenImage;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		
	}
	public void FlashScreen(float duration){
		flashScreenImage.GetComponent<CanvasGroup>().DOFade(1f, 0f);
		flashScreenImage.GetComponent<CanvasGroup>().DOFade(0f, duration);
	}

}
