using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CharacterList : MonoBehaviour {

	public bool charMoving;
	public Character[] characters;
	private IEnumerator animRoutine;
	private Transform setAnimationCharacter;
	private Vector3 setAnimationDestination;

  public static CharacterList instance;

  void Awake(){
    instance = this;
  }

  public static CharacterList GetInstance(){
    return instance;
  }



  public int GetCharIdByParam(string param){
    int char_index;
    if( int.TryParse(param, out char_index) )
      return char_index-1;

    foreach(Character character in characters){
      if( character.charName.ToLower() == param.ToLower() ){
        return character.id;
      }
    }
    return -1;
  }

	public void MakeCharTalk(string name){
//		if(name=="")
//			return;
//		foreach(Character character in characters){
//			if( character.charName == name ){
//        character.StartMouthAnim();
//			}
//		}

	}
	
	public float FindPositionByName(string name){
		if(name=="")
			return -99999f;
		foreach(Character character in characters){
			if( character.charName == name ){
				return character.transform.position.x;
			}
		}
		return -99999f;
	}
	
	public void MakeAllCharsStopTalking(){
		foreach(Character character in characters){
      character.StopMouthAnim();
		}
	}
	
	public void AnimateAlpha(int charIndex, float anim_time, float end_alpha){
		if(anim_time == 0){
      characters[charIndex].SetAlpha(end_alpha); // change alpha instantly
    }else{
      characters[charIndex].gameObject.GetComponent<CanvasGroup>().DOFade(end_alpha, anim_time);
		}
	}


	public void AnimateScale(int charIndex, float animTime, float scale){

    Image charImage = characters[charIndex].GetComponent<Image>();
    Vector2 currentScale = charImage.rectTransform.localScale;
		Vector2 newScale = new Vector2(scale, scale);

		if(animTime == 0)
      charImage.rectTransform.localScale = newScale; // scale quickly
		else{
      IEnumerator scaleCoroutine = Scale(characters[charIndex].transform, animTime, currentScale, newScale);
			StartCoroutine(scaleCoroutine);
		}
			
	}

	IEnumerator Scale(Transform character, float anim_time, Vector2 startScale, Vector2 endScale){

		float elapsedTime = 0;
		float time = anim_time;
		Image charImage = character.GetComponent<Image>();
		while(elapsedTime < time){
      charImage.rectTransform.localScale = Vector2.Lerp(startScale, endScale,  Mathf.SmoothStep(0.0f, 1.0f, elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
    charImage.rectTransform.localScale = endScale;
	}

	public void Move_x(int charIndex, float anim_time, float destination_x){
		float convertedDestinX = destination_x*100f;
//		//Debug.log("Convert Screen " + Screen.width + ": " + + convertedDestinX);

		Transform character = characters[charIndex].transform;
		Image charImage = character.GetComponent<Image>();

		Vector2 destination = new Vector2(convertedDestinX, charImage.rectTransform.anchoredPosition.y);

		if(anim_time == 0){
			charImage.rectTransform.anchoredPosition = destination; // teleport
		}
		else{
			animRoutine = MoveX(character, anim_time,
			                     new Vector2(charImage.rectTransform.anchoredPosition.x,
			             					  charImage.rectTransform.anchoredPosition.y),
			                     destination);
			setAnimationCharacter = character;
			setAnimationDestination = destination;
			StartCoroutine(animRoutine);
		}
	}

	public void Move_y(int charIndex, float anim_time, float destination_y){
		float convertedDestinY = destination_y*100f;
		Transform character = characters[charIndex].transform;
		Image charImage = character.GetComponent<Image>();
		
//		//Debug.log("Pre value: " + destination_y);
//		//Debug.log("Pos value: " + convertedDestinY);

		Vector2 destination = new Vector2(charImage.rectTransform.anchoredPosition.x, convertedDestinY);

		if(anim_time == 0){
			charImage.rectTransform.anchoredPosition = destination; // teleport
		}
		else{
			animRoutine = MoveY(character, anim_time,
			                     new Vector2(charImage.rectTransform.anchoredPosition.x,
			             					  charImage.rectTransform.anchoredPosition.y),
			                     destination);
			setAnimationCharacter = character;
			setAnimationDestination = destination;
			StartCoroutine(animRoutine);
		}
	}

	public void StopAnimation(){
		if(charMoving){
			StopCoroutine(animRoutine);
			setAnimationCharacter.position = setAnimationDestination;
			charMoving = false;
		}
	}

	public IEnumerator MoveX(Transform character, float anim_time, Vector2 origin, Vector2 destination){
		float elapsedTime = 0;
		charMoving = true;
		Character thisCharacter = character.GetComponent<Character>();
		thisCharacter.isMoving = true;
		Image charac = character.GetComponent<Image>();

		while(elapsedTime < anim_time){
			destination = new Vector2(destination.x, charac.rectTransform.anchoredPosition.y);			
			charac.rectTransform.anchoredPosition = Vector2.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, elapsedTime / anim_time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		charac.rectTransform.anchoredPosition = destination;
		thisCharacter.isMoving = false;
		charMoving = false;
	}

	public IEnumerator MoveY(Transform character_transform, float anim_time, Vector2 origin, Vector2 destination){
		float elapsedTime = 0;
		charMoving = true;
		Character thisCharacter = character_transform.GetComponent<Character>();
		thisCharacter.isMoving = true;
		Image character = character_transform.GetComponent<Image>();
		
		while(elapsedTime < anim_time){
			destination = new Vector2(character.rectTransform.anchoredPosition.x, destination.y);
			character.rectTransform.anchoredPosition = Vector2.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, elapsedTime / anim_time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		character.rectTransform.anchoredPosition = destination;
		//thisCharacter.isMoving = false;
		charMoving = false;
		character_transform.GetComponent<Character>().isMoving = false;
	}
}

