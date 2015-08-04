using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour {

	[SerializeField]
	private GameEffect _CollisionEffect;
	[SerializeField]
	private int _MaxDistance;

	private float _Offset = 2f;
	private float _Force = 6f;

	private Interactive _Origin;
	private Rigidbody _Rigidbody;

	public bool IsOn {get; private set;}

	void Awake () {
		_Rigidbody = GetComponent<Rigidbody>();
		TurnOn(false);
	}

	void Update () {
		if (IsOn) {
			if ((transform.position - _Origin.transform.position).magnitude > _MaxDistance)
				TurnOn(false);
		}
	}

	public void BeThrown (Interactive origin, Tile targetTile) {
		_Origin = origin;
		TurnOn();
		
		Vector3 direction = targetTile.transform.position - origin.transform.position;
		direction.y = 0f;
		
		Vector3 pos = origin.transform.position;
		pos.y= transform.position.y;
		pos += direction.normalized * _Offset;
		transform.position = pos;
		
		_Rigidbody.velocity = direction.normalized * _Force;
		transform.rotation = Quaternion.LookRotation(direction);
		
	}

	void OnCollisionEnter (Collision other) {
		//Characther c = other.gameObject.GetComponent<Characther>();
		Interactive interactive = other.gameObject.GetComponent<Interactive>();
		if (interactive!=null)
			_CollisionEffect.MakeEffect(_Origin,interactive);

		if (other.rigidbody!=null) {
			other.rigidbody.angularVelocity = Vector3.zero;
			other.rigidbody.velocity = Vector3.zero;
		}

		TurnOn(false);
	}

	void TurnOn (bool on = true) {
		IsOn = on;
		gameObject.SetActive(on);
	}

}
