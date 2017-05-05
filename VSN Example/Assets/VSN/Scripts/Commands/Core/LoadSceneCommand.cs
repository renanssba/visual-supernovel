using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Command{

	[CommandAttribute(CommandString="load_scene")]
	public class LoadSceneCommand : VsnCommand {

		string sceneName;

		public override void Execute (){
			SceneManager.LoadScene(sceneName);
		}

		public override void InjectArguments (List<VsnArgument> args){
      this.sceneName = args [0].GetStringValue();
		}

	}
}