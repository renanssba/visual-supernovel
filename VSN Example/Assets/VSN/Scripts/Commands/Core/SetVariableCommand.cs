using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="set_var")]
	public class SetVariableCommand : VsnCommand {

		string variableName;
		float numberValue;
		string variableReference;

		public SetVariableCommand(){
			VsnDebug.Log("Created new SayCommand");
		}


		public override void Execute (){

		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: SetVariableCommand");

		}

		public override void InjectArguments (List<VsnArgument> args){
			
		}

	}
}