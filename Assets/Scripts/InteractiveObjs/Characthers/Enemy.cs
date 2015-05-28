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

	// AI's
	protected enum EnemyAIAction {
		None, MoveRandom, FollowPersonage, AttackPersonage, 
		RunFromPersonage, RunAttackRand, MoveRandAttackRand
	}
	[SerializeField]
	protected EnemyAIAction _NoPersonageAction, _SawPersonageAction, _PersonageInRangeAction;

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
	/**
	 * The enemy makes its AI action.
	 * Returns 0 to say it ended well, otherwise returns > 0.
	 */
	protected override bool MakeTurnAction () {
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
	}

	protected int GetVisonRange () {
		int result = _VisionRange + TargetChar.UseEnemyVisionModifier();
		return (result<1) ? 1 : result;
	}

	protected void PerformAction (EnemyAIAction action) {
		switch (action) {
		case EnemyAIAction.None:
			break;
		case EnemyAIAction.MoveRandom:
			MoveRandom();
			break;
		case EnemyAIAction.FollowPersonage:
			FollowPersonage();
			break;
		case EnemyAIAction.AttackPersonage:
			AttackPersonage();
			break;
		case EnemyAIAction.RunFromPersonage:
			RunFromPersonage();
			break;
		case EnemyAIAction.RunAttackRand:
			RunFromPersonageAndAttackRandTile();
			break;
		case EnemyAIAction.MoveRandAttackRand:
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
			if (tile.OnTop == null || (tile.OnTop != null && !tile.OnTop.Blockable))
				AddMoveTo(tile);
		}
	}

	protected void FollowPersonage () {
		List<Tile> path = MapController.Instance.FindPath(MyTile, TargetChar.MyTile);
		
		// Start the movement
		if (path != null && path.Count > 0)
			AddMoveTo(path[0]);
	}

	protected void AttackPersonage () {
		if (IsAttackReady() && TargetChar!=null)
			Attack(TargetChar);
	}

	protected bool AttackRandomTile () {
		if (IsAttackReady()) {
			List<Tile> neighbours = MapController.Instance.GetNeighbours(MyTile);
			Attack(neighbours[Random.Range(0,neighbours.Count)]);

			return true;
		}
		return false;
	}

	protected void RunFromPersonage () {
		MapController map = MapController.Instance;
		Tile next = map.GetNextTile(MyTile, map.GetDirection(TargetChar.MyTile, MyTile));
		if (next!=null && (next.OnTop==null || (next.OnTop!=null && !next.OnTop.Blockable)) )
			AddMoveTo(next);
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
}
