using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSNController : MonoBehaviour {

	public static VSNController instance;
	public VSNCore core;

	private List<VSNCommand> commands;

	void Awake(){
		instance = this;

		StartVSN ("VSN Scripts/helloworld");
	}

	/// <summary>
	/// Starts VSN with a given script path, starting from Resources root.
	/// </summary>
	/// <param name="scriptPath">Script path from Resources root (e.g \"VSN Scripts/myscript.txt\"</param>
	public void StartVSN(string scriptPath){
		VSNDebug.Log ("VSN reading script, path: " + scriptPath);
		StartVSNScript (scriptPath);
	}

	void StartVSNScript (string scriptPath){
		TextAsset textAsset = Resources.Load<TextAsset>(scriptPath);
		string[] lines = textAsset.ToString ().Split ('\n');
		core.ParseVSNCommands (lines);

	}
}
