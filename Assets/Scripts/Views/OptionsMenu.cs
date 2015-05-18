using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public GameObject OptMenu;
	private bool _IsOptActive = false;

	public void ToggleOptionsMenu () {
		_IsOptActive = !_IsOptActive;
		OptMenu.SetActive(_IsOptActive);
		Time.timeScale = (_IsOptActive) ? 0f : 1f;
	}

	public void Quit () {
		Time.timeScale = 1f;
		if (LevelController.Instance!=null)
			Destroy(LevelController.Instance.gameObject);
		Application.LoadLevel("Menu");
	}

	public void Restart () {
		Time.timeScale = 1f;
		if (LevelController.Instance != null)
			LevelController.Instance.RestartLevel();
		else
			Application.LoadLevel("Battle");
	}
}
