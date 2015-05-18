using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccessoriesHUD : MonoBehaviour {

	public Characther AccessoriesChar;
	
	public Button[] BtAccessory;
	public Text[] TextAccessory;
	
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

	public void RemoveAccessory (int index) {
		AccessoriesChar.RemoveAccessory(index);
		UpdateAccessories();
	}

}
