using UnityEngine;
using System.Collections;

public class CharactherWeapons {

	protected Weapon _FirstWeapon, _SecondWeapon;
	public WeaponsHUD CharWeaponsHUD;

	public CharactherWeapons (WeaponsHUD weaponsHUD) {
		CharWeaponsHUD = weaponsHUD;
	}

	public enum WeaponHand {
		FirstHand, SecondHand
	}

	private void UpdateWeaponHUD () {
		if (CharWeaponsHUD!=null)
			CharWeaponsHUD.UpdateWeapons();
	}

	public void UpdateWeapons () {
		if (_FirstWeapon!=null)
			_FirstWeapon.UpdateWeapon();
		if (_SecondWeapon!=null)
			_SecondWeapon.UpdateWeapon();
	}

	public void Attack (Characther user, Interactive target) {
		if (IsAnyFirstWeaponEquipped())
			_FirstWeapon.Attack(user,target);
	
		VerifyIfIsBroken();
	}

	public void Attack (Characther user, Tile targetTile) {
		if (IsAnyFirstWeaponEquipped())
			_FirstWeapon.Attack(user,targetTile);
		
		VerifyIfIsBroken();
	}

	private void VerifyIfIsBroken () {
		if (_FirstWeapon.IsBroken()) {
			_FirstWeapon.BeDestroyed();
			_FirstWeapon = null;
		}
		UpdateWeaponHUD();
	}

	public Weapon EquipWeapon (Weapon weapon, WeaponHand hand) {
		Weapon result = weapon;
		if (hand == WeaponHand.FirstHand) {
			result = _FirstWeapon;
			_FirstWeapon = weapon;
			Logger.strLog += "First weapon equipped";
		}
		else if (hand == WeaponHand.SecondHand) {
			result = _SecondWeapon;
			_SecondWeapon = weapon;
		}

		UpdateWeaponHUD();
		return result;
	}

	public void SwitchWeapons () {
		Weapon w = _FirstWeapon;
		_FirstWeapon = _SecondWeapon;
		_SecondWeapon = w;
		Logger.strLog += "weapons switched";

		UpdateWeaponHUD();
	}

	public bool IsAnyFirstWeaponEquipped () {
		return (_FirstWeapon != null);
	}

	public int GetFirstWeaponAttackRange () {
		if (IsAnyFirstWeaponEquipped())
			return _FirstWeapon.GetWeaponAttackRange();
		return 0;
	}

	public bool IsFirstWeaponReady () {
		if (IsAnyFirstWeaponEquipped())
			return _FirstWeapon.IsReady();
		return false;
	}

	public Weapon GetFirstWeapon () {
		return _FirstWeapon;
	}

	public Weapon GetSecondWeapon () {
		return _SecondWeapon;
	}

}
