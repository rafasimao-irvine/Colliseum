using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlarmEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target.GetBeAttackedTarget() is Enemy)
			((Enemy)target.GetBeAttackedTarget()).RevealTargetCharacther();
	}

}
