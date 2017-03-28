using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class DialogScreen : MonoBehaviour {
	
	public float defaultMusicVolume = 0.3f;
	private ScreenLayout layout = ScreenLayout.Dialog;
	public GameObject emoticonPrefab;
	public GameObject choices;
  public GameObject arrow;

  public DialogBox dialogBox;
  public CharacterList charList;
  public Image bg;
  public Image fg;
  public GameObject loadingIcon;

  public const int maxCharsInScreen = 6;
	
	public enum ScreenLayout{
		Dialog,
		Question
	}

	public void ChangeScreenLayout(ScreenLayout new_layout){
		layout = new_layout;

		switch(layout) {
		case ScreenLayout.Dialog:
			dialogBox.gameObject.SetActive(true);
			choices.SetActive(false);
			break;
		case ScreenLayout.Question:
			dialogBox.gameObject.SetActive(true);
      arrow.SetActive(false);
			choices.SetActive(false);
			break;
		}
	}
	
	public void SetDialog(string dialog){
		ChangeScreenLayout(ScreenLayout.Dialog);
		dialogBox.Say(dialog);
	}
	
	public void SetDialog(string characterName, string dialog){
		dialogBox.SetCharacterName(characterName);

		// set position of character name box
		if(characterName==""){
      arrow.SetActive(false);
		}else{
      arrow.SetActive(true);
      SetArrowPosition(characterName);
		}

		SetDialog(dialog);
	}

  public void EndDialogEffect(){
    if( layout == ScreenLayout.Question ){
      if( choices.activeSelf == false ){
        choices.SetActive(true);
      }
    }
  }

  public void SetArrowPosition(string characterName){
    float pos_x = charList.FindPositionByName(characterName);
    
    if(pos_x == -99999f){
      pos_x = -10f;
    }
    arrow.transform.position = new Vector3(pos_x,
                                           arrow.transform.position.y,
                                           arrow.transform.position.z);
    if(pos_x<0){
      arrow.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }else{
      arrow.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
  }

	public void Screenshake(int amount = 3){
		//Camera.main.DOShakePosition(2, amount*10, 80, 90f);
		//Camera.main.transform.DOMoveY(300f, 2f).SetRelative();
		GameObject.Find("GameCanvas").transform.Find("UI Elements").DOShakePosition(0.5f, amount*10, 80, 90f);
	}

	public string GetTalkingCharName(){
		return dialogBox.GetCharacterName();
	}
	
	public void SetSfxInterval(int interval){
		dialogBox.sfxInterval = interval;
	}
	
	public void SetDialogSfx(string name){
		dialogBox.SetSfxName(name);
	}
	
	public void SetCollect(int id, int backWp, int[] collectibleWp){
		// deprecated
	}
	
	public void SetCharacter(string[,] parameters){
		int i;
		ChangeScreenLayout(ScreenLayout.Dialog);

		// set characters images and names
    for(i=0; i<maxCharsInScreen; i++){
			if(parameters[i,0] != "None"){
        Character character = charList.characters[i];

        if(character.GetBaseSprite() != null){
          Resources.UnloadAsset(character.GetBaseSprite());
        }
        character.SetBaseSprite(Resources.Load<Sprite>("Characters/" + parameters[i,0]));
				character.SetName(parameters[i,0]);
        character.SetMouthAnimation( CharacterAnimations.GetMouthAnimationAssigned( parameters[i,0] ) );
        character.SetEyesAnimation( CharacterAnimations.GetEyeAnimationAssigned( parameters[i,0] ) );
        character.SetAlpha(1f);

        if(parameters[i,0] != "None"){
          character.SetPositionX( float.Parse(parameters[i,1]) );
          character.SetFacing(1f);
        }
			}
		}




//    foreach(Character character in charList.characters){
//      Image charImg = character.GetBaseSprite();
//
//			string charName = parameters[i,0];
//			
//			if(charName != "None"){
//				float new_x = float.Parse(parameters[i,1]);
//				
//				if(!charImg.GetComponent<Character>().isMoving){
//					charImg.rectTransform.anchoredPosition = new Vector2(new_x*100f, 0f);
//				}
//
//				if(i < 2)
//					charImg.rectTransform.eulerAngles = new Vector3(0, 0, 0);
//				else
//					charImg.rectTransform.eulerAngles = new Vector3(0, 180, 0);
//			}
//			i++;
//		}
	}
	
  public void SetCharacterSprite(string[] character_sprite){
		int i;
		ChangeScreenLayout(ScreenLayout.Dialog);

    for(i=0; i<maxCharsInScreen; i++){
			if(character_sprite[i] != "None"){
        Character character = charList.characters[i];
        character.SetBaseSprite( Resources.Load<Sprite>("Characters/" + character_sprite[i]) );
				character.SetName(character_sprite[i]);
        character.SetMouthAnimation( CharacterAnimations.GetMouthAnimationAssigned( character_sprite[i] ) );
        character.SetEyesAnimation( CharacterAnimations.GetEyeAnimationAssigned( character_sprite[i] ) );
			}
		}
  }

  public void SetCharacterBaseSprite(string[] character_sprite){
    for(int i=0; i<maxCharsInScreen; i++){
      if(character_sprite[i] != "None"){
        Character character = charList.characters[i];
        character.SetBaseSprite( Resources.Load<Sprite>("Characters/" + character_sprite[i]) );
      }
    }
  }
	
	void ClearCharacters(){
    foreach(Character character in charList.characters){
      Image charImg = character.GetComponent<Image>();
      charImg.sprite = null;
		}
	}

	
	public void MirrorCharacter(int charIndex){
    charList.characters[charIndex].Mirror();
  }
  
  public void SetMovement_x(int charIndex, float anim_time, float destination){
		ChangeScreenLayout(ScreenLayout.Dialog);
		charList.Move_x(charIndex, anim_time, destination);
	}
	
	public void SetMovement_y(int charIndex, float anim_time, float destination){
		ChangeScreenLayout(ScreenLayout.Dialog);
		charList.Move_y(charIndex, anim_time, destination);
	}
	
	public void StopAnimation(){
		charList.StopAnimation();
	}

	public void SetScale(int charIndex, float anim_time, float scale_y){
		charList.AnimateScale(charIndex, anim_time, scale_y);
	}
	
	public void SetQuestion(string text){
    VSNController.GetInstance().SetQuestion(text);
		dialogBox.Say(text);
	}

	public void SetQuestion(string characterName, string text){
		dialogBox.SetCharacterName(characterName);
    SetQuestion(text);
	}
	
  public void SetChoices(string[] text, int[] wp){
    VSNController.GetInstance().SetQuestionState(wp);
    dialogBox.SetChoicesText(text);
    ChangeScreenLayout(DialogScreen.ScreenLayout.Question);
	}
	
  public void SetBg(string path){
    if(path.ToLower() !="none"){
      bg.enabled = true;
      bg.sprite = Resources.Load<Sprite>("Bg/" + path);
    }else{
      bg.enabled = false;
    }
  }

  public void SetFg(string path){
    if(path.ToLower() !="none"){
      fg.enabled = true;
      fg.sprite = Resources.Load<Sprite>("Bg/" + path);
    }else{
      fg.enabled = false;
    }
  }
	
	public void EnableDialogBox(bool value){
		dialogBox.EnableBox(value);
	}

  public void SetLoadingIcon(bool value){
    loadingIcon.SetActive(value);
  }
}
