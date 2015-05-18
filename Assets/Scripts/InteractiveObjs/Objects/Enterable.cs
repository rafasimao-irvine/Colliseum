using UnityEngine;
using System.Collections;

public abstract class Enterable : Interactive {

	void Awake () {
		Blockable = false;
		Attackable = false;
		Interactable = false;
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		Debug.Log("Warning: "+GetType()+" is being attacked!");
	}
}
