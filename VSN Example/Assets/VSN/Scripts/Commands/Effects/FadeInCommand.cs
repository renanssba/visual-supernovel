using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="fade_in")]
	public class FadeInCommand : VsnCommand {

		float duration;

		public override void Execute (){
			VsnEffectManager.instance.FadeIn(duration);
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.duration = args [0].floatValue;
			} else{
				this.duration = 0.5f; //default
			}
		}

	}
}