using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactherMovement {

	// Characther informations
	private Characther _Characther;
	private Transform _Transform;
	private Animation _Animation;
	private Rigidbody _Rigidbody;

	private MovementFeats _MovementFeats;

	// Movement fields
	private const float _Speed = 6f;
	private float _SpeedModifier = 0f; // Used to give burst or slow movement effects
	private Vector3 _VectorSpeed;
	private Vector3 _MoveTo;
	private Tile _MoveToTile;
	private List<Tile> _NextMoveTo = new List<Tile>(); // Used to keep next moveto's
	private bool _IsMoving = false;
	private float _TurnSmoothing = 15f;   // A smoothing value for turning the player.
	private const float _MoveCloseDistance = 0.1f;

	public CharactherMovement (Characther c) {
		_Characther = c;
		_Transform = c.transform;
		_Animation = c.GetComponent<Animation>();
		_Rigidbody = c.GetComponent<Rigidbody>();

		_MovementFeats = new MovementFeats(c, this);
	}

	public void UpdateCharactherMovement () {
		MoveChar();
		_MovementFeats.Update();
	}

	// Moving characther -------------------------------
	// Move player to the target area
	protected void MoveChar () {
		if(_IsMoving){
			if(IsCharNearMoveTo() || PassedMoveTo()){
				_Transform.position = _MoveTo;
				if(_NextMoveTo.Count>0) {
					StartMoveCharTo(_NextMoveTo[0]);
					_NextMoveTo.RemoveAt(0);
				}else{
					StopMoveCharTo();
					if(!_Animation.isPlaying)// Animation
						_Animation.CrossFade("Wait");
				}
			}else{
				_Transform.position += _VectorSpeed*Time.deltaTime;
				Rotating(_VectorSpeed);
				if(!_Animation.isPlaying)// Animation
					_Animation.CrossFade("Walk");
			}
		}else{
			if(!_Animation.isPlaying)// Animation
				_Animation.CrossFade("Wait");
		}
	}
	
	// Rotate the player around to move to the new position
	private void Rotating (Vector3 targetVector){
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetVector, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(
			_Rigidbody.rotation, targetRotation, _TurnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		_Rigidbody.MoveRotation(newRotation);
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
		if (_MoveToTile.TryGetIn(_Characther)) { // Try to get in the target tile
			if (_Characther.IsDead()) 
				_MoveToTile.TryGetOut(_Characther);
			_Characther.SetMyTile(_MoveToTile);
			// finish walking anyway
			_IsMoving = false;
			if (_Animation.IsPlaying("Walk"))
				_Animation.Stop();
		} else { // if it returned null, than comeback to the previous tile
			Debug.Log("Warning: Couldn't set char to next tile");
			if(!_Characther.IsDead())
				StartMoveCharTo(_Characther.MyTile);
		}
	}
	
	// Move player to position
	protected void StartMoveCharTo (Tile tile) {
		// Try to leave the current tile he is on
		if (!_Characther.MyTile.TryGetOut(_Characther)) 
			return;
		
		_MoveToTile = tile;
		_MoveTo = tile.transform.position;
		_MoveTo.y = _Transform.position.y;
		
		_IsMoving = true;
		
		// Calculate VectorSpeed
		Vector3 diff = _MoveTo - _Transform.position;
		_VectorSpeed = diff.normalized * GetSpeed();
		
		// Animation
		_Animation.CrossFade("Walk");
	}
	
	/**
	 * Add position point's to the characther to move to.
	 */
	public void AddMoveTo (Tile tile){
		if (_IsMoving)
			_NextMoveTo.Add(tile); // Add the position to the list
		else
			StartMoveCharTo(tile); // Start moving to the position
	}

	public void InterruptMoves () {
		if (_IsMoving)
			StopMoveCharTo();
	}
	
	// Make player look at desired direction
	public void LookAtDirection (Vector3 direction){
		// Garantee no changes in the y axis
		direction.y = 0f;
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		
		// Change the players rotation to this new rotation.
		_Rigidbody.MoveRotation(targetRotation);
	}
	//--------------------------------------------------

	// Movement Feats ----------------------------------
	public void AddMovementFeat (IFeat feat) {
		_MovementFeats.AddFeat(feat);
	}
	//--------------------------------------------------

	// Speed
	public float GetSpeed () {
		return _Speed+_SpeedModifier;
	}

	public void SetSpeedModifier (float mod) {
		_SpeedModifier = mod;
	}

	/**
	 * Is this characther moving?
	 */
	public bool IsMoving () {
		return _IsMoving;
	}
	
	public bool IsInAction () {
		return ((_Animation!=null && _Animation.isPlaying && !_Animation.IsPlaying("Wait")) || 
		        _IsMoving || !_MovementFeats.IsFeatsDone());
	}

}
