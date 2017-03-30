using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class VSNCore : MonoBehaviour {

	public List<VSNSubSystem> subSystemList;

	void Awake(){
		VSNDebug.Log ("VSN Core has " + subSystemList.Count + " subsystems loaded.");
	}

	public void ParseVSNCommands (string[] lines){
		VSNDebug.Log ("Analysing " + lines.GetLength (0) + " lines");

		foreach (string line in lines) {
			VSNDebug.Log ("Analysing line: " + line);
			string commandName = Regex.Match (line, "^([\\w\\-]+)").Value;
			VSNDebug.Log ("Command name: " + commandName);

			Match valuesMatch = Regex.Match (line, "[^\\s\"']+|\"[^\"]*\"|'[^']*'");

			valuesMatch.NextMatch(); //Skips the initial value, which is the command name already
			while (valuesMatch.Success) {
				// Infinite loop, beware
				VSNDebug.Log(valuesMatch.Value);
				valuesMatch.NextMatch ();
			}

		}
	}
}
