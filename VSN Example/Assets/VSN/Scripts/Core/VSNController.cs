using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsnController : MonoBehaviour {

	public static VsnController instance;
	public VsnCore core;

	private List<VsnCommand> vsnCommands;

	void Awake(){
		instance = this;

		//StartVSN ("VSN Scripts/helloworld");
		VsnDebug.Log("Hello, world");
		/*
		VsnSave.SetVariable ("testvar", 18);
		VsnSave.SetVariable ("characterName", "Fulano");
		VsnSave.SetVariable ("currentMood", "ILIKETURTLES");
		VsnSave.Save (1);
		*/

		string currentMood = "";

		VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood before load: " + currentMood);

		VsnSaveSystem.Load (1);

		currentMood = VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood after load: " + currentMood);

		VsnSaveSystem.Load (2);

		currentMood = VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood after loading wrong save slot: " + currentMood);


	}

	/// <summary>
	/// Starts VSN with a given script path, starting from Resources root.
	/// </summary>
	/// <param name="scriptPath">Script path from Resources root (e.g \"VSN Scripts/myscript.txt\"</param>
	public void StartVSN(string scriptPath){
		VsnDebug.Log ("VSN reading script, path: " + scriptPath);
		StartVSNScript (scriptPath);
	}

	void StartVSNScript (string scriptPath){
		TextAsset textAsset = Resources.Load<TextAsset>(scriptPath);
		string[] lines = textAsset.ToString ().Split ('\n');

		vsnCommands = core.ParseVSNCommands (lines);

		foreach(VsnCommand vsnCommand in vsnCommands){
			vsnCommand.PrintName();
		}
	}
}
