using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;

public class Command : MonoBehaviour {

  public Dictionary<string, int> waypoints = new Dictionary<string, int>();
  public int lineCount;

  public GameController game;
  public DialogScreen screen;
//  public Inventory inventory;
  public Fade fade;
  public GameObject inventoryButton;
  //public TopicLists topics;
  public Text letreiroText;
  public Image flashImage;

  void Awake() {
    game.lineCount = lineCount;
  }

  public bool CommandBreaksReading(string line) {
    string command = GetCommand(line);

    if(command == "wait" || command == "say" || command == "choices" ||
       command == "collect" || command == "fade_out" || command == "fade_in" ||
       command == "minigame_start" || command == "to_start_menu" ||
       command == "topic_new")
      return true;
    else
      return false;
  }

  public void CheckCommand(string line, int lineNumber) {
		
    //Debug.log("line " + lineNumber+": "+line );
    string command = GetCommand(line);
    string[] param = GetParams(line);
    ScriptReader.GetInstance().GoToLine(lineNumber);
		
    switch(command) {
      case "analytics_event":
        Analytics_Event_Command(param);
        break;
      case "player_prefs":
        Player_Prefs_Command(param);
        break;
      case "wait":
        Wait_Command(param);
        break;
      case "character":
        Character_Command(param);
        break;
      case "character_reset_all":
        Character_Reset_All_Command();
        break;
      case "char_sprite":
        Char_Sprite_Command(param);
        break;
      case "char_base":
        Char_Base_Command(param);
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
        Anim_Alpha_Command(param);
        break;
      case "anim_scale":
        Anim_Scale_Command(param);
        break;
      case "music":
        Music_Command(param);
        break;
      case "music_arg":
        Music_Arg_Command(param);
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
      case "minigame_start":
        Minigame_Start_Command(param);
        break;
      case "minigame_enable_skill":
        Minigame_Enable_Skill_Command();
        break;
      case "say":
        Say_Command(param);
        break;
      case "say_sfx":
        Say_Sfx_Command(param);
        break;
      case "say_pitch":
        Say_Pitch_Command(param);
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
      case "mirror":
        Mirror_Command(param);
        break;
      case "add_var":
        Add_Var_Command(param);
        break;
      case "if":
        If_Command(param);
        break;
      case "set_var":
        Set_Var_Command(param);
        break;
      case "save_game":
        Save_Game_Command(param);
        break;
      case "set_number_victories":
        Set_Number_Victories(param);
        break;
      case "checkpoint":
        Checkpoint_Command(param);
        break;
      case "to_next_chapter":
        To_Next_Chapter_Command();
        break;
      case "lettering":
        Lettering_Command (param);
        break;
      case "flash":
        Flash_Command (param);
        break;
    case "else":
      Else_Command ();
      break;
      default:
			//Debug.log("WRONG COMMAND: " + command);
        break;
    }
  }

  void Flash_Command(string[] param){

    float flashDuration = 0.5f;

    if (param.Length == 1){
      flashDuration = float.Parse(param [0]);
    }


    flashImage.GetComponent<CanvasGroup> ().alpha = 1f;

//    flashImage.GetComponent<CanvasGroup> ().DOFade (0f, flashDuration);
      
  }

  void Lettering_Command(string[] param){
//    // 0 - Text
//    // 1 - Delay Time
//    // 2 - Fade Time
//    float fadeTime = 2f;
//    float delayTime = 3f;
//
//    if (param.Length == 3){
//      delayTime = float.Parse(param [1]);
//      fadeTime = float.Parse(param [2]);
//    }
//    else if (param.Length == 2){
//      delayTime = float.Parse(param [1]);
//    }
//
//    string letteringText = param [0];
//
//    letreiroText.text = letteringText;
//    /letreiroText.GetComponent<CanvasGroup> ().DOFade (1f, fadeTime);
//    letreiroText.GetComponent<CanvasGroup> ().DOFade (0f, fadeTime).SetDelay(delayTime+fadeTime);
//
//    string[] waitCommandArguments = new string[1];
//    float totalWaitTime = fadeTime * 2 + delayTime;
//    waitCommandArguments[0] = "" + totalWaitTime;
//
//    //FIXME DOESN'T SEEM TO WORK?
//    Wait_Command (waitCommandArguments);
//

  }

  public static void Analytics_Event_Command(string[] param) {
    if(param.Length < 2) {
      //Debug.log("Error! Sent too few arguments to analytics command");
      return;
    }

    Analytics.CustomEvent("commandEvent", new Dictionary<string, object>{ { param[0], param[1] } });
  }

