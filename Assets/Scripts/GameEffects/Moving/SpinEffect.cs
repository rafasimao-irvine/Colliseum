using UnityEngine;
using System.Collections;

public class SpinEffect : GameEffect {

	private bool _StartSpinning;

	[SerializeField]
	private int _SpinSpeed;

	[SerializeField]
	private int _SpinAngle;
	private Vector3 _SpinVector;

	void Start () {
		_SpinVector = new Vector3(0,0,0);
	}

	void Update () {
		Vector3 rotate = transform.eulerAngles;
		rotate = Vector3.Lerp(rotate, _SpinVector, Time.deltaTime * _SpinSpeed);
		transform.eulerAngles = rotate;
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		_SpinVector = new Vector3(0,_SpinAngle+transform.eulerAngles.y,0);
		if (_SpinVector.y>=360)
			_SpinVector.y = 0;
	}

}
