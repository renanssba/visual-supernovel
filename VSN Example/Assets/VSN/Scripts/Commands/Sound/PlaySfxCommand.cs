using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="play_sfx")]
	public class PlaySfxCommand : VsnCommand {

		string sfxName;

		public override void Execute (){
			
			AudioClip audioClip = Resources.Load<AudioClip>("SFX/" + sfxName);
			VsnAudioManager.instance.PlaySfx(audioClip);
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.sfxName = args [0].stringValue;
			}
		}

	}
}