using UnityEngine;
using System.Collections;

[System.Serializable]
public class Accessory {

	[HideInInspector]
	public GameObject MyGameObject;

	#pragma warning disable 0649
	[SerializeField]
	private int _Delay;
	private int _DelayCounter;

	[SerializeField]
	private bool _Permanent;
	[SerializeField]
	private int _Durability;

	// Modifiers
	public enum ModifierType {
		Strength, Range, EnemyVision
	}
	[SerializeField]
	private int _StrengthModifier, _RangeModifier, _EnemyVisionModifier;

	// Counters
	public enum CounterType {
		BeAttacked, BeSaw, Attacked
	}
	[SerializeField]
	private GameEffect _BeAttackedEffect, _BeSawEffect, _AttackedEffect;

	// Actives
	[SerializeField]
	private GameEffect _ActivateEffect;
	#pragma warning restore 0649

	public void UpdateAccessory () {
		if (_DelayCounter <= _Delay)
			_DelayCounter++;
	}

	protected bool MadeAction () {
		if (IsReady() && !IsBroken()) {
			//reboot cooldown
			_DelayCounter=0;
			// Decrease Durability
			if (!_Permanent) _Durability--;

			return true;
		}
		return false;
	}

	public bool IsBroken () {
		return (!_Permanent && _Durability<1);
	}

	public bool IsReady () {
		return (_DelayCounter > _Delay);
	}

	public bool IsActivatable () {
		return (_ActivateEffect != null);
	}

	#region Modifiers
	public int GetModifier (ModifierType type) {
		int result = 0;
		switch (type) {
		case ModifierType.Strength:
			result = _StrengthModifier;
			break;
		case ModifierType.Range:
			result = _RangeModifier;
			break;
		case ModifierType.EnemyVision:
			result = _EnemyVisionModifier;
			break;
		}

		if (result!=0) 
			if(!MadeAction()) 
				result = 0;

		return result;
	}
	#endregion

	#region Counters
	public void MakeCounterAction (Interactive origin, Interactive target, CounterType type) {
		switch (type) {
		case CounterType.BeAttacked:
			BeAttacked(origin,target);
			break;
		case CounterType.BeSaw:
			BeSaw(origin,target);
			break;
		case CounterType.Attacked:
			Attacked(origin,target);
			break;
		}
	}

	public void BeAttacked (Interactive origin, Interactive target) {
		if (_BeAttackedEffect!=null) {
			if (MadeAction())
				_BeAttackedEffect.MakeEffect(origin,target);
		}
	}

	public void BeSaw (Interactive origin, Interactive target) {
		if (_BeSawEffect!=null) {
			if (MadeAction())
				_BeSawEffect.MakeEffect(origin,target);
		}
	}

	public void Attacked (Interactive origin, Interactive target) {
		if (_AttackedEffect!=null) {
			if (MadeAction())
				_AttackedEffect.MakeEffect(origin,target);
		}
	}
	#endregion

	#region Actives
	public void Activate (Interactive origin, Tile target) {
		if (_ActivateEffect!=null && target.OnTop!=null) {
			if (MadeAction())
				_ActivateEffect.MakeEffect(origin, target.OnTop);
		}
	}
	#endregion

	public void BeDestroyed () {
		MyGameObject.GetComponent<Interactive>().BeDestroyed();
	}

}
