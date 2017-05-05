using System;

public class VsnNumber : VsnArgument{

  protected float floatValue;

	public VsnNumber(float number){
		this.floatValue = number;
  }

  public override VsnArgumentType GetArgumentType(){
    return VsnArgumentType.numberArg;
  }

  public override float GetNumberValue(){
    return floatValue;
  }
}
