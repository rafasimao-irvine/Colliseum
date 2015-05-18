using UnityEngine;
using System.Collections;

public class TriggerEffect : GameEffect {

	[HideInInspector]
	public IReceiver Receiver;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if(Receiver != null)
			Receiver.BeTriggered(origin,target);
	}

}
