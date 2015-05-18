using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponsHUD : MonoBehaviour {

	public Characther WeaponsChar;

	public Text FirstWeapon, SecondWeapon;

	public void UpdateWeapons () {
		UpdateWeaponInfo(FirstWeapon, WeaponsChar.GetFirstWeapon());
		UpdateWeaponInfo(SecondWeapon, WeaponsChar.GetSecondWeapon());
	}

	private void UpdateWeaponInfo (Text text, Weapon weapon) {
		if (weapon!=null)
			text.text = weapon.MyGameObject.name.Substring(0,weapon.MyGameObject.name.Length-7);
		else
			text.text = "Hand";
	}

}
