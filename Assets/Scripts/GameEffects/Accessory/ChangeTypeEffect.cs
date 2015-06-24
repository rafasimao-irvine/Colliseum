using UnityEngine;
using System.Collections;

public class ChangeTypeEffect : GameEffect {

	[SerializeField]
	private Characther.Types _NewType;

	protected override void DoEffect (Interactive origin, Interactive target) {
		target = target.GetBeAttackedTarget();
		if (target is Characther)
			CharacthersHolder.Instance.ChangeCharType((Characther)target, _NewType);
	}

}
