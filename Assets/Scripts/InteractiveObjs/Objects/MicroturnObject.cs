using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MicroturnObject : MonoBehaviour {

	[SerializeField]
	private Interactive _InteractiveObject;

	[SerializeField]
	private int _TurnRate;
	private int _CurrentTurnMod;

	[SerializeField]
	private bool _DestroyUponBeTriggered;
	private bool _DestroySelf;

	[SerializeField]
	private List<GameEffect> _TurnChangeEffects;

	void Start () {
		ObjectsController objControll = FindObjectOfType<ObjectsController>();
		if (objControll!=null) objControll.Subscribe(this);
	}

	void Update () {
		if (_DestroySelf)
			BeDestroyed();
	}

	public void OnTurnChange () {
		_CurrentTurnMod++;
		if (_CurrentTurnMod >= _TurnRate) {
			// Self Destruction
			if (_DestroyUponBeTriggered) _DestroySelf = true;

			_CurrentTurnMod = 0;
			for (int i=0; i<_TurnChangeEffects.Count; i++)
				_TurnChangeEffects[i].MakeEffect(_InteractiveObject, _InteractiveObject);
		}
	}

	// Destroy the object and set the characther inside it to be at its tile
	public void BeDestroyed () {
		_InteractiveObject.MyTile.TryGetOut(_InteractiveObject);
		Destroy(gameObject);
	}

	void OnDestroy () {
		ObjectsController objControll = FindObjectOfType<ObjectsController>();
		if (objControll!=null) objControll.Unsubscribe(this);
	}
}
