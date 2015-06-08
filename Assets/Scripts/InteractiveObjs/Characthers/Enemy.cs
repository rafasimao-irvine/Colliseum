using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Characther {

	//[HideInInspector]
	public Characther TargetChar;

	// Attributes
	[SerializeField]
	protected int _VisionRange;
	protected bool _SawPersonage = false;

	[SerializeField]
	protected WeaponEffect _InitialWeapon;

	// Prepared Action
	public enum ActionType {None, Move, MoveWithAttack, Attack, SeeTarget}
	public struct Action {
		public ActionType Type;
		public Tile Target;
	}
	public Action PreparedAction;

	// AI's
	protected enum AIAction {
		None, MoveRandom, FollowPersonage, AttackPersonage, 
		RunFromPersonage, RunAttackRand, MoveRandAttackRand
	}
	[SerializeField]
	protected AIAction _NoPersonageAction, _SawPersonageAction, _PersonageInRangeAction;

	// Boomer
	[SerializeField]
	protected bool _ExplodeUponDeath;
	protected bool _Exploded = false;
	[SerializeField]
	protected int _ExplosionDamage = 1;
	[SerializeField]
	protected VisualEffect _Explosion;

	protected override void Start () {
		base.Start ();
		if (_InitialWeapon!=null)
			TryToEquip(_InitialWeapon.GetWeapon());
	}

	#region Enemy Actions --------------------------------------

	public void PrepareTurnAction () {
		MapController mapController = MapController.Instance;
		if (IsDead() || _CharStatus.IsTrapped())
			SetPreparedAction(ActionType.None); // Do nothing

		// If it is close to the target personage, perceive it!
		else if (!_SawPersonage &&
		    mapController.GetNeighbours(MyTile,GetVisonRange()).Contains(TargetChar.MyTile))
			SetPreparedAction(ActionType.SeeTarget);
		
		// If it haven't seen the player yet
		else if (!_SawPersonage || TargetChar.IsDead())
			AIPrepareAction(_NoPersonageAction);
		
		// If it is close to the target personage, attack!
		else if (mapController.GetNeighbours(MyTile,GetCurrentAttackRange()).Contains(TargetChar.MyTile))
			AIPrepareAction(_PersonageInRangeAction);
		
		// Otherwise move towards it
		else
			AIPrepareAction(_SawPersonageAction);
	}

	protected void SetPreparedAction (ActionType type, Tile target = null) {
		PreparedAction.Type = type;
		PreparedAction.Target = target;
	}

	/**
	 * The enemy makes its AI action.
	 * Returns 0 to say it ended well, otherwise returns > 0.
	 */
	protected override bool MakeTurnAction () {
		PerformAction(PreparedAction);
		return true;
		/*
		MapController mapController = MapController.Instance;
		// If it is close to the target personage, perceive it!
		if (!_SawPersonage && 
		    mapController.GetNeighbours(MyTile,GetVisonRange()).Contains(TargetChar.MyTile)) {
			_SawPersonage = true;
			TargetChar.BeSaw(this);
			BeSaw(TargetChar);
			Logger.strLog += gameObject.name.Substring(0,gameObject.name.Length-7)+" saw you!\n";
			return true; // END the action
		}

		// If it haven't seen the player yet
		if (!_SawPersonage || TargetChar.IsDead()) {
			PerformAction(_NoPersonageAction);
			return true; // END the action
		}

		// If it is close to the target personage, attack!
		if (mapController.GetNeighbours(MyTile,GetCurrentAttackRange()).Contains(TargetChar.MyTile)) {
			PerformAction(_PersonageInRangeAction);
			return true; // END the action
		}

		// Otherwise move towards it
		PerformAction(_SawPersonageAction);

		return true;
		*/
	}

	protected void PerformAction (Action action) {
		switch (action.Type) {
		case ActionType.None:
			break;
		case ActionType.SeeTarget:
			SeeTarget();
			break;
		case ActionType.Move:
			Move(action.Target);
			break;
		case ActionType.MoveWithAttack:
			MoveWithAttack(action.Target);
			break;
		case ActionType.Attack:
			if (IsAttackReady())
				Attack(action.Target);
			break;
		}
	}

	protected void SeeTarget () {
		_SawPersonage = true;
		TargetChar.BeSaw(this);
		BeSaw(TargetChar);
		Logger.strLog += gameObject.name.Substring(0,gameObject.name.Length-7)+" saw you!\n";
	}

	protected void Move (Tile target) {
		if (target.OnTop == null || 
		    (target.OnTop != null && !target.OnTop.Blockable && !target.OnTop.Unpathable))
			AddMoveTo(target);
	}

	protected void MoveWithAttack (Tile target) {
		Move(target);
		ActivateMoveFowardAtk(MapController.Instance.GetDirection(MyTile,target));
	}


	protected int GetVisonRange () {
		int result = _VisionRange + TargetChar.UseEnemyVisionModifier();
		return (result<1) ? 1 : result;
	}

	protected void AIPrepareAction (AIAction action) {
		switch (action) {
		case AIAction.None:
			break;
		case AIAction.MoveRandom:
			MoveRandom();
			break;
		case AIAction.FollowPersonage:
			FollowPersonage();
			break;
		case AIAction.AttackPersonage:
			AttackPersonage();
			break;
		case AIAction.RunFromPersonage:
			RunFromPersonage();
			break;
		case AIAction.RunAttackRand:
			RunFromPersonageAndAttackRandTile();
			break;
		case AIAction.MoveRandAttackRand:
			MoveRandAndAttackRandTile();
			break;
		}
	}
	#endregion

	#region AI Actions -----------------------------------
	protected void MoveRandom () {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile);

		if (tiles.Count > 0) {
			Tile tile = tiles[Random.Range(0,tiles.Count)];
			//if (tile.OnTop == null || (tile.OnTop != null && !tile.OnTop.Blockable))
			//	AddMoveTo(tile);
			SetPreparedAction(ActionType.Move, tile);
		}
	}

	protected void FollowPersonage () {
		List<Tile> path = MapController.Instance.FindPath(MyTile, TargetChar.MyTile);
		
		// Start the movement
		if (path != null && path.Count > 0) {
			//AddMoveTo(path[0]);
			//ActivateMoveFowardAtk(MapController.Instance.GetDirection(MyTile,path[0]));
			SetPreparedAction(ActionType.MoveWithAttack, path[0]);
		}
	}

	protected void AttackPersonage () {
		if (IsAttackReady() && TargetChar!=null)
			//Attack(TargetChar);
			SetPreparedAction(ActionType.Attack, TargetChar.MyTile);
	}

	protected bool AttackRandomTile () {
		if (IsAttackReady()) {
			List<Tile> neighbours = MapController.Instance.GetNeighbours(MyTile);
			//Attack(neighbours[Random.Range(0,neighbours.Count)]);
			SetPreparedAction(ActionType.Attack, neighbours[Random.Range(0,neighbours.Count)]);

			return true;
		}
		return false;
	}

	protected void RunFromPersonage () {
		MapController map = MapController.Instance;
		Tile next = map.GetNextTile(MyTile, map.GetDirection(TargetChar.MyTile, MyTile));
		if (next!=null && (next.OnTop==null || (next.OnTop!=null && !next.OnTop.Blockable)) )
			//AddMoveTo(next);
			SetPreparedAction(ActionType.Move, next);
		else 
			MoveRandom();
	}

	protected void RunFromPersonageAndAttackRandTile() {
		if (!AttackRandomTile())
			RunFromPersonage();
	}

	protected void MoveRandAndAttackRandTile () {
		if (!AttackRandomTile())
			MoveRandom();
	}

	#endregion -------------------------------------------

	#region Explode Upon Death Effect --------------------
	public override void BeAttacked (Interactive iObj, int damage) {
		base.BeAttacked (iObj, damage);
		if (_ExplodeUponDeath && !_Exploded && IsDead()) {
			RefreshMyTile(); // So that it explodes where it was, even if it was moving
			_Exploded = true;
			Explode();
		}
	}

	private void Explode () {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile);
		if (_Explosion!=null)
			_Explosion.MakeEffect(this, null);
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop != null && tiles[i].OnTop.Attackable)
				tiles[i].OnTop.BeAttacked(this,_ExplosionDamage);
		}
	}
	#endregion --------------------------------------------

	public void RevealTargetCharacther () {
		_SawPersonage = true;
		BeSaw(TargetChar);
	}

	public bool SawTarget () {
		return _SawPersonage;
	}

	public int GetVisionRange () {
		return _VisionRange;
	}
}
