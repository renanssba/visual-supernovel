using UnityEngine;
using System.Collections;

public class OperationFactory {

	public bool Run(int varValue, string operatorVal, int value){

		switch(operatorVal) {
			case "==": return varValue == value;
			case ">": return varValue > value;
			case "<": return varValue < value;
			case "<=": return varValue <= value;
			case ">=": return varValue >= value;
			case "!=": return varValue != value;
			default: throw new UnityException("Wrong Operator");
		}
	}
}
