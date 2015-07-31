using UnityEngine;
using System.Collections;

public class AttackCharAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		if (e.IsAttackReady() && e.TargetChar!=null)
			e.SetPreparedAction(Enemy.ActionType.Attack, e.TargetChar.MyTile);

		return true;
	}

}
