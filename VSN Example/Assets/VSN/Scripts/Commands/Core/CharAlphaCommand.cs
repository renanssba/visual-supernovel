using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="char_alpha")]
	public class CharAlphaCommand : VsnCommand {

		string characterLabel;
		float alphaValue;
		float duration;

		public override void Execute (){
			VsnUIManager.instance.SetCharacterAlpha (characterLabel, alphaValue, duration);
		}
			

		public override void PrintName (){
		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2) {
				this.characterLabel = args [0].stringValue;
				this.alphaValue = args [1].floatValue;

				if (args.Count == 3) {
					this.duration = args [2].floatValue;
				} else {
					duration = 0f;
				}
			}
		}

	}
}