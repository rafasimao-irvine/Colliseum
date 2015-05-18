using UnityEngine;
using System.Collections;

public class PortalFeat : IFeat {

	private Characther _Characther;
	private CharactherMovement _CharactherMovement;

	private Tile _TileClone, _TileTarget;

	private bool _Finished;

	public PortalFeat (Tile clone, Tile target) {
		_TileClone = clone;
		_TileTarget = target;

		_Finished = false;
	}

	public void Start (Characther c, CharactherMovement cMove) {
		_Characther = c;
		_CharactherMovement = cMove;

		_Finished = false;

		PortalTeleportTo();
	}

	public void Update () {}

	private void PortalTeleportTo () {
		if (_TileTarget.OnTop==null || !_TileTarget.OnTop.Blockable) {
			// Set to clone's position
			_Characther.transform.position = new Vector3(
				_TileClone.transform.position.x,
				_Characther.transform.position.y,
				_TileClone.transform.position.z );

			_Characther.MyTile.TryGetOut(_Characther); // Leave
			_Characther.SetMyTile(_TileClone); // Goes to clone tile to move from it
			_CharactherMovement.AddMoveTo(_TileTarget); // Moves to the target tile
		}
		_Finished = true;
	}

	public bool IsDone () {
		return _Finished;
	}

}
