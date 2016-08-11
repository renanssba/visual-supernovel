using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

  private static AudioController instance;
//	private Options options;
	private bool fadingMusic;

 // FMOD.Studio.EventInstance musicEvent = null;
  //FMOD.Studio.EventInstance ambienceEvent = null;

//
//  [FMODUnity.BankRef]
//  public string masterBank;
//  [FMODUnity.BankRef]
//  public string sfxBank;
//  [FMODUnity.BankRef]
//  public string uiBank;
//	[FMODUnity.BankRef]
//	public string puzzleBank;
//  [FMODUnity.BankRef]
//  public string[] musicBanks;

  void Awake(){
    if( instance==null ){
      instance = this;
//
//      FMODUnity.RuntimeManager.LoadBank(masterBank);
//      FMODUnity.RuntimeManager.LoadBank(sfxBank);
//      FMODUnity.RuntimeManager.LoadBank(uiBank);
////	    FMODUnity.RuntimeManager.LoadBank(puzzleBank);
//      foreach(string bank in musicBanks)
//        FMODUnity.RuntimeManager.LoadBank(bank);

      DontDestroyOnLoad(transform.gameObject);
    }else if( instance!=this ){
      Destroy(gameObject);
      return;
    }
  }

  public static AudioController GetInstance(){
    if( AudioController.instance == null ){
      instance = GameObject.FindWithTag("AudioControl").GetComponent<AudioController>();
    }
    return AudioController.instance;
  }

	public void FadeMusic(float anim_time){
		if(anim_time == 0){
      StopMusic();
      // stop quickly
		}else{
      StartCoroutine(FadeOutMusic(anim_time));
			fadingMusic = true;
		}
	}

	public void StopFading(){
		StopCoroutine(FadeOutMusic(1f));
    StopMusic();
	}
	
	IEnumerator FadeOutMusic(float anim_time){
		yield return null;
//		float time = anim_time;
//
//    if(musicEvent!=null){
//  		while(time > 0){
//        musicEvent.setVolume( Mathf.Lerp(0f, /*options.musicVolume*/1f, time/anim_time) );
//  			time -= Time.deltaTime;
//  			yield return new WaitForEndOfFrame();
//  		}
//      StopMusic();
//    }else{
////      //Debug.log("passou aqui");
//    }
	}
	
  public void PlayMusic(string name){
//    if(musicEvent!=null){
//      FMOD.Studio.EventDescription description;
//      string eventName;
//
//      musicEvent.getDescription(out description);
//      if(description != null){
//        description.getPath(out eventName);
//        if(eventName == "event:/musicas/"+name){
//          // dont stop and replay music
//          //Debug.log("spare playing music");
//          return;
//        }
//      }
//
//      StopMusic();
//    }
//
//    if(name!="none"){
//      musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/musicas/"+name);
//      musicEvent.setVolume(/*options.musicVolume*/1f);
//      musicEvent.start();
//    }
	}

  public void SetMusicArgument(float argument){
//    musicEvent.setParameterValue("discurso", argument);
  }

  public void StopMusic(){
//    musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); 
//    musicEvent.release();
    fadingMusic = false;
  }


  public void PlayAmbience(string name, float argument){
//    if(ambienceEvent!=null){
//      StopAmbience();
//    }
//
//    if(name!="none"){
//      ambienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/ambiencia/"+name);
//      ambienceEvent.setParameterValue("Hype", argument);
//      ambienceEvent.setVolume(/*options.musicVolume*/ 1f *0.5f);
//      ambienceEvent.start();
//    }
  }

  public void StopAmbience(){
//    ambienceEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
//    ambienceEvent.release();
  }
	
	public void PlayClickSound(){
		PlaySfx("ui/opcao3");
	}
	
	public void PlayCancelSound(){
    PlaySfx("ui/cancelar1");
	}
	
	public void PlayConfirmSound(){
    PlaySfx("ui/evidencia2");
	}
	
	public void PlaySlideSound(){
//    PlaySfx("ui/dialogo_slide");
	}
	
	public void PlayPageSound(){
    PlaySfx("ui/pagina1");
	}
	
	public void PlayItemSound(){
    PlaySfx("ui/evidencia2");
	}

	public void SetDialogSfxClip(string sfxName){
//		switch(sfxName){
//		case "dialogo1":
//			dialogSource.clip = sfxList[2];
//			break;
//		case "dialogo2":
//			dialogSource.clip = sfxList[3];
//			break;
//		default:
//			//			//Debug.log("PlayDialogueSfx Error - Sfx Not Found: '" + sfxName + "'");
//			break;
//		}
	}
	
	public void PlaySfx(string name){
//    	FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/"+name, new Vector3(0f, 0f, 0f));
	}
	
	public void PlayDialogSound(){
    PlaySfx("dialogo/dialogo1");
	}
	
	public void SetDialogPitch(string characterName){
//		dialogSource.pitch = GetCharacterPitch(characterName);
	}
	
	float GetCharacterPitch(string characterName){
//		switch(characterName.ToLower()){
//		default:
////			//Debug.log("GetCharacterPitch Error - Character Name Not Found: '" + characterName + "'");
//			return dialogSource.pitch;
//			//return 1.0f;
//		}
    return 1f;
	}

	public void SetCharacterPitch(string characterName, float pitch){

		switch(characterName.ToLower()){
		default:
//			//Debug.log("SayPitch_Command Error - Character Name Not Found: '" + characterName + "'");
			break;
		}
	}
}
