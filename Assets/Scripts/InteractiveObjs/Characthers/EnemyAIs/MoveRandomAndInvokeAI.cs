using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveRandomAndInvokeAI : EnemyAI {

	MoveRandomAI _MoveRandomAI;

	public MoveRandomAndInvokeAI () {
		_MoveRandomAI = new MoveRandomAI();
	}

	public override bool PrepareAction (Enemy e) {
		List<Tile> neighbours = MapController.Instance.GetNeighbours(e.MyTile);
		for (int i=0; i<neighbours.Count; i++) {
			if (neighbours[i].OnTop == null) {
				e.SetPreparedAction(Enemy.ActionType.Invoke, neighbours[i]);
				i = neighbours.Count;
			}
		}
		
		if (e.PreparedAction.Type == Enemy.ActionType.None)
			_MoveRandomAI.PrepareAction(e);

		return true;
	}
}
