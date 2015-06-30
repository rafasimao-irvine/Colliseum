using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveStats : MonoBehaviour {

	public Toggle ToggleBt;

	void Start () {
		if (PreparedLevelController.Instance != null)
			ToggleBt.isOn = PreparedLevelController.Instance.SaveStats;
	}

	public void ToggleSaveStats () {
		if (PreparedLevelController.Instance != null) 
			PreparedLevelController.Instance.SaveStats = ToggleBt.isOn;
	}
}
