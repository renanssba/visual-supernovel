using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="shake")]
	public class ScreenShakeCommand : VsnCommand {

		float shakeIntensity;
		float shakeDuration;

		public override void Execute (){

			VsnEffectManager.instance.ScreenShake(shakeDuration, shakeIntensity);
		}


		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2){
				this.shakeIntensity = args[0].floatValue;
				this.shakeDuration = args[1].floatValue;
			} else{
				shakeDuration = 0.5f;
				shakeIntensity = 1f;
			}
		}

	}
}