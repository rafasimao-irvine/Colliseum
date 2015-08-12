using UnityEngine;
using System.Collections;

/**
 * Attacks a target(Characther), with a certain amount of Damage
 * that certainly will kill the target, passing the origin as the causer.
 * */
public class KillEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {

		if (target is Characther)
			((Characther)target).BeKilled(origin);
	}

}
