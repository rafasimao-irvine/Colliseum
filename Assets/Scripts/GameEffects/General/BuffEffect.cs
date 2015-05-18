using UnityEngine;
using System.Collections;

public class BuffEffect : GameEffect {

	[SerializeField]
	private int _StrBuff;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			((Characther)target).BeBuffered(_StrBuff);
	}

}
