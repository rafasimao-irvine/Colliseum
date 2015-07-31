using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindToHealAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(e.MyTile,e.GetVisionRange());
		
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop!=null && tiles[i].OnTop.GetBeAttackedTarget() is Enemy) {
				Enemy eOther = (Enemy)tiles[i].OnTop.GetBeAttackedTarget();
				if (eOther.GetLife() < eOther.GetMaxLife()) {
					e.SetPreparedAction(Enemy.ActionType.Heal, eOther);
					return true;
				}
			}
		}
		
		return false;
	}
		
}
