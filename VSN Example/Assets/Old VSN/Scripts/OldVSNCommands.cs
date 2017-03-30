using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;

public class OldVSNCommands : MonoBehaviour {

  public static OldVSNCommands instance;

  public Dictionary<string, int> waypoints = new Dictionary<string, int>();

  public DialogScreen screen;
  public Fade fade;


  void Awake(){
    instance = this;
  }

  public static OldVSNCommands GetInstance(){
    return instance;
  }


  public bool CommandBreaksReading(string line) {
    string command = GetCommand(line);

    if(command == "wait" || command == "say" || command == "say_hobby" || command == "say_taste" || command =="say_crush" ||
       command == "current_introduction" || command == "choices" || command == "collect" || command == "fade_out" ||
       command == "fade_in" || command == "to_start_menu" || command == "topic_new" || command == "wait_answer_confirm" ||
       command == "end_script" || command == "change_sublocation" || command == "discover_random_taste")
      return true;
    else
      return false;
  }

  public void CheckCommand(string line, int lineNumber) {
		
    //Debug.log("line " + lineNumber+": "+line );
    string command = GetCommand(line);
    string[] param = GetParams(line);
    OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
		
    switch(command) {
      case "player_prefs":
        Player_Prefs_Command(param);
        break;
      case "wait":
        Wait_Command(param);
        break;
      case "wait_answer_confirm":
        WaitAnswerConfirm_Command(param);
        break;     
      case "bg":
        Bg_Command(param);
        break;
      case "fg":
        Fg_Command(param);
        break;
      case "move_x":
        Move_x_Command(param);
        break;
      case "move_y":
        Move_y_Command(param);
        break;
      case "mouth_anim":
        Mouth_Anim_Command(param);
        break;
      case "eye_blink_anim":
        Eye_Blink_Anim_Command(param);
        break;
      case "anim_alpha":
        AnimAlpha_Command(param);
        break;
      case "anim_scale":
        Anim_Scale_Command(param);
        break;
      case "music":
        Music_Command(param);
        break;
      case "ambience":
        Ambience_Command(param);
        break;
      case "fade_out_music":
        Fade_Out_Music_Command(param);
        break;
      case "sfx":
        Sfx_Command(param);
        break;
      case "say":
        Say_Command(param);
        break;
      case "say_sfx":
        Say_Sfx_Command(param);
        break;
      case "question":
        Question_Command(param);
        break;
      case "choices":
        Choices_Command(param);
        break;

      case "goto":
        Goto_Command(param);
        break;
      case "waypoint":
        break;
      case "screenshake":
        Screenshake_Command(param);
        break;
      case "fade_in":
        Fade_In_Command(param);
        break;
      case "fade_out":
        Fade_Out_Command(param);
        break;
      case "fade_to":
        Fade_To_Command(param);
        break;
      case "mirror":
        Mirror_Command(param);
        break;
      case "add_var":
        Add_Var_Command(param);
        break;
      case "if":
        If_Command(param);
        break;
      case "else":
        Else_Command();
        break;
      case "set_var":
        Set_Var_Command(param);
        break;
      case "set_string":
        Set_String_Command(param);
        break;
      case "save_game":
        Save_Game_Command(param);
        break;
      case "end_script":
        EndScript_Command();
        break;
      case "goto_script":
        GotoScript_Command(param);
        break;
      case "resume_script":
        ResumeScript_Command(param);
        break;
      default:
  			//Debug.log("WRONG COMMAND: " + command);
        break;
    }
  }

  void EndScript_Command() {
    Debug.Log("Calling endscript");
    OldVSNController.GetInstance().PauseVSN();
    OldVSNController.GetInstance().ClearScriptPositionStack();
  }

  void GotoScript_Command(string[] param) {
    OldVSNController.GetInstance().PauseVSN();
    OldVSNController.GetInstance().SaveScriptPositionInStack();
    OldVSNController.GetInstance().StartVSNScript(param[0]);
  }

  void ResumeScript_Command(string[] param) {
    OldVSNController.ScriptPosition pos = OldVSNController.GetInstance().LastScriptStackEntry();
    OldVSNController.GetInstance().PauseVSN();
    if(param.Length > 0){
      OldVSNController.GetInstance().StartVSNScript(pos.scriptName, param[0]);
    }else{
      OldVSNController.GetInstance().StartVSNScript(pos.scriptName, pos.scriptLine);
    }
  }		

  void ShowLoadingIcon_Command(string[] param){
    screen.SetLoadingIcon(param[0]=="true" ? true : false);
  }

  void ShowTutorialIcon_Command(string[] param){
//    GameMapManager.GetInstance().ShowTutorialArrow(int.Parse(param[0]), param[1]=="true" ? true : false);
  }		

  public static void Player_Prefs_Command(string[] param) {
    PlayerPrefs.SetString(param[0], param[1]);
  }

