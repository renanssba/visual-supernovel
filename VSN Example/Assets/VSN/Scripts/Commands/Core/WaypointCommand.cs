using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="waypoint")]
	public class WaypointCommand : VsnCommand {

		string label;

		public WaypointCommand(){
		}


		public override void Execute (){
			VsnController.instance.core.RegisterWaypoint (new VsnWaypoint (label, commandNumber));
		}

		public override void PrintName (){
		}

		public override void InjectArguments (List<VsnArgument> args){
			this.label = args [0].stringValue;
		}

	}
}