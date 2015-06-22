using UnityEngine;
using System.Collections;

public class AbsorbEffect : GameEffect {

	[SerializeField]
	protected int _NTurns;
	
	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			((Characther)target).BeAbsorptive(_NTurns);
	}

}
