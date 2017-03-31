using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{
	
	[CommandAttribute(CommandString="say")]
	public class SayCommand : VsnCommand {

		public SayCommand(){
			VsnDebug.Log("Created new SayCommand");
		}


		public override void Execute (){
			
		}

		public override void PrintName (){
			VsnDebug.Log("PrintName: SayCommand");
		}

	}
}