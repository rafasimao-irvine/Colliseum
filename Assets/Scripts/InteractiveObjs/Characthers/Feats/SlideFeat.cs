using UnityEngine;
using System.Collections;

public class SlideFeat : IFeat {

	private Characther _Characther;
	private CharactherMovement _CharactherMovement;
	private Transform _Transform;
	
	private bool _Started;
	
	// Movement fields
	private const float _Speed = 4f;
	private Vector3 _VectorSpeed;
	private Vector3 _MoveTo;
	private Tile _TileTarget;
	private bool _IsMoving = false;
	private const float _MoveCloseDistance = 0.1f;


	public SlideFeat (Tile target) {
		_TileTarget = target;
	}

	public void Start (Characther c, CharactherMovement cMove) {
		_Characther = c;
		_CharactherMovement = cMove;
		_Transform = c.transform;

		StartMoveCharTo(_TileTarget);
	}
	
	public void Update () {
		MoveChar();
	}
	
	
	public bool IsDone() {
		return !_IsMoving;
	}

	// Moving characther -------------------------------
	// Move player to the target area
	protected void MoveChar () {
		if (_IsMoving) {
			if (IsCharNearMoveTo() || PassedMoveTo()) {
				_Transform.position = _MoveTo;
				StopMoveCharTo();
			} else {
				_Transform.position += _VectorSpeed*Time.deltaTime;
			}
		}
	}
	
	// Is player near the position it should be?
	private bool IsCharNearMoveTo(){
		return (Vector3.Distance(_Transform.position, _MoveTo) < _MoveCloseDistance);
	}
	
	// Have player passed the _MoveTo target?
	private bool PassedMoveTo () {
		return (Vector3.Dot(_MoveTo-_Transform.position,_VectorSpeed) < 0);
	}
	
	// Stop characther movement
	protected void StopMoveCharTo () {
		if (!_TileTarget.TryGetIn(_Characther))
			_CharactherMovement.AddMoveTo(_Characther.MyTile);
		else
			_Characther.SetMyTile(_TileTarget);

		_IsMoving = false;
	}
	
	// Move player to position
	protected void StartMoveCharTo (Tile tile) {
		if (!_Characther.MyTile.TryGetOut(_Characther)) return;
		
		_MoveTo = tile.transform.position;
		_MoveTo.y = _Transform.position.y;
		
		_IsMoving = true;
		
		// Calculate VectorSpeed
		Vector3 diff = _MoveTo - _Transform.position;
		_VectorSpeed = diff.normalized * _Speed;
	}
	
}
