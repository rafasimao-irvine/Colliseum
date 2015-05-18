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
	private List<GameEffect> _TurnChangeEffects;

	void Start () {
		ObjectsController objControll = FindObjectOfType<ObjectsController>();
		if (objControll!=null) objControll.Subscribe(this);
	}

	public void OnTurnChange () {
		_CurrentTurnMod++;
		if (_CurrentTurnMod >= _TurnRate) {
			_CurrentTurnMod = 0;
			for (int i=0; i<_TurnChangeEffects.Count; i++)
				_TurnChangeEffects[i].MakeEffect(_InteractiveObject, null);
		}
	}

	void OnDestroy () {
		ObjectsController objControll = FindObjectOfType<ObjectsController>();
		if (objControll!=null) objControll.Unsubscribe(this);
	}
}
