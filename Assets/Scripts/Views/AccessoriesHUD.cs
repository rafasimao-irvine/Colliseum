using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccessoriesHUD : MonoBehaviour {

	public PlayerController AccessoriesPlayerController;
	private Characther AccessoriesChar;

	public SightView PersonageSightView;

	public Button[] BtAccessory;
	public Text[] TextAccessory;

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
		for (int i=0; i<BtAccessory.Length; i++)
			UpdateAccessoryInfo(BtAccessory[i], TextAccessory[i], AccessoriesChar.GetAccessory(i));
	}
	
	private void UpdateAccessoryInfo (Button bt, Text t, Accessory a) {
		if (a == null) {
			bt.interactable = false;
			t.text = "Empty";
		} else {
			bt.interactable = true;
			t.text = a.MyGameObject.name.Substring(0,a.MyGameObject.name.Length-7);
		}
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
	} 
	#endregion

}
