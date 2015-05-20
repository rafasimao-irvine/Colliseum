using UnityEngine;
using System.Collections;

public class GhostRunFeat : DashFeat {

	private CharactherMovement _CharMove;
	private Collider _CharCollider;
	private Rigidbody _CharRigidBody;

	public GhostRunFeat (Tile target, float speedMod) : base(target, speedMod) {
	}

	public override void Start (Characther c, CharactherMovement cMove) {
		base.Start (c, cMove);
		_Finished = false;

		_CharMove = cMove;
		
		_CharCollider = c.GetComponent<Collider>();
		_CharCollider.enabled = false;
		_CharRigidBody = c.GetComponent<Rigidbody>();
		_CharRigidBody.useGravity = false;
	}

	public override void Update () {
		base.Update ();

		if (!_CharMove.IsMoving()) {
			_CharCollider.enabled = true;
			_CharRigidBody.useGravity = true;
			_Finished = true;
		}
	}
}
