using UnityEngine;
using System.Collections;

public class ComparisonEvaluator {

	public static bool Evaluate(int firstOperand, string operatorVal, int secondOperand){

		switch(operatorVal) {
			case "==":
        return firstOperand == secondOperand;
			case ">":
        return firstOperand > secondOperand;
			case "<":
        return firstOperand < secondOperand;
			case "<=":
        return firstOperand <= secondOperand;
			case ">=":
        return firstOperand >= secondOperand;
			case "!=":
        return firstOperand != secondOperand;
			default:
        return false;
		}
	}
}
