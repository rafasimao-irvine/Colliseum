using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowAndPlaceTrapAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		MapController map = MapController.Instance;
		
		Vector2 direction = 
			(map.GetDistance(e.MyTile,e.TargetChar.MyTile) > e.GetVisionRange()) ?
				map.GetDirection(e.MyTile, e.TargetChar.MyTile) : 
				map.GetDirection(e.TargetChar.MyTile, e.MyTile);
		
		List<Tile> tiles = map.GetFrontArea(e.MyTile, direction);
		
		for (int i=0; i<tiles.Count; i++) {
			if (e.IsMoveable(tiles[i])) {
				e.SetPreparedAction(Enemy.ActionType.MoveAndPlaceTrap, tiles[i]);
				i = tiles.Count;// out of loop
			}
		}

		return true;
	}

}
