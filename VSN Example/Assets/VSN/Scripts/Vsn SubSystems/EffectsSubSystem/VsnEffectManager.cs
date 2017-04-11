using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VsnEffectManager : MonoBehaviour {
	
	public static VsnEffectManager instance;
	public Image flashScreenImage;
	public Image fadeImage;
	public Image graphicsPanel;


	void Awake(){
		if (instance == null){
			instance = this;
		}
		
	}
	public void FlashScreen(float duration){
		flashScreenImage.GetComponent<CanvasGroup>().DOFade(1f, 0f);
		flashScreenImage.GetComponent<CanvasGroup>().DOFade(0f, duration);
	}

	public void ScreenShake(float duration, float intensity){
		graphicsPanel.transform.DOShakeRotation(duration, intensity);
	}

	public void FadeOut (float duration){
		fadeImage.GetComponent<CanvasGroup>().DOFade(1f, duration);
	}

	public void FadeIn (float duration){
		fadeImage.GetComponent<CanvasGroup>().DOFade(0f, duration);
	}
}
