using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="else")]
	public class ElseCommand : VsnCommand {

		public override void Execute (){
			
		}

		public override void InjectArguments (List<VsnArgument> args){
			
		}

	}
}