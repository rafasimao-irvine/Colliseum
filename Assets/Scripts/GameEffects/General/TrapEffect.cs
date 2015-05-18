using UnityEngine;
using System.Collections;

public class TrapEffect : GameEffect {

	[SerializeField]
	private int _NTurns;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if(target is Characther)
			((Characther)target).BeTrapped(_NTurns);
	}

}
