using UnityEngine;
using System.Collections;

public class HealEffect : GameEffect {

	[SerializeField]
	private int HealingFactor;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			((Characther)target).Heal(HealingFactor);
	}
}
