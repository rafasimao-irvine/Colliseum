using UnityEngine;
using System.Collections;

public class MoveRandomAndFusionAI : EnemyAI {

	MoveToFusionAI _MoveToFusionAI;
	MoveRandomAI _MoveRandomAI;

	public MoveRandomAndFusionAI () {
		_MoveToFusionAI = new MoveToFusionAI();
		_MoveRandomAI = new MoveRandomAI();
	}

	public override bool PrepareAction (Enemy e) {
		if (!_MoveToFusionAI.PrepareAction(e))
			_MoveRandomAI.PrepareAction(e);

		return true;
	}

}
