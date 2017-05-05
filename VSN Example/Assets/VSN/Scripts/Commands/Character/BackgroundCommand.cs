using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "bg")]
  public class BackgroundCommand : VsnCommand {

    string backgroundFilename;

    public override void Execute() {
      Sprite backgroundSprite = Resources.Load<Sprite>("Backgrounds/" + backgroundFilename);
      if(backgroundSprite == null) {
        Debug.LogError("Error loading " + backgroundFilename + " character sprite. Please check its path");
        return;
      }
      VsnUIManager.instance.SetBackground(backgroundSprite);
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 1) {
        this.backgroundFilename = args[0].GetStringValue();
      }
    }

  }
}