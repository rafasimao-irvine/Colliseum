using UnityEngine;
using System.Collections;

public class ObjectHead : Interactive {

	enum ObjectType {
		Attackable, Enterable, Interactable, Obstacle
	};

	#pragma warning disable 0649
	[SerializeField]
	private ObjectType _Type;
	
	[SerializeField]
	private GameEffect _BeAttackedEffect, _BeEnteredEffect, _BeLeftEffect;

	[SerializeField]
	private bool _DestroyUponBeAttacked, _DestroyUponBeEntered, _DestroyUponBeLeft;
	private bool _DestroySelf;
	#pragma warning restore 0649
	private Characther _CharInto;

	void Awake () {
		SetType(_Type);
	}

	void Update () {
		if (_DestroySelf)
			BeDestroyed();
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		bool isDestroyed = _DestroySelf;
		if (_DestroyUponBeAttacked) _DestroySelf=true;
	
		if (_BeAttackedEffect!=null && !isDestroyed)
			_BeAttackedEffect.MakeEffect(this, iObj);
		if (_CharInto!=null)
			_CharInto.BeAttacked(iObj,damage);
	}

	public override bool BeEntered (Characther c) {
		bool isDestroyed = _DestroySelf;
		if (_DestroyUponBeEntered) _DestroySelf=true;

		if (_CharInto==null) {
			if (_BeEnteredEffect!=null && !isDestroyed)
				_BeEnteredEffect.MakeEffect(this, c);

			_CharInto = c;
			SetType(ObjectType.Attackable);

			return true;
		}
		return false;
	}

	public override bool BeLeft (Characther c) {
		bool isDestroyed = _DestroySelf;
		if (_DestroyUponBeLeft) _DestroySelf=true;

		if (_CharInto==c) {
			if (_BeLeftEffect!=null && !isDestroyed)
				_BeLeftEffect.MakeEffect(this, c);

			_CharInto = null;
			SetType(ObjectType.Enterable);
		}
		return true;
	}

	public override Interactive GetBeAttackedTarget () {
		// If it is enterable returns what is inside
		if (_CharInto!=null || !Blockable)
			return _CharInto;

		return base.GetBeAttackedTarget(); // will return self
	}

	// Destroy the object and set the characther inside it to be at its tile
	public override void BeDestroyed () {
		MyTile.TryGetOut(this);
		if (_CharInto!=null)
			MyTile.TryGetIn(_CharInto);

		Destroy(gameObject);
	}

	// Set the object type, based in the possible types: Attackable, Enterable, Interactable, Obstacle
	void SetType (ObjectType type) {
		_Type = type;
		Attackable = Blockable = Interactable = false;
		switch (type){
		case ObjectType.Attackable:
			Attackable = Blockable = true;
			break;
		case ObjectType.Enterable:
			break;
		case ObjectType.Interactable:
			Blockable = Interactable = true;
			break;
		case ObjectType.Obstacle:
			Blockable = true;
			break;
		}
	}

}
