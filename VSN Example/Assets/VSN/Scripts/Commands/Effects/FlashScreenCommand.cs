using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="flash")]
	public class FlashScreenCommand : VsnCommand {

		float duration;

		public override void Execute (){
			VsnEffectManager.instance.FlashScreen(this.duration);
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.duration = args [0].floatValue;
			} else{
				this.duration = 0.2f; //default
			}
		}

	}
}