﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VsnSave{

	static Dictionary<string, string> savedDataDictionary;

	static readonly string switchPrefix = "SWITCH";
	static readonly string varIntPrefix = "VARINT";
	static readonly string varFloatPrefix = "VARFLOAT";
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

	static VsnSave(){
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

	static string GetSwitchPrefix(string key, bool isGlobal){
		return switchPrefix + "_" + key;
	}

	static string GetVariableIntPrefix(string key, bool isGlobal){
		return varIntPrefix + "_" + key;
	}

	static string GetVariableFloatPrefix(string key, bool isGlobal){
		return varFloatPrefix + "_" + key;
	}

	static string GetVariableStringPrefix(string key, bool isGlobal){
		return varStringPrefix + "_" + key;
	}

	#endregion

	#region Switches (set, get)

	public static void SetSwitch(string key, bool value, bool isGlobal = false){		
		string savedKey = GetSwitchPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			savedDataDictionary[savedKey] = value.ToString();
		}else{
			savedDataDictionary.Add(savedKey, value.ToString());
		}
	}

	public static bool GetSwitch(string key, bool isGlobal = false){
		string savedKey = GetSwitchPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			bool currentValue;
			if (bool.TryParse(savedDataDictionary[savedKey], out currentValue)){
				return currentValue;
			}
		}
		return false;
	}

	#endregion

	#region Variables (sets, adds, gets)

	public static void SetVariable(string key, int value, bool isGlobal = false){
		string savedKey = GetVariableIntPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			savedDataDictionary[savedKey] = value.ToString();
		} else{
			savedDataDictionary.Add(savedKey, value.ToString());
		}

	}

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

	public static void AddVariable(string key, int amount, bool isGlobal = false){
		string savedKey = GetVariableIntPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			int currentValue;
			if (int.TryParse(savedDataDictionary[savedKey], out currentValue)){	
				savedDataDictionary[savedKey] = (currentValue + amount).ToString();
			}
		} else{
			savedDataDictionary.Add(savedKey, amount.ToString());
		}
	}

	public static int GetIntVariable(string key, bool isGlobal = false){
		string savedKey = GetVariableIntPrefix(key, isGlobal);

		if (savedDataDictionary.ContainsKey(savedKey)){
			int currentValue;
			if (int.TryParse(savedDataDictionary[savedKey], out currentValue)){	
				return currentValue;
			}
		}

		return 0;

	}

	public static float GetFloatVariable(string key, bool isGlobal = false){
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

	public static void Save(){		
		SaveHandler.Save(savedDataDictionary, SaveSlot, (bool success) => {
			if (success){
			} else{
			}
		});
	}

	public static void Load(){
		SaveHandler.Load(savedDataDictionary, SaveSlot, (bool success) => {
			
		});


	}

	#endregion

}