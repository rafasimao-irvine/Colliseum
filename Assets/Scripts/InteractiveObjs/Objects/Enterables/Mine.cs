using UnityEngine;
using System.Collections;

public class Mine : Enterable {

	private bool _Trespassed = false;

	private Characther _CharInto;

	void Update () {
		if(_Trespassed) {
			_CharInto.BeAttacked(this, 1);
			Destroy(gameObject);
		}
	}

	public override bool BeEntered (Characther c) {
		if (_CharInto==null) {

			MyTile.TryGetOut(this);
			MyTile.TryGetIn(c);
			_Trespassed = true;

			_CharInto = c;

			return true;
		}
		return false;
	}
	
	public override bool BeLeft (Characther c) {
		Debug.Log("Warning: Hole is being left!");
		return true;
	}

}
