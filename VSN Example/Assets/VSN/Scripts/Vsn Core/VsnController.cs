using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExecutionState{
	STARTING,
	PLAYING,
	WAITING,
	WAITINGINPUT,
	END,
	NumberOfExecutionStates
}
public class VsnController : MonoBehaviour {

	public static VsnController instance;
	public VsnCore core;
	public ExecutionState state;

	public int currentCommandIndex = -1;


	private List<VsnCommand> vsnCommands;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		state = ExecutionState.STARTING;
		core.GetComponent<VsnCore>();
		core.ResetWaypoints ();
		/*
		VsnSave.SetVariable ("testvar", 18);
		VsnSave.SetVariable ("characterName", "Fulano");
		VsnSave.SetVariable ("currentMood", "ILIKETURTLES");
		VsnSave.Save (1);
		*/

		/*
		string currentMood = "";

		VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood before load: " + currentMood);

		VsnSaveSystem.Load (1);

		currentMood = VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood after load: " + currentMood);

		VsnSaveSystem.Load (2);

		currentMood = VsnSaveSystem.GetStringVariable ("currentMood");
		VsnDebug.Log ("currentMood after loading wrong save slot: " + currentMood);
		*/
	}

	void Start(){
		
	}

	/// <summary>
	/// Starts VSN with a given script path, starting from Resources root.
	/// </summary>
	/// <param name="scriptPath">Script path from Resources root (e.g \"VSN Scripts/myscript.txt\"</param>
	public void StartVSN(string scriptPath){
		StartVSNScript (scriptPath);
	}

	void StartVSNScript (string scriptPath){
		TextAsset textAsset = Resources.Load<TextAsset>(scriptPath);
		string[] lines = textAsset.ToString ().Split ('\n');

		vsnCommands = core.ParseVSNCommands (lines);
			
		StartCoroutine (StartExecutingCommands ());

	}

	IEnumerator StartExecutingCommands (){
		state = ExecutionState.PLAYING;

		for (currentCommandIndex = 0; currentCommandIndex < vsnCommands.Count ; currentCommandIndex++){
			VsnCommand currentCommand = vsnCommands [currentCommandIndex];
			while (state != ExecutionState.PLAYING) {			
				yield return null;
			}

			currentCommand.Execute ();
		}
	}
}
