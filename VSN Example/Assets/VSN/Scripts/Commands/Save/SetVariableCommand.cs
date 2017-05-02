using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "set_var")]
  public class SetVariableCommand : VsnCommand {

    string variableName;
    float numberValue;

    public override void Execute() {
      VsnSaveSystem.SetVariable(variableName, numberValue);
      VsnSaveSystem.Save(0);
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 2) {
        this.variableName = args[0].stringValue;
        this.numberValue = args[1].floatValue;
      }
    }
  }
}