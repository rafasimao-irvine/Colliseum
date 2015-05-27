using UnityEngine;
using System.Collections;

/**
 * Paralizes the target(Characther), with a certain amount of Turns.
 * If it is destroyed the player is unparalized.
 * */
public class ParalizeEffect : GameEffect {

	[SerializeField]
	private int _NTurns = 99;
	private Characther _CharInto;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther) {
			_CharInto = ((Characther)target);
			_CharInto.BeParallized(_NTurns);
		}
	}

	void OnDestroy () {
		if(_CharInto!=null)
			_CharInto.BeUnparallized();
	}

}
