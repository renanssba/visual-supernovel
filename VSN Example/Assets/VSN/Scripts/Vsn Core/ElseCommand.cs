using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="else")]
	public class ElseCommand : VsnCommand {

		public override void Execute (){
			int commandIndex = VsnController.instance.FindNextElseOrEndifCommand ();


			if (commandIndex == -1) {
				Debug.LogError ("ERROR: Invalid if/else/endif structure. Please check the command number " + this.commandIndex);
				VsnController.instance.currentCommandIndex = 999999999;
			} else {
				VsnController.instance.currentCommandIndex = commandIndex;
			}
		}

		public override void InjectArguments (List<VsnArgument> args){
			
		}

	}
}