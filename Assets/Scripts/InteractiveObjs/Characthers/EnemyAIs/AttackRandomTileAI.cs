using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRandomTileAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		if (e.IsAttackReady()) {
			List<Tile> neighbours = MapController.Instance.GetNeighbours(e.MyTile);
			e.SetPreparedAction(Enemy.ActionType.Attack, neighbours[Random.Range(0,neighbours.Count)]);
			
			return true;
		}
		return false;
	}

}
