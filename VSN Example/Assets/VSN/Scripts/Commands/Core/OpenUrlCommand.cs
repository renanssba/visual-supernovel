using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command {

  [CommandAttribute(CommandString = "open_url")]
  public class OpenUrlCommand : VsnCommand {

    VsnArgument url;

    public override void Execute() {
      Application.OpenURL(url.GetStringValue());
    }

    public override void InjectArguments(List<VsnArgument> args) {
      if(args.Count >= 1) {
        url = args[0];
      }
    }
  }
}