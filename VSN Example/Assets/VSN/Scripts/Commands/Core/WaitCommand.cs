using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="wait")]
	public class WaitCommand : VsnCommand {

		float duration;

		public override void Execute (){
			VsnController.instance.state = ExecutionState.WAITING;
			VsnController.instance.StartCoroutine (Wait ());
		}

		private IEnumerator Wait(){
			yield return new WaitForSeconds (duration);
			VsnController.instance.state = ExecutionState.PLAYING;
		}



		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1) {
				this.duration = args [0].floatValue;
			}
		}

	}
}