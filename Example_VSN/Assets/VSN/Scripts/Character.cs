using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

public class Character : MonoBehaviour {

  public bool isMoving;
	public string currentName;
	public string charName;
  public string spriteName;
  public int id;

  private string imageCharName;
  private Image characterBaseImage;
  private MouthAnimator mouthAnimator;
  private EyesAnimator eyesAnimator;

  void Start(){
    characterBaseImage = GetComponent<Image>();
    mouthAnimator = GetComponentInChildren<MouthAnimator>();
    eyesAnimator = GetComponentInChildren<EyesAnimator>();

    characterBaseImage.enabled = false;
    mouthAnimator.gameObject.SetActive(false);
    eyesAnimator.gameObject.SetActive(false);
  }
  
  public void SetName(string sprite_name){
		spriteName = sprite_name;
		charName = GetRealNameBySpriteName(sprite_name);
	}

	void SetCharacterVisible(){
		if( characterBaseImage.sprite ) {
			characterBaseImage.enabled = true;
		} else {
			characterBaseImage.enabled = false;
		}
	}

  public void SetAlpha(float alpha){
    Image[] targets = new Image[3];
    targets[0] = characterBaseImage;
    targets[1] = mouthAnimator.GetComponent<Image>();
    targets[2] = eyesAnimator.GetComponent<Image>();


    foreach(Image target in targets){
      target.color = new Color(target.color.r,
                               target.color.g,
                               target.color.b,
                               alpha);
    }
  }
  
  public float GetAlpha(){
    return characterBaseImage.color.a;
  }

  public void SetFacing(float facing){
    if( (transform.localScale.x>=0f) != (facing>=0f) ){
      transform.localScale = new Vector3(transform.localScale.x*-1f, transform.localScale.y, transform.localScale.z);
    }
  }

  public void SetPositionX(float position_x){
    if(!isMoving){
      characterBaseImage.rectTransform.anchoredPosition = new Vector2(position_x*100f, 0f);
    }
  }
  
  public void SetPositionY(float position_y){
    
  }

  public void SetScale(float scale){
    transform.localScale = new Vector3(scale, scale, scale);
  }

  public void Mirror(){
    transform.localScale = new Vector3(transform.localScale.x * -1f,
                                       transform.localScale.y,
                                       transform.localScale.z);
  }


	string GetRealNameBySpriteName(string sprite_name){

    if( DoesNameStartWith(sprite_name, "jose") )
      return "Policial";
    
    if( DoesNameStartWith(sprite_name, "bruno") )
      return "Bruno";

    if( DoesNameStartWith(sprite_name, "coringa") )
      return "Sensei";

    if( DoesNameStartWith(sprite_name, "teleleco") )
      return "Teleleco";
    
    if( DoesNameStartWith(sprite_name, "netuno") ||  DoesNameStartWith(sprite_name, "fernando") )
      return "Netuno";
    
    if( DoesNameStartWith(sprite_name, "sofia") )
      return "Sofia";
    
    if( DoesNameStartWith(sprite_name, "rivaldo") )
      return "Rivaldo";
    
    if( DoesNameStartWith(sprite_name, "dorival") )
      return "Dorival";
    
    if( DoesNameStartWith(sprite_name, "melissa") )
      return "Melissa";
    
    if( DoesNameStartWith(sprite_name, "alberto") )
      return "Alberto";

    if( DoesNameStartWith(sprite_name, "guilherme") )
      return "Guilherme";

    if( DoesNameStartWith(sprite_name, "parlamentar") )
      return "Repórter";

//		//Debug.log("Real name not found for sprite: "+sprite_name);
		return null;
  }


  bool DoesNameStartWith(string sprite_name, string character_name){
    int size = sprite_name.Length;
    int size_char = character_name.Length;
    
    if( size>=size_char ){
      if( sprite_name.Substring(0, size_char).ToLower() == character_name ){
        return true;
      }
    }
    
    return false;
  }

  public void SetBaseSprite(Sprite sprite){
    characterBaseImage.sprite = sprite;
    SetCharacterVisible();
    characterBaseImage.SetNativeSize();
  }

  public Sprite GetBaseSprite(){
    return characterBaseImage.sprite;
  }
  
  public void SetMouthAnimation(Sprite[] anim){
    mouthAnimator.SetAnim(anim);
  }

  public void SetEyesAnimation(Sprite[] anim){
    eyesAnimator.SetAnim(anim);
  }

  public void StartMouthAnim(){
    mouthAnimator.StartAnim();
  }

  public void StopMouthAnim(){
    mouthAnimator.StopAnim();
  }

}
