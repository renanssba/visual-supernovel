using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="char_move_y")]
	public class CharMoveYCommand : VsnCommand {

		string characterLabel;
		float characterPositionY;
		float duration;

		public override void Execute (){
			VsnUIManager.instance.MoveCharacterY (characterLabel, characterPositionY, duration);
		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2) {
        this.characterLabel = args [0].GetStringValue();
        this.characterPositionY = args [1].GetNumberValue();

				if (args.Count == 3) {
          this.duration = args [2].GetNumberValue();
				} else {
					duration = 0f;
				}
			}
		}

	}
}