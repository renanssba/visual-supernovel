using System;

public class VsnVariableReference : VsnArgument{

  protected string variableReferenceValue;

	public VsnVariableReference(string variableName){
		variableReferenceValue = variableName;
	}

  public override VsnArgumentType GetArgumentType(){
    return VsnArgumentType.variableArg;
  }

  public override float GetNumberValue(){
    return VsnSaveSystem.GetFloatVariable(variableReferenceValue);
  }

  public override string GetStringValue(){
    return VsnSaveSystem.GetStringVariable(variableReferenceValue);
  }

  public override string GetVariableReference(){
    return variableReferenceValue;
  }
}

