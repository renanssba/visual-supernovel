﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;
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

	public List<VsnCommand> vsnCommands;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		state = ExecutionState.STARTING;
		core.GetComponent<VsnCore>();
		core.ResetWaypoints ();
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

	public int FindNextElseOrEndifCommand(){
		List<VsnCommand> commands = VsnController.instance.vsnCommands;

		int index = this.currentCommandIndex+1;

		int nestedIfCommandsFound = 0;

		for (int i = index; i < commands.Count; i++) {

			VsnCommand command = commands [i];

			if (command.GetType () == typeof(IfCommand)) {
				nestedIfCommandsFound += 1;
			} else if (command.GetType () == typeof(EndIfCommand)) {
				if (nestedIfCommandsFound == 0) {
					return command.commandIndex;
				} else {
					nestedIfCommandsFound -= 1;
				}
			} else if (command.GetType () == typeof(ElseCommand)) {
				if (nestedIfCommandsFound == 0) {
					return command.commandIndex;
				}
			}

		}

		return -1;
	}
}
