using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="move_x")]
	public class MoveXCommand : VsnCommand {

		string characterLabel;
		float characterPositionX;

		public override void Execute (){
			VsnUIManager.instance.MoveCharacterX (characterLabel, characterPositionX);
		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: MoveXCommand");
		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count == 2) {
				this.characterLabel = args [0].stringValue;
				this.characterPositionX = args [1].floatValue;
			}
		}

	}
}