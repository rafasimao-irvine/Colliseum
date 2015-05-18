using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoHUD : MonoBehaviour {

	public GameObject InfoPanel;
	public Text InfoText;

	public bool IsShowing {get; protected set;}

	public void ShowInfo (Interactive iObj) {
		InfoPanel.SetActive(true);
		InfoText.text = iObj.name.Substring(0,iObj.name.Length-7);
		IsShowing = true;
	}

	public void HideInfo () {
		InfoPanel.SetActive(false);
		IsShowing = false;
	}

}
