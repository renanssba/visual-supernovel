using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="choices")]
	public class ChoicesCommand : VsnCommand {

		string[] choices;
		string[] labels;

		public override void Execute (){
			VsnUIManager.instance.SetChoicesPanel (true, choices.Length);
			VsnUIManager.instance.SetChoicesTexts (choices);
			VsnUIManager.instance.SetChoicesLabels (labels);

			VsnController.instance.state = ExecutionState.WAITINGINPUT;
		}



		public override void InjectArguments (List<VsnArgument> args){
			choices = new string[args.Count / 2];
			labels = new string[args.Count / 2];
			int baseIndex = 0;
			for (int i = 0; i < args.Count; i += 2) {
        choices [baseIndex] = args [i].GetStringValue();
        labels [baseIndex] = args [i+1].GetStringValue();
				baseIndex++;
			}
		}

	}
}