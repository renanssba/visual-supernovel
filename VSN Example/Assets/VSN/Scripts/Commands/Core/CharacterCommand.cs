﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command{

	[CommandAttribute(CommandString="character")]
	public class CharacterCommand : VsnCommand {

		string characterLabel;
		string characterFilename;

		public override void Execute (){
			Sprite characterSprite = Resources.Load<Sprite>("Characters/" + characterFilename);
			VsnUIManager.instance.CreateNewCharacter (characterSprite, characterFilename, characterLabel);

		}

		public override void PrintName (){
		}

		public override void InjectArguments (List<VsnArgument> args){
			if (args.Count == 2) {
				this.characterLabel = args [0].stringValue;
				this.characterFilename = args [1].stringValue;
			}
		}

	}
}