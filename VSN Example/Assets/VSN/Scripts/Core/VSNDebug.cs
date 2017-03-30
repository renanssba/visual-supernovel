using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSNDebug{

	public static void Log(string msg){
		if (Application.isEditor) {
			Debug.Log (msg);	
		}
	}

}
