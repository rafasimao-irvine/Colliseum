using UnityEngine;
using System.Collections;

/**
 * Teleports the target(Characther) to a random place at the map.
 * */
public class TeleportEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther) {
			target.MyTile.TryGetOut(target);
			MapController.Instance.PlaceIt(MapController.Instance.GetMapTiles() , target);
		}
	}

}
