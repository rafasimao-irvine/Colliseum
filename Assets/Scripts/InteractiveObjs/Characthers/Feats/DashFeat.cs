using UnityEngine;
using System.Collections;

public class DashFeat : IFeat {

	protected Tile _Target;
	protected float _SpeedModifier;
	protected bool _Finished;

	public DashFeat (Tile target, float speedMod) {
		Reset(target);
		_SpeedModifier = speedMod;
	}

	virtual public void Start (Characther c, CharactherMovement cMove) {
		if (!c.IsParalized()) {
			cMove.SetSpeedModifier(_SpeedModifier);
			cMove.AddMoveTo(_Target);
			c.ActivateMoveFowardAtk(MapController.Instance.GetDirection(c.MyTile,_Target));
			cMove.SetSpeedModifier(0f);
		}
		_Finished = true;
	}

	virtual public void Update () {
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
