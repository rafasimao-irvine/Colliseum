using UnityEngine;
using System.Collections;

public class JumpFeat : IFeat {

	private Characther _Characther;
	private CharactherMovement _CharactherMovement;
	private Rigidbody _Rigidbody;

	private bool _Started;

	private Tile _TileTarget;

	public JumpFeat (Tile target) {
		_TileTarget = target;
	}

	public void Start (Characther c, CharactherMovement cMove) {
		_Characther = c;
		_CharactherMovement = cMove;
		_Rigidbody = c.GetComponent<Rigidbody>();

		_Started = true;
		// Leave
		_Characther.MyTile.TryGetOut(_Characther);
		// Jump
		JumpTo(_TileTarget);
	}
	
	public void Update () {
		// Finish jump
		if (_Started && IsDone()) {
			_Started = false;
			// Enter
			if (_TileTarget.TryGetIn(_Characther))
				_Characther.SetMyTile(_TileTarget);
			else // Just in case:
				_CharactherMovement.AddMoveTo(_Characther.MyTile);
		}
	}

	private void JumpTo (Tile target) {
		GeneralPhysics.Throw(_Rigidbody, target.transform.position);
	}

	public bool IsDone() {
		return (_Rigidbody.velocity == Vector3.zero);
	}

}