  public void Save_Game_Command(string[] param) {
    //Persistence.GetInstance().SaveGame();
  }

  public static void CheckAnimCommand(string line, int lineNumber) {
		
    string command = GetCommand(line);
    string[] param = GetParams(line);
		
    switch(command) {
      case "mouth_anim":
        Mouth_Anim_Command(param);
        break;
      case "eye_blink_anim":
        Eye_Blink_Anim_Command(param);
        break;
      default:
        break;
    }
  }

  void Wait_Command(string[] waitTime) {
    float time = float.Parse(waitTime[0]);
    screen.EnableDialogBox(false);
    screen.choices.SetActive(false);
    OldVSNController.GetInstance().WaitSeconds(time);
  }

  void WaitAnswerConfirm_Command(string[] param){
    screen.EnableDialogBox(false);
    screen.choices.SetActive(false);
    OldVSNController.GetInstance().WaitForValidation(param[0], param[1]);
  }


  void Music_Command(string[] args) {
//    AudioController.GetInstance().PlayMusic(args[0]);
  }

  void Ambience_Command(string[] args) {
//    AudioController.GetInstance().PlayAmbience(args[0], float.Parse(args[1]));
  }

  void Fade_Out_Music_Command(string[] args) {
//    AudioController.GetInstance().FadeMusic(float.Parse(args[0]));
  }

  void Sfx_Command(string[] args) {
//    AudioController.GetInstance().PlaySfx(args[0]);
//    SoundManager.GetInstance().PlaySfx(args[0]);
  }

  void Bg_Command(string[] args) {
    screen.SetBg(args[0]);
    if(args.Length >= 2) {
      screen.SetFg(args[1]);
    }
  }

  void Fg_Command(string[] args) {
    screen.SetFg(args[0]);
  }		

  void Say_Command(string[] param) {

    OldVSNController.GetInstance().SetDialogState();
    if(param.Length > 1) {
      screen.SetDialog(param[0], param[1]);
      OldVSNController.GetInstance().charList.MakeCharTalk(param[0]);
    } else {
      screen.SetDialog(param[0]);
      OldVSNController.GetInstance().charList.MakeCharTalk(screen.GetTalkingCharName());
    }
  }
		
  void Say_Sfx_Command(string[] param) {
		
    string name;
    int interval;
    if(param.Length > 1) {
      name = param[0];
      interval = int.Parse(param[1]);
      screen.SetDialogSfx(name);
      screen.SetSfxInterval(interval);
      return;
    } else {
      bool result = int.TryParse(param[0], out interval);
      if(result) {
        screen.SetSfxInterval(interval);
        return;
      } else {
        name = param[0];
        screen.SetDialogSfx(name);
        return;
      }
    }
  }

  void Move_x_Command(string[] param) {
		
    int char_index = OldVSNController.GetInstance().charList.GetCharIdByParam(param[0]);
    float anim_time = float.Parse(param[1]);
    float destination_x = float.Parse(param[2]);
		
    screen.SetMovement_x(char_index, anim_time, destination_x);
  }

  void Move_y_Command(string[] param) {
		
    int char_index = OldVSNController.GetInstance().charList.GetCharIdByParam(param[0]);
    float anim_time = float.Parse(param[1]);
    float destination_y = float.Parse(param[2]);

    screen.SetMovement_y(char_index, anim_time, destination_y);
  }

  static void Mouth_Anim_Command(string[] param) {

    string animation_name = param[0];
    string[] sprite_names = new string[param.Length - 1];	
    int j = 1;
		
    for(int i = 0; i < sprite_names.Length; i++) {
      sprite_names[i] = param[j];
      j++;
    }
    CharacterAnimations.CreateMouthAnimation(animation_name);
    CharacterAnimations.AssignMouthAnimation(animation_name, sprite_names);
  }

  static void Eye_Blink_Anim_Command(string[] param) {

    string animation_name = param[0];
    string[] sprite_names = new string[param.Length - 1];
    int j = 1;
		
    for(int i = 0; i < sprite_names.Length; i++) {
      sprite_names[i] = param[j];
      j++;
    }
    CharacterAnimations.CreateEyeAnimation(animation_name);
    CharacterAnimations.AssignEyeAnimation(animation_name, sprite_names);
  }

  void AnimAlpha_Command(string[] param) {

    int char_index = OldVSNController.GetInstance().charList.GetCharIdByParam(param[0]);
    float alpha = float.Parse(param[1]);
    float time = float.Parse(param[2]);
		
    CharacterList.GetInstance().AnimateAlpha(char_index, time, alpha);
  }

  void Anim_Scale_Command(string[] param) {

    int char_index = OldVSNController.GetInstance().charList.GetCharIdByParam(param[0]);
    float scale = float.Parse(param[1]);
    float time = float.Parse(param[2]);

    screen.SetScale(char_index, time, scale);
  }

