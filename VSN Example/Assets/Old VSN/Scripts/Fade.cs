using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour {

  public Image fadeImage;
	
	public void FadeIn(float animTime){
		if(animTime == 0){
      fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
      fadeImage.gameObject.SetActive(false);
    }else{
      fadeImage.gameObject.SetActive(true);
      fadeImage.DOFade(0f, animTime).OnComplete( ()=>{
        fadeImage.gameObject.SetActive(false);
      } );
    }
	}

	public void FadeOut(float animTime){
    fadeImage.gameObject.SetActive(true);
    if(animTime == 0){
      fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
    }else{
      fadeImage.DOFade(1f, animTime);
    }
	}

  public void FadeTo(float finalAlpha, float animTime){
    fadeImage.gameObject.SetActive(true);
    if(animTime == 0){
      fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, finalAlpha);
    }else{
      fadeImage.DOFade(finalAlpha, animTime).OnComplete( ()=>{
        if(fadeImage.color.a == 0f)
          fadeImage.gameObject.SetActive(false);
      } );
    }
  }
}
