using UnityEngine;
using System.Collections;

public class FollowAndFusionAI : EnemyAI {

	MoveToFusionAI _MoveToFusionAI;
	FollowCharAI _FollowCharAI;

	public FollowAndFusionAI () {
		_MoveToFusionAI = new MoveToFusionAI();
		_FollowCharAI = new FollowCharAI();
	}

	public override bool PrepareAction (Enemy e) {
		if (!_MoveToFusionAI.PrepareAction(e))
			_FollowCharAI.PrepareAction(e);

		return true;
	}

}
