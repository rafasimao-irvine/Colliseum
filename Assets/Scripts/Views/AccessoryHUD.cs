using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccessoryHUD : MonoBehaviour {

	public Button BtAccessory;
	public Text TextAccessory;
	public DelayRocksHUD DelayRocksHUDAccessory;

	public void UpdateAccessoryInfo (Accessory a, bool isOnDelay) {

		bool isInteractable = false;
		if (a == null) {
			TextAccessory.text = "Empty";
		} else {
			if (!isOnDelay)
				isInteractable = true;
			TextAccessory.text = a.MyGameObject.name.Substring(0,a.MyGameObject.name.Length-7);
			DelayRocksHUDAccessory.FixRocksSize(a.Delay);
		}
		BtAccessory.interactable = isInteractable;
	}

	public void ActivateAccessoryDelayRocks (int n) {
		DelayRocksHUDAccessory.ActivateRocks(n);
	}

	public void ResetAccessoryDelayRocks () {
		DelayRocksHUDAccessory.ResetRocks();
	}

}
