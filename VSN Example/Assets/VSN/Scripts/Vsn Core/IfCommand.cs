using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "if")]
  public class IfCommand : VsnCommand {

    VsnArgument firstOperand;
    VsnArgument comparisonOperator;
    VsnArgument secondOperand;

    public override void Execute() {
			
      bool comparisonResult = ((VsnOperator)comparisonOperator).EvaluateComparison(firstOperand, secondOperand);

      if( comparisonResult == false ) {
        int commandIndex = VsnController.instance.FindNextElseOrEndifCommand();

        if(commandIndex == -1) {
          Debug.LogError("ERROR: Invalid if/else/endif structure. Please check the command number " + this.commandIndex);
          VsnController.instance.currentCommandIndex = 999999999;
        } else {
          VsnController.instance.currentCommandIndex = commandIndex;
        }
      }
    }




    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 3) {
        firstOperand = args[0];
        comparisonOperator = args[1];
        secondOperand = args[2];
      }
    }
  }
}