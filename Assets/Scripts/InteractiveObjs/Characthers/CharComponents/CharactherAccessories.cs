using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactherAccessories {

	private Accessory[] _Accessories;
	private int _MaxAccessories;
	public AccessoriesHUD CharAccessoriesHUD;

	public CharactherAccessories (int initialMax, int maxMax, AccessoriesHUD accessoriesHUD) {
		_MaxAccessories = initialMax;
		_Accessories = new Accessory[maxMax];

		CharAccessoriesHUD = accessoriesHUD;
	}

	private void UpdateAccessoryHUD () {
		if (CharAccessoriesHUD!=null)
			CharAccessoriesHUD.UpdateAccessories();
	}

	public void UpdateAccessories () {
		for (int i=0; i<_MaxAccessories; i++) {
			if (_Accessories[i]!=null) {
				_Accessories[i].UpdateAccessory();
				if (_Accessories[i].IsBroken()) {
					_Accessories[i].BeDestroyed();
					_Accessories[i] = null;
				}
			}
		}

		UpdateAccessoryHUD();
	}

	public Accessory EquipAccessory (Accessory accessory, int index) {
		if (!IsValidIndex(index))
			return null;

		Accessory result = null;
		if (_Accessories[index]!=null)
			result = _Accessories[index];
		_Accessories[index] = accessory;

		UpdateAccessoryHUD();

		return result;
	}

	public void SwitchAccessories (int pos1, int pos2) {
		if (!IsValidIndex(pos1) || !IsValidIndex(pos2)) return;

		Accessory a = _Accessories[pos1];
		_Accessories[pos1] = _Accessories[pos2];
		_Accessories[pos2] = a;

		UpdateAccessoryHUD();
	}

	public void RemoveAccessory (int pos) {
		if (_Accessories[pos] != null) {
			_Accessories[pos].BeDestroyed();
			_Accessories[pos] = null;
		}
	}

	public int GetFreeSpot () {
		for (int i=0; i<_MaxAccessories; i++) {
			if (_Accessories[i]==null)
				return i;
		}
		return 0;
	}

	public Accessory GetAccessory (int index) {
		if (!IsValidIndex(index))
			return null;
		return _Accessories[index];
	}

	public int GetMaxAccessories () {
		return _MaxAccessories;
	}

	private bool IsValidIndex (int index) {
		return (index < _MaxAccessories && index >= 0);
	}

	#region Modifiers
	public int GetStrengthModifier () {
		return GetModifier(Accessory.ModifierType.Strength);
	}

	public int GetRangeModifier () {
		return GetModifier(Accessory.ModifierType.Range);
	}

	public int GetEnemyVisionModifier () {
		return GetModifier(Accessory.ModifierType.EnemyVision);
	}

	private int GetModifier (Accessory.ModifierType type) {
		int result = 0;
		for (int i=0; i<_MaxAccessories; i++) {
			if (_Accessories[i]!=null)
				result += _Accessories[i].GetModifier(type);
		}
		return result;
	}
	#endregion

	#region Counters
	public void OnBeAttacked (Characther c, Interactive target) {
		MakeCounterAction(c,target,Accessory.CounterType.BeAttacked);
	}

	public void OnAttack (Characther c, Interactive target) {
		MakeCounterAction(c,target,Accessory.CounterType.Attacked);
	}

	public void OnBeSaw (Characther c, Interactive target) {
		MakeCounterAction(c,target,Accessory.CounterType.BeSaw);
	}

	private void MakeCounterAction (Characther c, Interactive target, Accessory.CounterType type) {
		for (int i=0; i<_MaxAccessories; i++) {
			if (_Accessories[i]!=null)
				_Accessories[i].MakeCounterAction(c,target,type);
		}
	}
	#endregion

	#region Actives
	public void OnButtonPressed (int index, Characther c, Tile onTile) {
		if (_Accessories[index]!=null)
			_Accessories[index].Activate(c,onTile);
	}
	#endregion

}
