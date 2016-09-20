using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class ScriptReader : MonoBehaviour {
	
  private static ScriptReader instance;

	public Command commandController;
	public string[] script;
	public int[] checkpoints = new int[14];
  public int currentLine = -1;

  public TextAsset currentScript;
  private int totalLines;

  public void Awake(){
    instance = this;
  }

  public static ScriptReader GetInstance(){
    return instance;
  }


	public void SetCurrentScript(TextAsset newScript){
		currentScript = newScript;
	}

	public void LoadScript(int startingCheckpoint){
    //Debug.log("Load Script");
    //Debug.log("Chapter: "+Persistence.chapter_to_load+", Checkpoint: " +Persistence.checkpoint_to_load);

		Persistence.fastRead=true;
	   	commandController.waypoints = StoreWaypointsAndLoadInventoryAndCountLines(startingCheckpoint);
	    CreateScriptArray();
	    Persistence.fastRead=false;
		Debug.Log ("Script loaded");

    // initialize character animations
		//CreateCharacterAnims();
  }

  public void GoToLine(int line) {
    currentLine = line;
  }

  public int GetNextElseLine(){
    for (int i = currentLine+1 ; i < script.Length ; i++){
      string line = script [i];
      if (line == "else"){        
        return i;
      }   
    }

    Debug.LogError ("NO ELSE FOUND");

    return -1;
  }

  public int GetNextEndifLine(){
    for (int i = currentLine+1 ; i < script.Length ; i++){
      string line = script [i];
      if (line == "endif"){
        return i;
      }   
    }

    Debug.LogError ("NO ENDIF FOUND");

    return -1;
  }

	public void ReadScript(){

//    Debug.Log("Called ReadScript with line: " + startingLine);
    currentLine++;

    while(currentLine < script.Length){
      string line = script[currentLine];

      if( line.Length == 0 ){
        goto nextIteration;
      }

      if( line[0] == '/' || line[0] == '*' ){
        goto nextIteration;
      }

      commandController.CheckCommand(line, currentLine);
      if(commandController.CommandBreaksReading(line) == true){
        return;
      }

      nextIteration:
      currentLine++;
		}
	}

	void CreateScriptArray(){

		TextReader reader;
    if(Persistence.debugMode || false){
			FileStream scriptFile = new FileStream("script_debug.txt", FileMode.Open, FileAccess.Read);
			reader = new StreamReader(scriptFile);
		}else{
			reader = new StringReader(currentScript.text);
		}

    if(reader==null){
			Debug.Log("Error loading Script file");
    }


		script = new string[totalLines + 1];
		for(int i = 1; i < script.Length; i++){
      string line = reader.ReadLine ();
      if (line != null && line.StartsWith("\t"))
        line = line.Trim ();
      
      script [i] = line;
		}

		reader.Close();
	}

	Dictionary<string, int> StoreWaypointsAndLoadInventoryAndCountLines(int startingCheckpoint){
		Dictionary<string, int> waypoints = new Dictionary<string, int>();
		int lineCount = 1;
		bool canGetItem = true;
		string line = null;
		TextReader reader;
    if(Persistence.debugMode || false){
			FileStream scriptFile = new FileStream("script_debug.txt", FileMode.Open, FileAccess.Read);
			reader = new StreamReader(scriptFile);
		}else{
			reader = new StringReader(currentScript.text);
		}

		while( (line = reader.ReadLine()) != null )
		{


      
      if( line.Length<=1 ){
        lineCount++;
        continue;
      }

      if( line.Length>=4 ){		
				if( line.Substring(0, 4).ToLower() == "item" && canGetItem ){
					commandController.CheckCommand(line, lineCount);
				}
			}

			if( line.Length>=8 ){
				if( line.Substring(0, 8).ToLower() == "waypoint" ){
					int start = line.IndexOf("{");
					int end = line.IndexOf(" ", start + 1);
					if(end <= 0)
						end = line.Length - 1;
					else
						end--;
					string wp = line.Substring(start + 1, end - start);
          //Debug.Log (wp);
					waypoints.Add(wp, lineCount);
				}
			}

			if( line.Length>=10 ){
				if( line.Substring(0, 10).ToLower() == "checkpoint" ){
					string[] param = Command.GetParams(line);
					int i = int.Parse(param[0]);
					checkpoints[i] = lineCount;
					if( i == startingCheckpoint )
						canGetItem = false;
				}

				if( line.Substring(0, 10).ToLower() == "mouth_anim" ){
					commandController.CheckCommand(line, lineCount);
				}
			}
			
			if( line.Length>=12 ){
				if( line.Substring(0, 12).ToLower() == "handbook_btn" && canGetItem ){
					commandController.CheckCommand(line, lineCount);
				}
			}
			
			if( line.Length>=13 ){
				if( line.Substring(0, 13).ToLower() == "inventory_btn" && canGetItem ){
					commandController.CheckCommand(line, lineCount);
				}
			}

			if( line.Length>=14 ){				
				if( line.Substring(0, 14).ToLower() == "eye_blink_anim" ){
					commandController.CheckCommand(line, lineCount);
				}
			}

			lineCount++;
		}
		totalLines = lineCount;
		
		reader.Close();
		return waypoints;
	}


	public string PathForDocumentsFile(string filename) 
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			path = path.Substring( 0, path.LastIndexOf( '/' ) );
			return Path.Combine( Path.Combine( path, "Documents" ), filename );
		}
		
		else if(Application.platform == RuntimePlatform.Android){
			string resources = Path.Combine(Application.persistentDataPath, "Resources");
			string path = Path.Combine(resources, filename);
			return path;
		}
		else {
			string path = Application.dataPath + "/../" + filename;
			return path;
		}
	}
}
