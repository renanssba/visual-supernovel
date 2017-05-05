using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "add_var")]
  public class AddVariableCommand : VsnCommand {

    VsnArgument variableName;
    VsnArgument valueToSet;

    public override void Execute() {
      float oldValue = VsnSaveSystem.GetFloatVariable(variableName.GetVariableReference());
      float newValue = oldValue + valueToSet.GetNumberValue();

      Debug.LogWarning("Setting " + variableName.GetVariableReference() + " to add more "+ valueToSet.GetNumberValue());

      VsnSaveSystem.SetVariable(variableName.GetVariableReference(), newValue);
      VsnSaveSystem.Save(0);
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 2) {
        this.variableName = args[0];
        this.valueToSet = args[1];
      }
    }
  }
}