using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VsnArgumentType{
  numberArg,
  stringArg,
  variableArg,
  operatorArg
}

public abstract class VsnArgument{
	
  public abstract VsnArgumentType GetArgumentType();

  public virtual float GetNumberValue(){
    return 0f;
  }

  public virtual string GetStringValue(){
    return "";
  }

  public virtual string GetVariableReference(){
    return "";
  }
}

