using System;

public class VsnString : VsnArgument{
	
	public VsnString(string text){
		this.stringValue = text; //remove quotes from argument
	}

}

