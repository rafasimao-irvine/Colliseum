using UnityEngine;
using System.Collections;

public class UseAccessoryOnTargetAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		if (e.TargetChar!=null && !e.TargetChar.IsDead())
			e.SetPreparedAction(Enemy.ActionType.UseAccessory,e.TargetChar.MyTile);

		return true;
	}

}
