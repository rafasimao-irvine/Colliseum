using UnityEngine;
using System.Collections;

public abstract class Attackable : Interactive {

	void Awake () {
		Blockable = true;
		Attackable = true;
		Interactable = false;
	}

	public override bool BeEntered (Characther c) {
		Debug.Log("Warning: "+GetType()+" is being entered!");
		return false;
	}
	
	public override bool BeLeft (Characther c) {
		Debug.Log("Warning: "+GetType()+" is being left!");
		return true;
	}

}
