using UnityEngine;
using System.Collections;

public class KillEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {

		if (target is Characther)
			target.BeAttacked(origin, ((Characther)target).GetLife());
	}

}
