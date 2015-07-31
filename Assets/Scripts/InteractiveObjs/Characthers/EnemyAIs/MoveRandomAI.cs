using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveRandomAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(e.MyTile);
		
		if (tiles.Count > 0) {
			Tile tile = tiles[Random.Range(0,tiles.Count)];
			e.SetPreparedAction(Enemy.ActionType.Move, tile);
		}

		return true;
	}
}
