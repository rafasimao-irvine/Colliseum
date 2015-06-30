using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Image ShowImage;

	private bool _ShowingImage;

	void Update () {
		if (_ShowingImage) {
			if (Input.GetMouseButtonDown(0) || 
			    ((Input.touches.Length == 1) && Input.GetTouch(0).phase==TouchPhase.Began) )
				HideShowImage();
		}
	}

	public void Play () {
		//Application.LoadLevel("Battle");
		Application.LoadLevel("PrepareLevel");
	}

	public void PlaceShowImage (Sprite image) {
		ShowImage.gameObject.SetActive(true);
		_ShowingImage = true;
		ShowImage.sprite = image;
	}

	public void HideShowImage () {
		ShowImage.gameObject.SetActive(false);
		_ShowingImage = false;
	}

	public void Quit () {
		Application.Quit();
	}
}
