using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VsnSaveSystem{

	static Dictionary<string, string> savedDataDictionary;

	static readonly string varFloatPrefix = "VARNUMBER";
	static readonly string varStringPrefix = "VARSTRING";

	private static int saveSlot;
	private static IVsnSaveHandler saveHandler;

	#region getters/setters

	/// <summary>
	/// Gets or sets the current save slot. Starting slot is 1.
	/// Use 1 or more for actual save slots.
	/// </summary>
	/// <value>The save file.</value>
	public static int SaveSlot {
		get {
			return saveSlot;
		}
		set{
			if (value >= 1){
				saveSlot = value;	
			}
		}
	}

	public static IVsnSaveHandler SaveHandler {
		get;
		set;
	}

	#endregion

	static VsnSaveSystem(){
		SaveSlot = 1;
		SaveHandler = new DiskSaveHandler();

		savedDataDictionary = new Dictionary<string, string>();
	}
		
	static string GetSaveSlotPrefix(bool isGlobal){
		if (isGlobal){
			return "0";
		} else{
			return SaveSlot.ToString();
		}
	}

	#region Prefixes

	static string GetVariableFloatPrefix(string key, bool isGlobal){
		return varFloatPrefix + "_" + key;
	}

	static string GetVariableStringPrefix(string key, bool isGlobal){
		return varStringPrefix + "_" + key;
	}

	#endregion

	#region Variables (sets, adds, gets)

	public static void SetVariable(string key, float value, bool isGlobal = false){
		string savedKey = GetVariableFloatPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			savedDataDictionary[savedKey] = value.ToString();
		} else{
			savedDataDictionary.Add(savedKey, value.ToString());
		}
	}

	public static void SetVariable(string key, string value, bool isGlobal = false){
		string savedKey = GetVariableStringPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			savedDataDictionary[savedKey] = value.ToString();
		} else{
			savedDataDictionary.Add(savedKey, value.ToString());
		}
	}

	public static void AddVariable(string key, float amount, bool isGlobal = false){
		string savedKey = GetVariableFloatPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){			
			float currentValue;
			if (float.TryParse(savedDataDictionary[savedKey], out currentValue)){
				savedDataDictionary[savedKey] =  (currentValue + amount).ToString();
			}

		} else{
			savedDataDictionary.Add(savedKey, amount.ToString());
		}
	}	

	public static float GetNumberVariable(string key, bool isGlobal = false){
		string savedKey = GetVariableFloatPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			float currentValue;
			if (float.TryParse(savedDataDictionary[savedKey], out currentValue)){	
				return currentValue;
			}
		}

		return 0f;
	}

	public static string GetStringVariable(string key, bool isGlobal = false){
		string savedKey = GetVariableStringPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			return savedDataDictionary[savedKey];
		}

		return "";
	}

	#endregion

	#region save/load

	public static void Save(int saveSlot){		
		SaveHandler.Save(savedDataDictionary, saveSlot, (bool success) => {
			if (success){
				VsnDebug.Log("VSN SAVE success");
			} else{
				
			}
		});
	}

	public static void Load(int saveSlot){
		SaveHandler.Load(savedDataDictionary, saveSlot, (Dictionary<string,string> dictionary) => {
			if (dictionary != null){
				savedDataDictionary = dictionary;
				VsnDebug.Log("VSN LOAD success");
			}

		});


	}

	#endregion

}
