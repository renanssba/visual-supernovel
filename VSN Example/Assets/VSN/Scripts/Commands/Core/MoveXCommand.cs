using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="move_x")]
	public class MoveXCommand : VsnCommand {

		public MoveXCommand(){
			VsnDebug.Log("Created new MoveXCommand");
		}


		public override void Execute (){

		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: MoveXCommand");
		}

		public override void InjectArguments (List<VsnArgument> args){
			
		}

	}
}