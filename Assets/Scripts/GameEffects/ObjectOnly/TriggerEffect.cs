using UnityEngine;
using System.Collections;

/**
 * Sends a message to a setted receiver to be triggered passing origin and target.
 * */
public class TriggerEffect : GameEffect {

	[HideInInspector]
	public IReceiver Receiver;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if(Receiver != null)
			Receiver.BeTriggered(origin,target);
	}

}
