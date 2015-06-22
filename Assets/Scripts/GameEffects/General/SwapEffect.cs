using UnityEngine;
using System.Collections;

public class SwapEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (origin!=null && origin.GetBeAttackedTarget() is Characther && 
		    target!=null && target.GetBeAttackedTarget() is Characther) {
			origin = origin.GetBeAttackedTarget();
			target = target.GetBeAttackedTarget();

			MapController map = MapController.Instance;

			Tile oTile = origin.MyTile;
			Tile tTile = target.MyTile;

			oTile.TryGetOut(origin);
			tTile.TryGetOut(target);

			map.PlaceItAt(origin,tTile);
			map.PlaceItAt(target,oTile);
		}
	}

}
