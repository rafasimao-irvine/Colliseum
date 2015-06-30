using UnityEngine;
using System.Collections;

public class LevelReady : MonoBehaviour {

	public void Ready () {
		if (PreparedLevelController.Instance != null)
			PreparedLevelController.Instance.SaveLevelAndPlay();
	}
}
