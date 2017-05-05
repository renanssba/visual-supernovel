using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "fade_out")]
  public class FadeOutCommand : VsnCommand {

    float duration;

    public override void Execute() {
      VsnEffectManager.instance.FadeOut(duration);
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 1) {
        this.duration = args[0].GetNumberValue();
      } else {
        this.duration = 0.5f; //default
      }
    }

  }
}