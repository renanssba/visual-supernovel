using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example entry point for the new VSN system. Just call VsnController.instance.StartVSN with the script path.
/// </summary>
public class ExampleEntryPoint : MonoBehaviour {


	void Start () {
		// Script is in the Resources folder. (e.g "VSN Scripts/example3" loads from "Resources/VSN Scripts/example3").
		// "example1": basic Say, goto, waypoint
		// "example2": say with rich tags, choices
		// "example3": characters, alpha, move, say with text
		// "example4": example of transition with movex and wait with a character

		VsnController.instance.StartVSN("VSN Scripts/example1");
	}

}
