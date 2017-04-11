using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="play_sfx")]
	public class PlaySfxCommand : VsnCommand {

		string sfxName;

		public override void Execute (){
			// Grab audioclip for audio manager instead of just a string
			VsnAudioManager.instance.PlaySfx(sfxName);
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.sfxName = args [0].stringValue;
			}
		}

	}
}