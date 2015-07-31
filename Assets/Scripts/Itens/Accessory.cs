using UnityEngine;
using System.Collections;

[System.Serializable]
public class Accessory {

	[HideInInspector]
	public GameObject MyGameObject;

	#pragma warning disable 0649
	[SerializeField]
	private int _UseRange, _Delay;
	private int _DelayCounter;

	public int UseRange { get { return _UseRange; } }

	[SerializeField]
	private bool _Permanent;
	[SerializeField]
	private int _Durability;

	// Modifiers
	public enum ModifierType {Strength, Range, EnemyVision}
	[SerializeField]
	private int _StrengthModifier, _RangeModifier, _EnemyVisionModifier;

	// Counters
	public enum CounterType {BeAttacked, BeSaw, Attacked}
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
			CounterAction(_BeAttackedEffect, origin, target);
			break;
		case CounterType.BeSaw:
			CounterAction(_BeSawEffect, origin, target);
			break;
		case CounterType.Attacked:
			CounterAction(_AttackedEffect, origin, target);
			break;
		}
	}

	private void CounterAction (GameEffect effect, Interactive origin, Interactive target) {
		if (effect!=null) {
			if (MadeAction())
				effect.MakeEffect(origin,target);
		}
	}
	#endregion

	#region Actives
	public void Activate (Interactive origin, Tile target) {
		if (MapController.Instance.GetDistance(origin.MyTile,target) < UseRange) {
			if (_ActivateEffect!=null && target!=null)
				if (MadeAction())
					_ActivateEffect.MakeEffect(origin, target);
		}
	}
	#endregion

	public void BeDestroyed () {
		MyGameObject.GetComponent<Interactive>().BeDestroyed();
	}

	public void IncreaseModifierValue (ModifierType type) {
		switch (type) {
		case ModifierType.Strength:
			_StrengthModifier++;
			break;
		case ModifierType.Range:
			_RangeModifier++;
			break;
		case ModifierType.EnemyVision:
			_EnemyVisionModifier++;
			break;
		}
	}
}
