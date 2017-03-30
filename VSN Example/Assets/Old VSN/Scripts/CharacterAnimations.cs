using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class CharacterAnimations {

	private static Dictionary<string, Sprite[]> MouthAnimationAssigned = new Dictionary<string, Sprite[]>();
	private static Dictionary<string, Sprite[]> EyeAnimationAssigned = new Dictionary<string, Sprite[]>();
	private static Dictionary<string, Sprite[]> mouthAnimations = new Dictionary<string, Sprite[]>();
  private static Dictionary<string, Sprite[]> eyeAnimations = new Dictionary<string, Sprite[]>();

	public static void DeleteAnimations(){

		foreach(string anim in mouthAnimations.Keys){
			(mouthAnimations[anim])[3] = null;
			(mouthAnimations[anim])[4] = null;
			(mouthAnimations[anim])[5] = null;
			Resources.UnloadAsset( (mouthAnimations[anim])[0] );
			Resources.UnloadAsset( (mouthAnimations[anim])[1] );
			Resources.UnloadAsset( (mouthAnimations[anim])[2] );
		}

		foreach(string anim in eyeAnimations.Keys ){
			Resources.UnloadAsset( (eyeAnimations[anim][0]) );
      Resources.UnloadAsset( (eyeAnimations[anim][1]) );
		}

		MouthAnimationAssigned.Clear();
		EyeAnimationAssigned.Clear();
		mouthAnimations.Clear();
		eyeAnimations.Clear();
		
		Resources.UnloadUnusedAssets();

		MouthAnimationAssigned = new Dictionary<string, Sprite[]>();
		EyeAnimationAssigned = new Dictionary<string, Sprite[]>();
		mouthAnimations = new Dictionary<string, Sprite[]>();
		eyeAnimations = new Dictionary<string, Sprite[]>();
	}

	public static void AssignMouthAnimation(string anim_name, string[] sprite_list){
		
		for(int i = 0; i < sprite_list.Length; i++){
			MouthAnimationAssigned[sprite_list[i]] = GetMouthAnimation(anim_name);
		}
	}

	public static void AssignEyeAnimation(string anim_name, string[] sprite_list){
		
		for(int i = 0; i < sprite_list.Length; i++){
			EyeAnimationAssigned[sprite_list[i]] = GetEyeAnimation(anim_name);
		}
	}

	public static Sprite[] GetMouthAnimationAssigned(string sprite_name){

		if(MouthAnimationAssigned.ContainsKey(sprite_name)){
      //Debug.log("Mouth animation "+sprite_name+" found!");
			return MouthAnimationAssigned[sprite_name];
    }else{
      //Debug.log("Mouth animation "+sprite_name+" not found!");
			return null;
    }
	}

	public static Sprite[] GetEyeAnimationAssigned(string sprite_name){

		if(EyeAnimationAssigned.ContainsKey(sprite_name))
			return EyeAnimationAssigned[sprite_name];
		else
			return null;
	}

	public static void CreateMouthAnimation(string anim_name){

		string mouth_path = "Characters/Mouth/" + anim_name;
		Sprite[] sprites = new Sprite[6];

		if(mouthAnimations.ContainsKey(anim_name)){
			//Debug.log("Trying to create same anim: "+anim_name);
			return;
		}

		sprites[0] = Resources.Load<Sprite>(mouth_path + "_0");
		sprites[1] = Resources.Load<Sprite>(mouth_path + "_1");
		sprites[2] = Resources.Load<Sprite>(mouth_path + "_2");
		sprites[3] = sprites[1];
		sprites[4] = sprites[2];
		sprites[5] = sprites[1];
    //Debug.log("Animation: "+anim_name+" created with success!");

		// actually set the new animation
		mouthAnimations[anim_name] = sprites;
	}
	
	public static void CreateEyeAnimation(string anim_name){

		string eyes_path = "Characters/Eyes/" + anim_name;
    Sprite[] sprites = new Sprite[2];
    
    if(eyeAnimations.ContainsKey(anim_name)){
      //Debug.log("Trying to create same anim: "+anim_name);
      return;
    }

    sprites[0] = Resources.Load<Sprite>(eyes_path + "_0");
    sprites[1] = Resources.Load<Sprite>(eyes_path + "_1");
    //Debug.log("Animation: "+anim_name+" created with success!");
    
    // actually set the new animation
    eyeAnimations[anim_name] = sprites;
	}

	public static Sprite[] GetMouthAnimation(string anim_name){
		if(mouthAnimations.ContainsKey(anim_name)) {
			return mouthAnimations [anim_name];
		}
		else{
			return null;
		}
	}

	public static Sprite[] GetEyeAnimation(string name){

		if(eyeAnimations.ContainsKey(name)) {
			return eyeAnimations [name];
		}
		else{
			return null;
		}
	}
}
