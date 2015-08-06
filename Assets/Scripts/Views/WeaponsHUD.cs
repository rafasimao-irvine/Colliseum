using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponsHUD : MonoBehaviour {

	public PlayerController WeaponsPlayerController;
	private Characther WeaponsChar;

	public SightView PersonageSightView;

	public Text FirstWeapon, SecondWeapon;

	void Start () {
		WeaponsChar = WeaponsPlayerController.PlayerPersonage;
	}

	void Update () {
		#if UNITY_EDITOR
		VerifyMouseInput();
		#elif UNITY_ANDROID || UNITY_IOS
		VerifyTouchInput();
		#endif
	}

	#region Verify Touch
	void VerifyMouseInput() {
		if (Input.GetMouseButton(0))
			PersonageSightView.ShutOffLastSight(); // Shut off SightView
	}
	
	void VerifyTouchInput () {
		if (Input.touches.Length == 1)
			PersonageSightView.ShutOffLastSight(); // Shut off SightView
	}
	#endregion

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

	public void SelectWeapon (bool isSecondWeapon) {
		if (isSecondWeapon)
			WeaponsPlayerController.SwitchWeapons();

		PersonageSightView.ShowSingleSight(WeaponsChar.MyTile,WeaponsChar.GetCurrentAttackRange());
	}

}
