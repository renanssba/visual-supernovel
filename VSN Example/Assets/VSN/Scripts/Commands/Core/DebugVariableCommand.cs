using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="debug_var")]
	public class DebugVariableCommand : VsnCommand {

		string shakeIntensity;

		public override void Execute (){
			
			float value = VsnSaveSystem.GetNumberVariable(shakeIntensity);

			Debug.Log("Variable " + shakeIntensity + ": " + value);
		}


		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1){
				this.shakeIntensity = args[0].variableReferenceValue;
			}
		}

	}
}