using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "set_var")]
  public class SetVariableCommand : VsnCommand {

    VsnArgument variableToSet;
    VsnArgument valueToSet;

    public override void Execute() {
      Debug.LogWarning("Setting " + variableToSet.GetVariableReference() + " to value: "+ valueToSet.GetNumberValue());
      VsnSaveSystem.SetVariable(variableToSet.GetVariableReference(), valueToSet.GetNumberValue());
      VsnSaveSystem.Save(0);
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 2) {
        this.variableToSet = args[0];
        this.valueToSet = args[1];
      }
    }
  }
}