using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	public Color colorFadeIn;
	public Color colorFadeOut;
	public bool disableAfterFadeIn = true;
	private GameObject imageObj;
	private Image image;

	void Start() {
		imageObj = transform.Find("ImageObj").gameObject;
		image = imageObj.GetComponent<Image>();
	}
	
	public void FadeIn(float anim_time){
		if(!imageObj){
			imageObj = transform.Find("ImageObj").gameObject;
		}
		if(!image){
			image = imageObj.GetComponent<Image>();
		}

		imageObj.SetActive(true);
		if(anim_time == 0){
			image.color = colorFadeIn; // fade quickly
			imageObj.SetActive(false);
		}
		else
			StartCoroutine("StartFadeIn", anim_time);
	}

	public void FadeOut(float anim_time){
		imageObj.SetActive(true);
		if(anim_time == 0)
			image.color = colorFadeOut; // fade quickly
		else
			StartCoroutine("StartFadeOut", anim_time);
	}

	IEnumerator StartFadeIn(float anim_time){


		float elapsedTime = 0;
		while(elapsedTime < anim_time){
			image.color = Color.Lerp(colorFadeOut, colorFadeIn, elapsedTime / anim_time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		image.color = colorFadeIn;
		if(disableAfterFadeIn)
			imageObj.SetActive(false);
	}

	IEnumerator StartFadeOut(float anim_time){

		float elapsedTime = 0;
		while(elapsedTime < anim_time){
			image.color = Color.Lerp(colorFadeIn, colorFadeOut, elapsedTime / anim_time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		image.color = colorFadeOut;
	}
}
