using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour {

	[SerializeField]
	private GameEffect _CollisionEffect;
	[SerializeField]
	private int _MaxDistance;

	private float _Offset = 1.5f;
	private float _Force = 6f;

	private Interactive _Origin;
	private Rigidbody _Rigidbody;
	private Collider _Collider;

	void Awake () {
		_Rigidbody = GetComponent<Rigidbody>();
		_Collider = GetComponent<Collider>();
		_Collider.enabled = false;
	}

	void Update () {
		if (_Collider.enabled) {
			if ((transform.position - _Origin.transform.position).magnitude > _MaxDistance)
				gameObject.SetActive(false);
		}
	}

	public void BeThrown (Interactive origin, Interactive target) {
		_Origin = origin;
		gameObject.SetActive(true);

		Vector3 direction = target.transform.position - origin.transform.position;
		direction.y = 0f;

		Vector3 pos = origin.transform.position;
		pos.y= transform.position.y;
		pos += direction.normalized * _Offset;
		transform.position = pos;

		_Rigidbody.velocity = direction.normalized * _Force;
		transform.rotation = Quaternion.LookRotation(direction);

		_Collider.enabled = true;
	}

	void OnCollisionEnter (Collision other) {
		Characther c = other.gameObject.GetComponent<Characther>();
		if (c!=null)
			_CollisionEffect.MakeEffect(_Origin,c);

		gameObject.SetActive(false);
	}

}
