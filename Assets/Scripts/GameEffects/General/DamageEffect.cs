using UnityEngine;
using System.Collections;

/**
 * Attacks a target, with a certain amount of Damage, 
 * passing the origin as the causer.
 * */
public class DamageEffect : GameEffect {

	[SerializeField]
	private int _Damage;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (!(target is Characther) || (target is Characther && !((Characther)target).IsDead()))
			target.BeAttacked(origin, _Damage);
	}
	
}
