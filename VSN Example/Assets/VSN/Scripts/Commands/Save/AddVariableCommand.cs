using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="add_var")]
	public class AddVariableCommand : VsnCommand {

		string variableName;
		float numberValue;

		public override void Execute (){
			float oldValue = VsnSaveSystem.GetNumberVariable(variableName);
			float newValue = oldValue + numberValue;

			VsnSaveSystem.SetVariable(variableName, newValue);
		}


		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2){
				this.variableName = args[0].variableReferenceValue;
				this.numberValue = args[1].floatValue;
			} 
		}

	}
}