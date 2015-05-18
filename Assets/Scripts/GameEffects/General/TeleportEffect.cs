using UnityEngine;
using System.Collections;

public class TeleportEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther) {
			target.MyTile.TryGetOut(target);
			MapController.Instance.PlaceIt(MapController.Instance.GetMapTiles() , target);
		}
	}

}
