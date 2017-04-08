using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="char_flip")]
	public class FlipCharacterCommand : VsnCommand {

		string characterLabel;

		public override void Execute (){
			VsnUIManager.instance.FlipCharacterSprite (characterLabel);
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1) {
				this.characterLabel = args [0].stringValue;
			}
		}

	}
}