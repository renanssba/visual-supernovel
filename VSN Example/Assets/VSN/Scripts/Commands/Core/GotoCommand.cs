using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "goto")]
  public class GotoCommand : VsnCommand {

    string label;

    public override void Execute() {
      VsnWaypoint waypoint = VsnController.instance.core.GetWaypointFromLabel(label);

      if(waypoint != null) {
        VsnController.instance.currentCommandIndex = waypoint.commandNumber;
      } else {
        VsnDebug.Log("Invalid waypoint with label: " + this.label);
      }
    }

    public override void InjectArguments(List<VsnArgument> args) {
      label = args[0].GetStringValue();
    }
  }
}