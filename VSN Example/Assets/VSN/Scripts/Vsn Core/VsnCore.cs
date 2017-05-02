using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Reflection;
using Command;

public class VsnCore : MonoBehaviour {

	public List<VsnSubSystem> subSystemList;
	public List<Type> possibleCommandTypes;

	[HideInInspector] public List<VsnWaypoint> waypoints;

	void Awake(){
		VsnDebug.Log ("VSN Core has " + subSystemList.Count + " subsystems loaded.");
	}

	public List<VsnCommand> ParseVSNCommands (string[] lines){
		List<VsnCommand> vsnCommandsFromScript = new List<VsnCommand>();

		possibleCommandTypes = GetClasses("Command");

		int commandNumber = 0;
		foreach (string raw_line in lines) {			
      if (raw_line == "\r" || String.IsNullOrEmpty(raw_line)) continue;

			string line = raw_line.TrimStart();


			List<VsnArgument> vsnArguments = new List<VsnArgument>();

			string commandName = Regex.Match (line, "^([\\w\\-]+)").Value;

			MatchCollection valuesMatch = Regex.Matches (line, "[^\\s\"']+|\"[^\"]*\"|'[^']*'");

			List<string> args = new List<string>();

			foreach(Match match in valuesMatch){
				args.Add(match.Value);
			}

			args.RemoveAt(0); // Removes the first match, which is the "commandName"

			foreach(string arg in args){
				VsnArgument vsnArgument = ParseArgument(arg);
				vsnArguments.Add(vsnArgument);
			}

			VsnCommand vsnCommand = InstantiateVsnCommand(commandName, vsnArguments);
			if (vsnCommand != null){
				if (commandName == "waypoint") {
					RegisterWaypoint(new VsnWaypoint(vsnArguments[0].stringValue, commandNumber));
				}

				vsnCommand.commandIndex = commandNumber;
				commandNumber++;
				vsnCommandsFromScript.Add(vsnCommand);


			}
		}
			
		return vsnCommandsFromScript;
	}

	/// <summary>
	/// Iterates through all command classes searching for one with the correct CommandAttribute matching the commandName
	/// </summary>
	/// <returns>The vsn command.</returns>
	/// <param name="commandName">Command name.</param>
	/// <param name="vsnArguments">Vsn arguments.</param>
	private VsnCommand InstantiateVsnCommand(string commandName, List<VsnArgument> vsnArguments){
		foreach (Type type in possibleCommandTypes){
			
			foreach(Attribute attribute in type.GetCustomAttributes(false)){
				if (attribute is CommandAttribute){
					CommandAttribute commandAttribute = (CommandAttribute) attribute;
					if (commandAttribute.CommandString == commandName){
						VsnCommand vsnCommand = Activator.CreateInstance(type) as VsnCommand;
						vsnCommand.InjectArguments(vsnArguments);

						// TODO add metadata like line number?...

						return vsnCommand;
					}
				}
			}
		}

		VsnDebug.Log("Got a null");
		return null;
	}

	/// <summary>
	/// Parses a string into one of three arguments: a string, a number (float) or a reference to a variable
	/// </summary>
	/// <returns>The argument.</returns>
	private VsnArgument ParseArgument(string arg){
		
		if (arg.StartsWith("\"") && arg.EndsWith("\"")){
			return new VsnString(arg.Substring(1, arg.Length-2));
		}

		if (StringIsDigitsOnly(arg)){
			float value = float.Parse(arg);
			return new VsnNumber(float.Parse(arg));
		}

		//TODO add PROPERsupport for variables
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
		
	private List<Type> GetClasses(string nameSpace){
		Assembly asm = Assembly.GetExecutingAssembly();

		List<string> namespacelist = new List<string>();
		List<Type> typeList = new List<Type>();

		foreach (Type type in asm.GetTypes())
		{
			if (type.Namespace == nameSpace)
				typeList.Add(type);
		}


		return typeList;
	}

	public void ResetWaypoints(){
		waypoints = new List<VsnWaypoint> ();
	}

	public void RegisterWaypoint (VsnWaypoint vsnWaypoint){		
		if (waypoints.Contains (vsnWaypoint) == false) {
			waypoints.Add (vsnWaypoint);
		}
	}

	public VsnWaypoint GetWaypointFromLabel (string label){
		foreach (VsnWaypoint waypoint in waypoints) {
			if (waypoint.label == label) {
				return waypoint;
			}
		}

		return null;
	}
		
}
