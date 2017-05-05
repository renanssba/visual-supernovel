using System;
using UnityEngine;

public class VsnOperator : VsnArgument{

  string operatorType;

  public VsnOperator(string type){
    operatorType = type;
  }

  public override VsnArgumentType GetArgumentType(){
    return VsnArgumentType.operatorArg;
  }

  public bool EvaluateComparison(VsnArgument first, VsnArgument second){
    if((first.GetArgumentType() == VsnArgumentType.numberArg && second.GetArgumentType() == VsnArgumentType.numberArg) ||
       (first.GetArgumentType() == VsnArgumentType.variableArg && second.GetArgumentType() == VsnArgumentType.numberArg) ||
       (first.GetArgumentType() == VsnArgumentType.numberArg && second.GetArgumentType() == VsnArgumentType.variableArg) ){
      return CompareFloats(first.GetNumberValue(), second.GetNumberValue());
    }

    if((first.GetArgumentType() == VsnArgumentType.stringArg && second.GetArgumentType() == VsnArgumentType.stringArg) ||
       (first.GetArgumentType() == VsnArgumentType.variableArg && second.GetArgumentType() == VsnArgumentType.stringArg) ||
       (first.GetArgumentType() == VsnArgumentType.stringArg && second.GetArgumentType() == VsnArgumentType.variableArg) ){
      return CompareStrings(first.GetStringValue(), second.GetStringValue());
    }

    return CompareVariables(first, second);

    return false;
  }


  private bool CompareFloats(float op1, float op2){
    switch(operatorType){
      case "==":
        if(op1 == op2) {
          return true;
        }
        break;
      case "!=":
        if(op1 != op2) {
          return true;
        }
        break;
      case "<=":
        if(op1 <= op2) {
          return true;
        }
        break;
      case ">=":
        if(op1 >= op2) {
          return true;
        }
        break;
      case "<":
        if(op1 < op2) {
          return true;
        }
        break;
      case ">":
        if(op1 > op2) {
          return true;
        }
        break;
    }
    return false;
  }


  private bool CompareStrings(string op1, string op2){
    switch(operatorType){
      case "==":
        if(op1 == op2) {
          return true;
        }
        break;
      case "!=":
        if(op1 != op2) {
          return true;
        }
        break;
    }
    return false;
  }


  private bool CompareVariables(VsnArgument op1, VsnArgument op2){
    /// TODO: also implement when the two variables are different types
    /// or when they're both strings

    return CompareFloats(op1.GetNumberValue(), op2.GetNumberValue());
  }
}
