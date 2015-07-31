using UnityEngine;
using System.Collections;

public class MoveRandomAndAttackRandomTile : EnemyAI {

	AttackRandomTileAI _AttackRandomTileAI;
	MoveRandomAI _MoveRandomAI;

	public MoveRandomAndAttackRandomTile () {
		_AttackRandomTileAI = new AttackRandomTileAI();
		_MoveRandomAI = new MoveRandomAI();
	}

	public override bool PrepareAction (Enemy e) {
		if (!_AttackRandomTileAI.PrepareAction(e))
			_MoveRandomAI.PrepareAction(e);

		return true;
	}

}
