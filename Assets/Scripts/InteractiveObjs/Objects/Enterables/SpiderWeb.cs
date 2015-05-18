using UnityEngine;
using System.Collections;

public class SpiderWeb : Enterable {

	private int _NTurnsTrapped = 2;

	private bool _Trespassed = false;

	void Update () {
		if(_Trespassed)
			Destroy(gameObject);
	}

	public override bool BeEntered (Characther c) {
		if (c is Personage) ((Personage)c).InterruptActions();
		c.BeTrapped(_NTurnsTrapped);
		MyTile.TryGetOut(this);
		MyTile.TryGetIn(c);
		_Trespassed = true;
		return true;
	}

	public override bool BeLeft (Characther c) {
		return true;
	}
}
