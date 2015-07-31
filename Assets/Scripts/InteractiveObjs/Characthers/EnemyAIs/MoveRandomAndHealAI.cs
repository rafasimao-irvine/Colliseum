using UnityEngine;
using System.Collections;

public class MoveRandomAndHealAI : EnemyAI {

	FindToHealAI _FindToHealAI;
	MoveRandomAI _MoveRandomAI;

	public MoveRandomAndHealAI () {
		_FindToHealAI = new FindToHealAI();
		_MoveRandomAI = new MoveRandomAI();
	}

	public override bool PrepareAction (Enemy e) {
		if (!_FindToHealAI.PrepareAction(e))
			_MoveRandomAI.PrepareAction(e);

		return true;
	}

}
