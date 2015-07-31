using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCharAI : EnemyAI {

	public Characther TargetChar;

	public override bool PrepareAction (Enemy e) {
		if (TargetChar==null) TargetChar = e.TargetChar;
		List<Tile> path = MapController.Instance.FindPath(e.MyTile, TargetChar.MyTile);
		
		// Start the movement
		if (path != null && path.Count > 0)
			e.SetPreparedAction(Enemy.ActionType.MoveWithAttack, path[0]);

		return true;
	}

}
