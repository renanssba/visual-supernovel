using System;

public class VsnVariableReference : VsnArgument{

	string value;

	public VsnVariableReference(string variableName){
		this.value = variableName;
	}

}

