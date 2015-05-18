using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelHUD : MonoBehaviour {

	public Text LevelText;

	void Start () {
		if (LevelController.Instance!=null)
			LevelText.text = "Level: "+LevelController.Instance.GetLevel();
		else
			LevelText.text = "";
	}
	
}
