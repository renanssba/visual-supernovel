using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{
	
	[CommandAttribute(CommandString="say")]
	public class SayCommand : VsnCommand {

		string messageString;

		public SayCommand(){
			VsnDebug.Log("Created new SayCommand");
		}


		public override void Execute (){
			VsnUIManager.instance.SetMessagePanel (true);
			VsnController.instance.state = ExecutionState.WAITINGINPUT;
			VsnUIManager.instance.SetText (messageString);
		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: SayCommand");
		}

		public override void InjectArguments (List<VsnArgument> args){
			
			this.messageString = args [0].stringValue;
			VsnDebug.Log ("Injecting argument: " + this.messageString);


		}

	}
}