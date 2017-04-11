using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="save")]
	public class SaveCommand : VsnCommand {

		float saveSlot;

		public override void Execute (){
			int intSlot = int.Parse(saveSlot.ToString());

			VsnSaveSystem.Save(intSlot);
		}


		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.saveSlot = args[0].floatValue;
			}
			else{
				this.saveSlot = 0;
			}
		}

	}
}