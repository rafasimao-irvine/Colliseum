using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharHUD : MonoBehaviour {

	[SerializeField]
	private Characther _Characther;
	public Slider HealthBar;

	public Text SurpriseText;

	public GameObject Buffered,Trapped,Paralized;

	private Quaternion _Rotation;

	void Awake () {
		HealthBar.maxValue = _Characther.GetMaxLife();
		_Rotation = transform.rotation;
	}

	void Update () {
		transform.rotation = _Rotation;
		UpdateStatus();
		HealthBar.maxValue = _Characther.GetMaxLife();
		HealthBar.value = _Characther.GetLife();
	}

	private void UpdateStatus () {
		Buffered.SetActive(_Characther.IsBuffered());
		Trapped.SetActive(_Characther.IsTrapped());
		Paralized.SetActive(_Characther.IsParalized());
	}


	public void ShowSurprised () {
		SurpriseText.gameObject.SetActive(true);
		Invoke("DeactivateSurprised", 1.2f);
	}

	public void DeactivateSurprised() {
		SurpriseText.gameObject.SetActive(false);
	}
}
