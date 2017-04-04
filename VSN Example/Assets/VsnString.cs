using System;

public class VsnString : VsnArgument{
	
	public VsnString(string text){
		this.stringValue = text.Substring(1, text.Length-2); //remove quotes from argument
	}

}

