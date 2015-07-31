using UnityEngine;
using System.Collections;

public class RunAndAlarmAI : EnemyAI {

	RunFromCharAI _RunFromCharAI;

	public RunAndAlarmAI () {
		_RunFromCharAI = new RunFromCharAI();
	}

	public override bool PrepareAction (Enemy e) {
		_RunFromCharAI.PrepareAction(e);
		e.SetPreparedAction(Enemy.ActionType.MoveAndAlarm ,
		                    e.PreparedAction.TargetTile, e.PreparedAction.TargetChar);

		return true;
	}

}
