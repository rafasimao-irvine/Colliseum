using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectReceiver : MonoBehaviour, IReceiver {

	[SerializeField]
	private Interactive _InteractiveObject;
	[SerializeField]
	private ObjectCreator _ObjectCreator;

	[SerializeField]
	private GameEffect _TriggerEffect;

	[SerializeField]
	private bool _Permanent;
	[SerializeField]
	private int _Durability;
	[SerializeField]
	private bool _DestroyUponBeTriggered;
	private bool _DestroySelf;

	void Start () {
		List<Interactive> objs =  _ObjectCreator.GetObjects();

		for (int i=0; i<objs.Count; i++) {
			TriggerEffect trigger = objs[i].GetComponent<TriggerEffect>();
			if (trigger!=null)
				trigger.Receiver = this;
		}
	}

	void Update () {
		if (_DestroySelf)
			BeDestroyed();
	}

	public void BeTriggered (Interactive origin, Interactive actor) {
		if (_DestroyUponBeTriggered) _DestroySelf = true;

		if (_TriggerEffect!=null && IsValid())
			_TriggerEffect.MakeEffect(_InteractiveObject, actor);

		DecreaseDurability();
	}

	private void DecreaseDurability () {
		if (!_Permanent && _Durability>0)
			_Durability--;
	}

	public bool IsValid () {
		return (_Permanent || _Durability>0);
	}

	// Destroy the object
	public void BeDestroyed () {
		_InteractiveObject.MyTile.TryGetOut(_InteractiveObject);
		Destroy(gameObject);
	}
}
