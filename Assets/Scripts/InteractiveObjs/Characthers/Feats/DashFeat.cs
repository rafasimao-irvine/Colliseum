using UnityEngine;
using System.Collections;

public class DashFeat : IFeat {

	private Tile _Target;
	private float _SpeedModifier = 6f;
	private bool _Finished;

	public DashFeat (Tile target) {
		Reset(target);
	}

	public void Start (Characther c, CharactherMovement cMove) {
		cMove.SetSpeedModifier(_SpeedModifier);
		cMove.AddMoveTo(_Target);
		cMove.SetSpeedModifier(0f);
		_Finished = true;
	}

	public void Update () {
		// Do nothing
	}

	public bool IsDone () {
		return _Finished;
	}

	public void Reset (Tile target) {
		_Target = target;
		_Finished = false;
	}
}
