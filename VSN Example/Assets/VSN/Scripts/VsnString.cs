using System;

public class VsnString : VsnArgument{

  protected string stringValue;

  public VsnString(string text){
		this.stringValue = text;
  }

  public override VsnArgumentType GetArgumentType(){
    return VsnArgumentType.stringArg;
  }

  public override string GetStringValue(){
    return stringValue;
  }
}

