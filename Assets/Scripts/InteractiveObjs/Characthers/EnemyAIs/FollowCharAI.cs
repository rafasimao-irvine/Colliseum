using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCharAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		PrepareAction(e,null);
		return true;
	}

	public bool PrepareAction (Enemy e, Characther targetChar) {
		Characther target = (targetChar==null) ? e.TargetChar : targetChar; 
		List<Tile> path = MapController.Instance.FindPath(e.MyTile, target.MyTile);
		
		// Start the movement
		if (path != null && path.Count > 0)
			e.SetPreparedAction(Enemy.ActionType.MoveWithAttack, path[0]);

		return true;
	}

}
