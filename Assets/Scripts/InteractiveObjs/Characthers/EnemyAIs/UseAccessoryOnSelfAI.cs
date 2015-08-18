using UnityEngine;
using System.Collections;

public class UseAccessoryOnSelfAI : EnemyAI {

	public override bool PrepareAction (Enemy e) {
		e.SetPreparedAction(Enemy.ActionType.UseAccessory, e.MyTile);
		
		return true;
	}

}
