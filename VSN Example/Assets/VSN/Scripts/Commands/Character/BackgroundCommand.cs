using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="bg")]
	public class BackgroundCommand : VsnCommand {

		string backgroundFilename;

		public override void Execute (){
			Sprite backgroundSprite = Resources.Load<Sprite>("Backgrounds/" + backgroundFilename);
			VsnUIManager.instance.SetBackground(backgroundSprite);

		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count >= 1) {
				this.backgroundFilename = args [0].stringValue;
			}
		}

	}
}