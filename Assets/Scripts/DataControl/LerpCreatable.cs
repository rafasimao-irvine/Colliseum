using UnityEngine;
using System.Collections;

[System.Serializable]
public class LerpCreatable : Creatable {
	[SerializeField]
	protected int InitialMin, EndMin, InitialMax, EndMax;

	public void LerpBy (float t) {
		Min = (int)Mathf.Lerp(InitialMin, EndMin, t);
		Max = (int)Mathf.Lerp(InitialMax, EndMax, t);
	}
}