  public static void Player_Prefs_Command(string[] param) {
    PlayerPrefs.SetString(param[0], param[1]);
  }

  public void Save_Game_Command(string[] param) {
    //Persistence.GetInstance().SaveGame();
  }

  public static void CheckAnimCommand(string line, int lineNumber) {
		
    string command = GetStaticCommand(line);
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
    game.WaitSeconds(time);
  }

  void Music_Command(string[] args) {
    AudioController.GetInstance().PlayMusic(args[0]);
  }

  void Music_Arg_Command(string[] args) {
    AudioController.GetInstance().SetMusicArgument(float.Parse(args[0]));
  }

  void Ambience_Command(string[] args) {
    AudioController.GetInstance().PlayAmbience(args[0], float.Parse(args[1]));
  }

  void Fade_Out_Music_Command(string[] args) {
    AudioController.GetInstance().FadeMusic(float.Parse(args[0]));
  }

  void Sfx_Command(string[] args) {
    AudioController.GetInstance().PlaySfx(args[0]);
  }

  void Minigame_Start_Command(string[] args) {
//    /*
//     * arg0 = chapter
//     * arg1 = checkpoint_to_load
//     * arg2 = hasSkills
//     */
//    //Analytics.CustomEvent("startMinigame", null);
//    Persistence.chapter_to_load = 1;
//    Persistence.checkpoint_to_load = int.Parse(args[1]);
//    MinigameData.chapter = int.Parse(args[0]);
//
//    if (args.Length >= 3 && args[2] == "false"){
//      MinigameData.hasSkills = false;
//    } else {
//      MinigameData.hasSkills = true;
//    }
//   
//
//    SceneManager.LoadScene("Minigame");
  }

