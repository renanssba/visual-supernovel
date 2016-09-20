using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	public Color colorFadeIn;
	public Color colorFadeOut;
	public bool disableAfterFadeIn = true;
  public Image fadeImage;

	
	public void FadeIn(float anim_time){
    fadeImage.gameObject.SetActive(true);
		if(anim_time == 0){
      fadeImage.color = colorFadeIn; // fade quickly
      fadeImage.gameObject.SetActive(false);
    }else{
			StartCoroutine("StartFadeIn", anim_time);
    }
	}

	public void FadeOut(float anim_time){
    fadeImage.gameObject.SetActive(true);
		if(anim_time == 0)
      fadeImage.color = colorFadeOut; // fade quickly
		else
			StartCoroutine("StartFadeOut", anim_time);
	}

	IEnumerator StartFadeIn(float anim_time){
    float elapsedTime = 0;
		
    while(elapsedTime < anim_time){
      fadeImage.color = Color.Lerp(colorFadeOut, colorFadeIn, elapsedTime / anim_time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
    fadeImage.color = colorFadeIn;
    if(disableAfterFadeIn){
      fadeImage.gameObject.SetActive(false);
    }
	}

	IEnumerator StartFadeOut(float anim_time){
    float elapsedTime = 0;
		
    while(elapsedTime < anim_time){
      fadeImage.color = Color.Lerp(colorFadeIn, colorFadeOut, elapsedTime / anim_time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
    fadeImage.color = colorFadeOut;
	}
}
