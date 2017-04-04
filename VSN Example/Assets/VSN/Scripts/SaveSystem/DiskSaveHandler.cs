using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class DiskSaveHandler : IVsnSaveHandler{

	private readonly string savePrefix = "VSNSAVE";

	void IVsnSaveHandler.Save (Dictionary<string, string> dictionary, int saveSlot, Action<bool> callback){
		bool success;
		string finalJson;
		string saveString = savePrefix + saveSlot.ToString();

		Dictionary<string, string> savedDictionary = new Dictionary<string, string>();
		//savedDictionary = PrefixDictionary(dictionary, saveSlot);

		Debug.Log("JSON count: " + dictionary.Count);
		finalJson = JsonUtility.ToJson(dictionary);
		Debug.Log("Saved JSON: " + finalJson);
		PlayerPrefs.SetString(saveString, finalJson);

		success = true;
		callback(success);
	}
		
	void IVsnSaveHandler.Load (Dictionary<string, string> dictionary, int saveSlot, Action<bool> callback){
		bool success = false;
		string loadedJson;
		string saveString = savePrefix + saveSlot.ToString();

		loadedJson = PlayerPrefs.GetString(saveString, "{}");
		Debug.Log("Loaded JSON: " + loadedJson);
		callback(success);
	}

	private Dictionary<string, string> PrefixDictionary(Dictionary<string, string> dictionary, int saveSlot){
		Dictionary<string, string> returnedDictionary = new Dictionary<string, string>();

		foreach(KeyValuePair<string, string> entry in dictionary){
			string prefixedKey = saveSlot.ToString() + "_" + entry.Key;
			returnedDictionary.Add(prefixedKey, entry.Value);
		}

		return returnedDictionary;
	}

}