  void Question_Command(string[] param) {

    if(param.Length > 1) {
      screen.SetQuestion(param[0], param[1]);
			
      if((param[1])[0] != '(')
        OldVSNController.GetInstance().charList.MakeCharTalk(screen.GetTalkingCharName());
    } else {
      screen.SetQuestion(param[0]);

      if(param[0] == null)
        return;
			
      if((param[0])[0] != '(')
        OldVSNController.GetInstance().charList.MakeCharTalk(screen.GetTalkingCharName());
    }
  }


  void Choices_Command(string[] param) {
		
    string[] choiceText = new string[param.Length / 2];
    int[] choiceWaypoint = new int[param.Length / 2];
    string[] labelNames = new string[param.Length / 2];
    int tempIndex = 0;


		
    for(int i = 0; i < param.Length; i++) {
      if(i % 2 == 0) {
        choiceText[tempIndex] = param[i];
      } else {
        labelNames[tempIndex] = param[i];
        if(waypoints.ContainsKey(param[i])) {
          choiceWaypoint[tempIndex] = waypoints[param[i]];
        } else {
          choiceWaypoint[tempIndex] = 0;
        }
        tempIndex++;
      }
    }
    OldVSNController.GetInstance().SetLabelNames(labelNames);
    screen.SetChoices(choiceText, choiceWaypoint);
  }
  




  void GotoNextElseOrEndif() {
    int lineNumber = OldVSNScriptReader.GetInstance().GetElseOrEndifLine();
    OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
  }

  void GotoNextEndif() {
    int lineNumber = OldVSNScriptReader.GetInstance().GetEndifLine();
    OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
  }


  public void Goto_Command(string[] label) {
    if(waypoints.ContainsKey(label[0])) {
      int lineNumber = waypoints[label[0]];
//      //Debug.log("The label " + label[0] + " has value: "+lineNumber);
      OldVSNScriptReader.GetInstance().GoToLine(lineNumber);
    }
  }


  void Screenshake_Command(string[] param) {
    Debug.Log("Screenshake happening");
		
    if(param.Length != 1)
      Debug.Log("Incorrect use of Screenshake. Use Screenshake {x, where x is the intensity from 1 to 10");
    else {
      int amount = int.Parse(param[0]);
      if(amount >= 1 && amount <= 10)
        screen.Screenshake(amount);
    }
  }


  public void Fade_In_Command(string[] args) {
    fade.FadeIn(float.Parse(args[0]));
    Wait_Command(args);
  }

  public void Fade_Out_Command(string[] anim_time) {
    fade.FadeOut(float.Parse(anim_time[0]));
    Wait_Command(anim_time);
  }

  public void Fade_To_Command(string[] param) {
    fade.FadeTo(float.Parse(param[0]), float.Parse(param[1]));
    Wait_Command(param);
  }

  void Mirror_Command(string[] param) {
    int char_index = OldVSNController.GetInstance().charList.GetCharIdByParam(param[0]);
    screen.MirrorCharacter(char_index);
  }

  void Set_Var_Command(string[] param) {
    string key = param[0];
	int value = int.Parse (param [1]);
		
    Persistence.GetInstance().SetVariableValue(key, value);
  }

  void Set_String_Command(string[] param){
    Persistence.GetInstance().SetCustomString(param[0]);
  }

  void If_Command(string[] param) {
	int firstOperand = int.Parse (param [0]);
    string operatorName = param[1];
	int secondOperand =  int.Parse (param [2]);
		
    Debug.Log("If " + firstOperand + " " + operatorName + " " + secondOperand);
    if(ComparisonEvaluator.Evaluate(firstOperand, operatorName, secondOperand) == false) {
      GotoNextElseOrEndif();      
    } // else, continue running and enter the if clause
  }

  void Else_Command() {
    GotoNextEndif();
  }



  void Add_Var_Command(string[] param) {
    string key = param[0];
		int value = int.Parse (param [1]);
		
    Persistence.GetInstance().SetVariableValue(key, Persistence.GetInstance().GetVariableValue(key) + value);
  }

  public static string GetCommand(string line) {

    line = Regex.Match(line, "[a-zA-Z_/]+").Value;
//    Debug.LogWarning("Command cleaned up: " + line);
    return line.ToLower();
  }

  public static string[] GetParams(string line) {

    List<string> arrayStrings = new List<string>();

    Match match = Regex.Match(line, @"({[^{]*)+");

    Debug.Log("Command: " + line);

    foreach(Capture c in match.Groups[1].Captures) {
      string value = c.Value;

      value = Regex.Replace(value, "^{", "");
      value = value.Trim();

//      Debug.Log("Arg: " + value);

      arrayStrings.Add(value);
    }

    return arrayStrings.ToArray();
  }
}
