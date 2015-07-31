using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveToFusionAI : EnemyAI {

	FollowCharAI _FollowCharAI;

	public MoveToFusionAI () {
		_FollowCharAI = new FollowCharAI();
	}

	public override bool PrepareAction (Enemy e) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(e.MyTile,e.GetVisionRange());
		
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop!=null && tiles[i].OnTop.GetBeAttackedTarget() is Enemy) {
				Enemy eOther = (Enemy)tiles[i].OnTop.GetBeAttackedTarget();
				if (eOther.gameObject.name.Equals(e.gameObject.name)) {
					if (MapController.Instance.GetDistance(e.MyTile,eOther.MyTile) > 1)
						_FollowCharAI.PrepareAction(eOther);
					else {
						e.SetPreparedAction(Enemy.ActionType.Fusion, eOther);
						eOther.SetPreparedAction(Enemy.ActionType.None);
					}
					return true;
				}
			}
		}
		
		return false;
	}

}
