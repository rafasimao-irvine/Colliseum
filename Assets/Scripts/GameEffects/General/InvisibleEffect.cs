using UnityEngine;
using System.Collections;

public class InvisibleEffect : GameEffect {

	[SerializeField]
	private int _NTurns;
	
	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther && !((Characther)target).IsDead())
			((Characther)target).BeInvisible(_NTurns);
	}

}
