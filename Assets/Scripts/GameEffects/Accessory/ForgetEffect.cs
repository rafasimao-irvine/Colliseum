using UnityEngine;
using System.Collections;

public class ForgetEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Enemy)
			((Enemy)target).ForgetTargetCharacther();
	}

}
