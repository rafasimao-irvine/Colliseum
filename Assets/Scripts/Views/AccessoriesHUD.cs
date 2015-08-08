using UnityEngine;
using System.Collections;

public class AccessoriesHUD : MonoBehaviour {

	public PlayerController AccessoriesPlayerController;
	private Characther AccessoriesChar;

	public SightView PersonageSightView;

	public AccessoryHUD[] AccessoriesHUDs; 
	private int _LastAccessorySelectedIndex;

	void Start () {
		AccessoriesChar = AccessoriesPlayerController.PlayerPersonage;
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

	public void UpdateAccessories () {
		bool isOnDelay = (AccessoriesChar.GetAccessoriesDelay() > -1);
		for (int i=0; i<AccessoriesHUDs.Length; i++)
			AccessoriesHUDs[i].UpdateAccessoryInfo(AccessoriesChar.GetAccessory(i), isOnDelay);

		if (isOnDelay)
			AccessoriesHUDs[_LastAccessorySelectedIndex].ActivateAccessoryDelayRocks(
				AccessoriesChar.GetAccessoriesDelay());
	}

	#region Accessories Buttons
	public void RemoveAccessory (int index) {
		AccessoriesChar.RemoveAccessory(index);
		UpdateAccessories();
	}

	public void AccessoryAction (int index) {
		AccessoriesPlayerController.AccessoryAction(index);

		PersonageSightView.ShowSingleSight(
			AccessoriesChar.MyTile, AccessoriesChar.GetAccessory(index).UseRange);

		_LastAccessorySelectedIndex = index;
	} 
	#endregion

}