  void Minigame_Enable_Skill_Command() {
//    Debug.Log ("%%%%Skills have been enabled%%%%");
//    MinigameData.hasSkills = true;
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

  void Character_Command(string[] args) {
    int index = 0;
    string[,] chars = new string[6, 2];
    chars[0, 0] = "None";
    chars[1, 0] = "None";
    chars[2, 0] = "None";
    chars[3, 0] = "None";
    chars[4, 0] = "None";
    chars[5, 0] = "None";
    for(int i = 1; i < args.Length; i += 3) {
      index = (int.Parse(args[i - 1])) - 1;
      chars[index, 0] = args[i];
      chars[index, 1] = args[i + 1];
    }	
    screen.SetCharacter(chars);
  }

  void Character_Reset_All_Command() {
    string[] args = {"1", "none", "0",
      "2", "none", "0",
      "3", "none", "0",
      "4", "none", "0",
      "5", "none", "0",
      "6", "none", "0",
    };
    Character_Command(args);
  }

  void Char_Sprite_Command(string[] param) {
		
    int convertedInt = 0;
    string[] chars = new string[6];
    chars[0] = "None";
    chars[1] = "None";
    chars[2] = "None";
    chars[3] = "None";
    chars[4] = "None";
    chars[5] = "None";
    for(int i = 0; i < param.Length; i++) {
      if(i % 2 != 0) {
        convertedInt = game.charList.GetCharIdByParam(param[0]);
        chars[convertedInt] = param[i];
      }	
    }	
    screen.SetCharacterSprite(chars);
  }

  void Char_Base_Command(string[] param) {
    int convertedInt = 0;
    string[] chars = new string[6];
    chars[0] = "None";
    chars[1] = "None";
    chars[2] = "None";
    chars[3] = "None";
    chars[4] = "None";
    chars[5] = "None";
    for(int i = 0; i < param.Length; i++) {
      if(i % 2 != 0) {
        convertedInt = game.charList.GetCharIdByParam(param[0]);
        chars[convertedInt] = param[i];
      } 
    } 
    screen.SetCharacterBaseSprite(chars);
  }

  void Say_Command(string[] args) {		
		
    game.SetDialogState();
    if(args.Length > 1) {
      screen.SetDialog(args[0], args[1]);

      if((args[1])[0] != '(' && (args[1])[0] != '<')
        game.charList.MakeCharTalk(args[0]);
    } else {
      screen.SetDialog(args[0]);

      if((args[0])[0] != '(' && (args[0])[0] != '<')
        game.charList.MakeCharTalk(screen.GetTalkingCharName());
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

  void Say_Pitch_Command(string[] param) {		
    for(int i = 0; i < param.Length; i += 2) {
			
      string name = param[i];
      float pitch = float.Parse(param[i + 1]);
      AudioController.GetInstance().SetCharacterPitch(name, pitch);
    }
  }

  void Move_x_Command(string[] param) {
		
    int char_index = game.charList.GetCharIdByParam(param[0]);
    float anim_time = float.Parse(param[1]);
    float destination_x = float.Parse(param[2]);
		
    screen.SetMovement_x(char_index, anim_time, destination_x);
  }

  void Move_y_Command(string[] param) {
		
    int char_index = game.charList.GetCharIdByParam(param[0]);
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

  void Anim_Alpha_Command(string[] param) {

    int char_index = game.charList.GetCharIdByParam(param[0]);
    float speed = float.Parse(param[1]);
    float alpha = float.Parse(param[2]);
		
    screen.SetAlpha(char_index, speed, alpha);
  }

  void Anim_Scale_Command(string[] param) {

    int char_index = game.charList.GetCharIdByParam(param[0]);
    float speed = float.Parse(param[1]);
    float scale_y = float.Parse(param[2]);

    screen.SetScale(char_index, speed, scale_y);
  }

  void Question_Command(string[] text) {
		
    if(text.Length > 1) {
      screen.SetQuestion(text[0], text[1]);
			
      if((text[1])[0] != '(')
        game.charList.MakeCharTalk(screen.GetTalkingCharName());
    } else {
      screen.SetQuestion(text[0]);

      if(text[0] == null)
        return;
			
      if((text[0])[0] != '(')
        game.charList.MakeCharTalk(screen.GetTalkingCharName());
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
          //Debug.log("ERROR: " + "'" + param[i] + "'" + " is not a valid Waypoint. Choices_Command");
          choiceWaypoint[tempIndex] = 0;
        }
        tempIndex++;
      }
    }
    game.SetLabelNames(labelNames);
    screen.SetChoices(choiceText, choiceWaypoint);
  }

  




  void GotoNextElse(){
    int lineNumber = ScriptReader.GetInstance ().GetNextElseLine ();
    ScriptReader.GetInstance ().GoToLine (lineNumber  );
  }

  void GotoNextEndif(){
    int lineNumber = ScriptReader.GetInstance ().GetNextEndifLine ();
    ScriptReader.GetInstance ().GoToLine (lineNumber + 1);
  }
    

  public void Goto_Command(string[] label) {
    if(waypoints.ContainsKey(label[0])) {
      int lineNumber = waypoints[label[0]];
//      //Debug.log("The label " + label[0] + " has value: "+lineNumber);
      ScriptReader.GetInstance().GoToLine(lineNumber);
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


  void UpdateMusicArgument() {
    string[] musicArgs = new string[1];
    string[] ambienceArgs = new string[2];
    ambienceArgs[0] = "congresso_amb";

    if(screen.hypeLevels >= 80) {
      musicArgs[0] = "3";
    } else if(screen.hypeLevels >= 40) {
      musicArgs[0] = "2";
    } else {
      musicArgs[0] = "1";
    }
    ambienceArgs[1] = (Mathf.Max((float)screen.hypeLevels - 20f, 0f) / 100f).ToString();
    //Debug.log("hype level: " + screen.hypeLevels + ", ambience value: " + ambienceArgs[1]);

    Music_Arg_Command(musicArgs);
    Ambience_Command(ambienceArgs);
  }
		

  public void Fade_In_Command(string[] anim_time) {
    fade.FadeIn(float.Parse(anim_time[0]));
    Wait_Command(anim_time);
  }

  public void Fade_Out_Command(string[] anim_time) {
    fade.FadeOut(float.Parse(anim_time[0]));
    Wait_Command(anim_time);
  }

  void Mirror_Command(string[] param) {
    int char_index = game.charList.GetCharIdByParam(param[0]);
    screen.MirrorCharacter(char_index);
  }

  void Set_Var_Command(string[] param) {
    string key = param[0];
    int value = int.Parse(param[1]);
		
    Persistence.GetInstance().SetVariableValue(key, value);
  }

  void Set_Number_Victories(string[] param) {
    string key = param[0];

    Persistence.GetInstance().SetVariableValue(key, Persistence.matchVictories);
  }

  void If_Command(string[] param) {
    string varName = param[0];
    string operatorVal = param[1];
    int value = int.Parse(param[2]);
    string[] waypointName = new string[1];
    if (param.Length == 4)
      waypointName[0] = param[3];
    int varValue = Persistence.GetInstance().GetVariableValue(varName);
		
    OperationFactory op = new OperationFactory();
    Debug.Log("IF-" + varValue + "/" + operatorVal + "/" + value);
    if (op.Run(varValue, operatorVal, value) == false){
      if (param.Length == 4)
        Goto_Command (waypointName);
      else 
        GotoNextElse();
    }
  }

  void Else_Command(){
    GotoNextEndif ();
  }

  void Add_Relationship(string[] param) {
    string party = param[0];
    party = party.ToUpper();
    int value = int.Parse(param[1]);
    switch(party) {
      case "PDN":
        break;
      case "PN":
        break;
      case "PPC":
        break;
      default:
        Debug.Log("Wrong use of command add_relationship. Use the arguments PDN, PN or PPC");
        break;
    }

    Persistence.GetInstance().SetVariableValue(party, Persistence.GetInstance().GetVariableValue(party) + value);
  }

  void Add_Var_Command(string[] param) {
    string key = param[0];
    int value = int.Parse(param[1]);
		
    Persistence.GetInstance().SetVariableValue(key, Persistence.GetInstance().GetVariableValue(key) + value);
  }

  void Checkpoint_Command(string[] param) {
    int chapter = game.chapterId;
    int checkpoint = int.Parse(param[0]);
    string msg;

    Resources.UnloadUnusedAssets();

    // sound effect
    AudioController.GetInstance().PlayConfirmSound();

    // informative message
    if(Persistence.ReachCheckpoint(chapter, checkpoint)) {
      msg = "<color=#ac5100ff>[</color><color=#aa1100ff>O SEU PROGRESSO ESTÁ SALVO!</color><color=#ac5100ff> Você pode continuar o jogo a partir deste ponto através do Menu Inicial.]</color>";
    } else {
      msg = "<color=#ac5100ff>[</color><color=#aa1100ff>PONTO DE RETORNO ALCANÇADO!</color><color=#ac5100ff> Você pode continuar o jogo a partir deste ponto através do Menu Inicial.]</color>";
    }
    string[] info_msg = { "", msg };
    Say_Command(info_msg);
  }

  void To_Next_Chapter_Command() {
    game.AdvanceChapter();
    game.WaitSeconds(3f);
  }

  public static string GetStaticCommand(string line) {
    int start = line.IndexOf("@");
    int end = line.IndexOf(" ", start + 1);
    if(end <= 0)
      end = line.Length - 1;
    else
      end--;
    string command = line.Substring(start + 1, end - start);
    return command.ToLower();
  }

  public string GetCommand(string line) {    

    int end = line.IndexOf(" ", 0);
    line = Regex.Match (line, "[a-zA-Z_]+").Value;
    Debug.Log (line);
    if(end <= 0) {
      end = line.Length;
    }
    return line.ToLower();

//    int end = line.IndexOf(" ", 0);
//    if(end <= 0) {
//      end = line.Length;
//    }
//    return line.Substring(0, end).ToLower();

  }

  public static string[] GetParams(string line) {

    List<string> arrayStrings = new List<string> ();

    Match match = Regex.Match (line, @"({[^{]*)+");

    Debug.Log ("Command: " + line);

    foreach (Capture c in match.Groups[1].Captures){
      string value = c.Value;

      value = Regex.Replace (value, "^{", "");
      value = value.Trim ();

      Debug.Log ("Arg: " + value);

      arrayStrings.Add (value);
    }
      




    return arrayStrings.ToArray ();
//    
//    IList<int> foundIndexes = new List<int>();
//		
//    for(int i = line.IndexOf("{"); i > -1; i = line.IndexOf("{", i + 1)) {
//      foundIndexes.Add(i);
//    }
//    string[] paramArray = new string[foundIndexes.Count];
//		
//    int j = 0;
//    char[] tempChars = new char[2];
//    tempChars[0] = '>';
//    tempChars[1] = '<';
//    foreach(int i in foundIndexes) {
//      string param = "";
//      string thisChar = line.Substring(i, 1);
//			
//      if(thisChar == "{") {
//        int textStart = i;
//        int textEnd = line.IndexOf('{', textStart + 1);
//        if(textEnd <= 0)
//          textEnd = line.Length - 1;
//        else
//          textEnd -= 2;
//        param = line.Substring(textStart + 1, textEnd - textStart);
//      } else if(thisChar == "#") {
//        int paramStart = i;
//        int paramEnd = line.IndexOf(" ", paramStart + 1);
//        if(paramEnd <= 0)
//          paramEnd = line.Length - 1;
//        else
//          paramEnd -= 2;
//        param = line.Substring(paramStart + 1, paramEnd - paramStart);
//      }
//      paramArray[j] = param;
//      j++;
//    }
//    return paramArray;

  }
}
