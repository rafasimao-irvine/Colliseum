using UnityEngine;
using System.Collections;

public class Obstacle : Interactive {

	void Awake () {
		Blockable = true;
		Attackable = false;
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		Debug.Log("Warning: Obstacle is being attacked.");
	}

	public override bool BeEntered (Characther c) {
		Debug.Log("Warning: Obstacle is being entered.");
		return false;
	}

	public override bool BeLeft (Characther c) {
		Debug.Log("Warning: Obstacle is being left.");
		return true;
	}
}
