using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VsnCommand{

	public abstract void Execute ();

	public abstract void PrintName ();

	public abstract void InjectArguments(List<VsnArgument> args);
}
