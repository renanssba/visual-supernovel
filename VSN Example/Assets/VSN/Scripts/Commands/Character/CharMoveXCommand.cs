using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="char_move_x")]
	public class CharMoveXCommand : VsnCommand {

		string characterLabel;
		float characterPositionX;
		float duration;

		public override void Execute (){
			VsnUIManager.instance.MoveCharacterX(characterLabel, characterPositionX, duration);
		}
			
		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2) {
				characterLabel = args [0].stringValue;
				characterPositionX = args [1].floatValue;

				if (args.Count == 3) {
					duration = args [2].floatValue;
				} else {
					duration = 0f;
				}
			}
		}

	}
}