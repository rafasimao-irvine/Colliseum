using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinLoseMenu : MonoBehaviour {

	public GameObject WinLoseScreen;
	public Text WinLoseText, WinLoseButtonText;

	private bool _Passed;

	void Start () {
		if (LevelController.Instance!=null)
			LevelController.Instance.WinLoseScreen = this;
	}

	public void Activate (bool passed) {
		WinLoseScreen.SetActive(true);
		_Passed = passed;

		if (passed) {
			SetTextStr(WinLoseText, "Passou de fase!");
			SetTextStr(WinLoseButtonText, "próxima");
		} else {
			SetTextStr(WinLoseText, "Você perdeu!");
			SetTextStr(WinLoseButtonText, "recomeçar!");
		}
	}

	private void SetTextStr (Text text, string str) {
		text.text = str;
	}

	public void WinLoseButonPressed () {
		if (LevelController.Instance != null) {
			if (_Passed)
				LevelController.Instance.NextLevel();
			else
				LevelController.Instance.RestartLevel();
		}
		else
			Application.LoadLevel("Battle");
	}
}
