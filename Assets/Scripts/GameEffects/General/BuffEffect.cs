using UnityEngine;
using System.Collections;

/**
 * Buffs the target(Characther), with a certain amount of StrBuff;
 * */
public class BuffEffect : GameEffect {

	[SerializeField]
	private int _StrBuff;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			((Characther)target).BeBuffered(_StrBuff);
	}

}
