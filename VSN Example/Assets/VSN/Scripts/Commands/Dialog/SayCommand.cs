using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {
	
  [CommandAttribute(CommandString = "say")]
  public class SayCommand : VsnCommand {

    VsnArgument messageText;
    VsnArgument messageTitle;

    private bool changeTitle;

    public SayCommand() {
      VsnDebug.Log("Created new SayCommand");
    }

    public override void Execute() {
      VsnUIManager.instance.SetMessagePanel(true);
      VsnController.instance.state = ExecutionState.WAITINGINPUT;
      if(changeTitle) {
        VsnUIManager.instance.SetTextTitle(messageTitle.GetStringValue());
      }
      VsnUIManager.instance.SetText(messageText.GetStringValue());
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 2) {
        messageTitle = args[0];
        messageText = args[1];
        changeTitle = true;
      } else if(args.Count >= 1) {
        messageText = args[0];
        changeTitle = false;
      }
    }
  }
}