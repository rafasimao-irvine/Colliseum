using UnityEngine;
using System.Collections;

public class RunFromCharAndAttackRandTileAI : EnemyAI {

	RunFromCharAI _RunFromCharAI;
	AttackRandomTileAI _AttackRandomTileAI;

	public RunFromCharAndAttackRandTileAI () {
		_RunFromCharAI = new RunFromCharAI();
		_AttackRandomTileAI = new AttackRandomTileAI();
	}

	public override bool PrepareAction (Enemy e) {
		if (!_AttackRandomTileAI.PrepareAction(e))
			_RunFromCharAI.PrepareAction(e);

		return true;
	}

}
