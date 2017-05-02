using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="play_sfx")]
	public class PlaySfxCommand : VsnCommand {

		string sfxName;

		public override void Execute(){
			AudioClip audioClip = Resources.Load<AudioClip>("SFX/" + sfxName);
      if(audioClip == null){
        Debug.LogError("Error loading " + sfxName + " sfx. Please check its path");
        return;
      }
			VsnAudioManager.instance.PlaySfx(audioClip);
		}

		public override void InjectArguments(List<VsnArgument> args){
			if(args.Count >= 1){
				this.sfxName = args [0].stringValue;
			}
		}

	}
}