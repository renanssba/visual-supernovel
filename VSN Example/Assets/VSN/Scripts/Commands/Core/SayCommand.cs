using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{
	
	[CommandAttribute(CommandString="say")]
	public class SayCommand : VsnCommand {

		string messageText;
		string messageTitle;

		private bool changeTitle;

		public SayCommand(){
			VsnDebug.Log("Created new SayCommand");
		}


		public override void Execute (){
			VsnUIManager.instance.SetMessagePanel (true);
			VsnController.instance.state = ExecutionState.WAITINGINPUT;
			if (changeTitle) {
				VsnUIManager.instance.SetTextTitle (messageTitle);
			}
			VsnUIManager.instance.SetText (messageText);



		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: SayCommand");
		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 2) {
				this.messageTitle = args [0].stringValue;
				this.messageText = args [1].stringValue;
				changeTitle = true;
			} else if (args.Count >= 1) {
				this.messageText = args [0].stringValue;
				changeTitle = false;
			}

		}

	}
}