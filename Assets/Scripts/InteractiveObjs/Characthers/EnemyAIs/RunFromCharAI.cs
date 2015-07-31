using UnityEngine;
using System.Collections;

public class RunFromCharAI : EnemyAI {

	MoveRandomAI _MoveRandomAI;

	public RunFromCharAI () {
		_MoveRandomAI = new MoveRandomAI();
	}

	public override bool PrepareAction (Enemy e)
	{
		MapController map = MapController.Instance;
		Tile next = map.GetNextTile(e.MyTile, map.GetDirection(e.TargetChar.MyTile, e.MyTile));
		if (next!=null && (next.OnTop==null || (next.OnTop!=null && !next.OnTop.Blockable)) )
			e.SetPreparedAction(Enemy.ActionType.Move, next);
		else 
			_MoveRandomAI.PrepareAction(e);

		return true;
	}

}
