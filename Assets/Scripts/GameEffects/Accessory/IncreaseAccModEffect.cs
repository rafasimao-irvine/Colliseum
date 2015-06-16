using UnityEngine;
using System.Collections;

public class IncreaseAccModEffect : GameEffect {

	[SerializeField]
	private AccessoryEffect _AccessoryEffect;

	[SerializeField]
	private Accessory.ModifierType _ModType;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Characther)
			_AccessoryEffect.GetAccessory().IncreaseModifierValue(_ModType);
	}
}
