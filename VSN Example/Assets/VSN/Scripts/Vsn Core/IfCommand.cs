using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="if")]
	public class IfCommand : VsnCommand {

		string variableName;
		string comparedString;
		float comparedNumber;

		private bool conditionSatisfied;

		public override void Execute (){
			
			if (comparedString != ""){
				// is comparing to a string
				string variableValue = VsnSaveSystem.GetStringVariable(variableName);
				if (comparedString == variableValue){
					conditionSatisfied = true;
				}
			} else{
				// is comparing to a number
				float variableValue = VsnSaveSystem.GetNumberVariable(variableName);
				if (comparedNumber == variableValue){
					conditionSatisfied = true;
				}
			}

			if (conditionSatisfied == false){
				List<VsnCommand> commands = VsnController.instance.vsnCommands;
			}

		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 3) {
				this.variableName = args[0].stringValue;

				this.comparedString = args[2].stringValue;
				this.comparedNumber = args[2].floatValue;
			}
		}

	}
}