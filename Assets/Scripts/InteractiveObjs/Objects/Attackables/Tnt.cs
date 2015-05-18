using UnityEngine;
using System.Collections;

public class Tnt : Attackable {

	private int Damage = 1;

	public override void BeAttacked (Interactive i, int damage) {

		MyTile.TryGetOut(this);

		foreach (Tile t in MapController.Instance.GetNeighbours(MyTile,1)) {
			if (t.OnTop != null)
				t.OnTop.BeAttacked(this, Damage);
		}

		Destroy(gameObject);
	}

}
