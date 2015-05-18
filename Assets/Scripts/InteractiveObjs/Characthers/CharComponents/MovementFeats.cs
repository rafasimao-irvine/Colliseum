using UnityEngine;
using System.Collections;

public class MovementFeats {

	private Characther _Characther;
	private CharactherMovement _CharactherMovement;

	private IFeat _NextFeat, _CurrentFeat;

	public MovementFeats (Characther characther, CharactherMovement charactherMovement) {
		_Characther = characther;
		_CharactherMovement = charactherMovement;
	}

	public void Update () {
		if (_NextFeat!=null && _CurrentFeat==null) {
			_CurrentFeat = _NextFeat;
			_NextFeat = null;
			_CurrentFeat.Start(_Characther, _CharactherMovement);
		}
		if (_CurrentFeat!=null) {
			_CurrentFeat.Update();
			if (_CurrentFeat.IsDone())
				_CurrentFeat = null;
		}
	}

	public void AddFeat (IFeat feat) {
		_NextFeat = feat;
	}

	public bool IsFeatsDone () {
		return (_NextFeat==null && _CurrentFeat==null);
	}
}
