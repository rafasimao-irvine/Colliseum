using UnityEngine;
using System.Collections;

public class FakeBuffer : Interactable {

	private int _Damage = 1;
	
	public override void BeAttacked (Interactive iObj, int damage) {
		if (iObj is Characther)
			((Characther)iObj).BeAttacked(this, _Damage);

		MyTile.TryGetOut(this);
		Destroy(gameObject);
	}
}
