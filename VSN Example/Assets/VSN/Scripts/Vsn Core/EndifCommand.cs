using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "endif")]
  public class EndIfCommand : VsnCommand {

    public override void Execute() {

    }

    public override void InjectArguments(List<VsnArgument> args) {

    }

  }
}