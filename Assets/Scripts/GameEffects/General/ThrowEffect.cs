using UnityEngine;
using System.Collections;

public class ThrowEffect : GameEffect {

	[SerializeField]
	private GameObject _ThrowablePrefab;
	private Throwable _Throwable;

	void OnAwake () {
		CreateThrowable();
	}

	private void CreateThrowable () {
		_Throwable = GeneralFabric.CreateObject<Throwable>(_ThrowablePrefab, null);
		_Throwable.gameObject.SetActive(false);
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (_Throwable==null)
			CreateThrowable();

		_Throwable.BeThrown(origin,target);
	}

}
