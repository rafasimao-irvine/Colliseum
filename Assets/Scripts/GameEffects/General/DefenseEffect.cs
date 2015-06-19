using UnityEngine;
using System.Collections;

public class DefenseEffect : GameEffect {

	[SerializeField]
	private int _NTurns;
	
	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			((Characther)target).BeDefensive(_NTurns);
	}

}
