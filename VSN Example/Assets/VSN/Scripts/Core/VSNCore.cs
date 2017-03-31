using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class VsnCore : MonoBehaviour {

	public List<VsnSubSystem> subSystemList;

	void Awake(){
		VsnDebug.Log ("VSN Core has " + subSystemList.Count + " subsystems loaded.");
	}

	public void ParseVSNCommands (string[] lines){
		VsnDebug.Log ("Analysing " + lines.GetLength (0) + " lines");

		foreach (string line in lines) {			
			if (line == "\r") continue;

			VsnDebug.Log ("Analysing line: " + line);
			string commandName = Regex.Match (line, "^([\\w\\-]+)").Value;
			VsnDebug.Log ("Command name: " + commandName);

			MatchCollection valuesMatch = Regex.Matches (line, "[^\\s\"']+|\"[^\"]*\"|'[^']*'");

			List<string> args = new List<string>();

			foreach(Match match in valuesMatch){
				args.Add(match.Value);
			}

			args.RemoveAt(0); // Removes the first match, which is the "commandName"

			foreach(string arg in args){
				VsnArgument vsnArgument = ParseArgument(arg);
			}
		}
	}

	/// <summary>
	/// Parses a string into one of three arguments: a string, a number (float) or a reference to a variable
	/// </summary>
	/// <returns>The argument.</returns>
	private VsnArgument ParseArgument(string arg){
		
		if (arg.StartsWith("\"") && arg.EndsWith("\"")){
			VsnDebug.Log("String value:" + arg);
			return new VsnString(arg);
		}

		if (StringIsDigitsOnly(arg)){
			float value = float.Parse(arg);
			VsnDebug.Log("Float value: " + value);
			return new VsnNumber(float.Parse(arg));
		}

		//TODO add PROPERsupport for variables
		VsnDebug.Log("Variable Reference: " + arg);
		return new VsnVariableReference(arg);

	}

	/// <summary>
	/// Returns true if string is made of digits only (0~9) and a point ('.') for float values.
	/// </summary>
	/// <returns><c>true</c>, if is string is only digits, <c>false</c> otherwise.</returns>
	/// <param name="str">String.</param>
	private bool StringIsDigitsOnly(string str){
		foreach (char c in str)
		{
			if ((c < '0' || c > '9') && c != '.')
				return false;
		}

		return true;
	}
}
