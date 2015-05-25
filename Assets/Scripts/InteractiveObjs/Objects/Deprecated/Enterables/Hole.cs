using UnityEngine;
using System.Collections;

public class Hole : Enterable {

	public override bool BeEntered (Characther c) {
		c.BeAttacked(this, c.GetLife()); // TODO create beKilled()

		return true;
	}

	public override bool BeLeft (Characther c) {
		Debug.Log("Warning: Hole is being left!");
		return true;
	}

}
