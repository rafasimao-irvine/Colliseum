using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlarmEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		target = target.GetBeAttackedTarget();
		if (target is Enemy) {
			if (origin is Enemy)
				((Enemy)target).RevealTargetCharacther(((Enemy)origin).TargetChar);
			else if (origin is Characther)
				((Enemy)target).RevealTargetCharacther((Characther)origin);
		}
	}

}
