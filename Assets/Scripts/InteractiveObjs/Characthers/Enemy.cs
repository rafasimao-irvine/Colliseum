using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Characther {

	//[HideInInspector]
	public Characther TargetChar;

	public GameObject PlaceablePrefab;

	// Attributes
	[SerializeField]
	protected int _VisionRange;
	protected bool _SawPersonage = false;

	[SerializeField]
	protected WeaponEffect _InitialWeapon;
	[SerializeField]
	protected AccessoryEffect _InitialAccessory;

	// Prepared Action
	public enum ActionType {
		None, Move, MoveWithAttack, Attack, SeeTarget,
		Heal, Fusion, MoveAndPlaceTrap, MoveAndAlarm, 
		Invoke, UseAccessory
	}
	public struct Action {
		public ActionType Type;
		public Tile TargetTile;
		public Characther TargetChar;
	}
	public Action PreparedAction;

	// AI's
	/*
	public enum AIAction {
		None, MoveRandom, FollowPersonage, AttackPersonage, 
		RunFromPersonage, RunAttackRand, MoveRandAttackRand,
		MoveRandAndHeal, MoveRandAndFusion, FollowAndFusion,
		FollowAndPlaceTraps, RunAndAlarm, MoveRandAndInvoke,
		UseAccessoryOnTarget
	}
	*/

	[SerializeField]
	protected EnemyAIFabric.AIAction _NoPersonageAction, _SawPersonageAction, _PersonageInRangeAction;
	protected EnemyAI _NoPersonageAI, _SawPersonageAI, _PersonageInRangeAI;

	// Death effects
	[SerializeField]
	protected bool _DisappearUponDeath, _ExplodeUponDeath;
	protected bool _Exploded = false;
	[SerializeField]
	protected int _ExplosionDamage = 1;
	[SerializeField]
	protected VisualEffect _Explosion;

	protected override void Start () {
		base.Start ();
		if (_InitialWeapon!=null)
			TryToEquip(_InitialWeapon.GetWeapon());

		if (_InitialAccessory!=null)
			TryToEquip(_InitialAccessory.GetAccessory());

		_NoPersonageAI = EnemyAIFabric.CreateEnemyAI(_NoPersonageAction);
		_SawPersonageAI = EnemyAIFabric.CreateEnemyAI(_SawPersonageAction);
		_PersonageInRangeAI = EnemyAIFabric.CreateEnemyAI(_PersonageInRangeAction);
	}

	#region Enemy Actions --------------------------------------
	/*public void PrepareTurnAction () {
		MapController mapController = MapController.Instance;
		SetPreparedAction(ActionType.None);

		// If invisible stop seeing it
		if (TargetChar.IsInvisible())
			_SawPersonage = false;

		if (IsDead() || _CharStatus.IsTrapped())
			SetPreparedAction(ActionType.None); // Do nothing

		// If it is close to the target personage, perceive it!
		else if (!_SawPersonage && !TargetChar.IsInvisible() &&
		    mapController.GetNeighbours(MyTile,GetVisionRange()).Contains(TargetChar.MyTile))
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
	}*/

	public void PrepareTurnAction () {
		PrepareTargetChar();
		if (TargetChar == null)
			_SawPersonage = false;

		MapController mapController = MapController.Instance;
		SetPreparedAction(ActionType.None);

		if (IsDead() || _CharStatus.IsTrapped())
			SetPreparedAction(ActionType.None); // Do nothing
		
		// If it is close to the target personage, perceive it!
		else if (!_SawPersonage && TargetChar!=null && !TargetChar.IsInvisible() &&
		         mapController.GetNeighbours(MyTile,GetVisionRange()).Contains(TargetChar.MyTile))
			SetPreparedAction(ActionType.SeeTarget);
		
		// If it haven't seen the player yet
		else if (!_SawPersonage || TargetChar==null || TargetChar.IsDead())
			//AIPrepareAction(_NoPersonageAction);
			AIPrepareAction(_NoPersonageAI);
		
		// If it is close to the target personage, attack!
		else if (mapController.GetDistance(MyTile,TargetChar.MyTile) <= GetCurrentAttackRange())
			//AIPrepareAction(_PersonageInRangeAction);
			AIPrepareAction(_PersonageInRangeAI);
		
		// Otherwise move towards it
		else
			//AIPrepareAction(_SawPersonageAction);
			AIPrepareAction(_SawPersonageAI);
	}

	protected void PrepareTargetChar () {
		MapController map = MapController.Instance;

		// If invisible stop seeing it
		if (TargetChar!=null && (TargetChar == this || TargetChar.IsInvisible() || TargetChar.IsDead()))
			TargetChar = null;

		List<Characther> targets = CharacthersHolder.Instance.GetChars(HuntType);
		int dist = (TargetChar==null) ? GetVisionRange()+1 : map.GetDistance(MyTile, TargetChar.MyTile);
		for (int i=0; i<targets.Count; i++) {
			if (targets[i]!=this && !targets[i].IsInvisible() && !targets[i].IsDead()) {
				int d = map.GetDistance(MyTile,targets[i].MyTile);
				if (dist > d) {
					dist = d;
					TargetChar = targets[i];
				}
			}
		}

	}

	public void SetPreparedAction (ActionType type, Tile tile = null, Characther c = null) {
		PreparedAction.Type = type;
		PreparedAction.TargetTile = tile;
		PreparedAction.TargetChar = c;
	}

	public void SetPreparedAction (ActionType type, Characther c) {
		SetPreparedAction(type,c.MyTile,c);
	}

	/**
	 * The enemy makes its AI action.
	 * Returns 0 to say it ended well, otherwise returns > 0.
	 */
	protected override bool MakeTurnAction () {
		PerformAction(PreparedAction);
		return true;
	}
	#endregion

	#region Actions --------------------------------------
	protected void PerformAction (Action action) {
		switch (action.Type) {
		case ActionType.None:
			break;
		case ActionType.SeeTarget:
			SeeTarget();
			break;
		case ActionType.Move:
			Move(action.TargetTile);
			break;
		case ActionType.MoveWithAttack:
			MoveWithAttack(action.TargetTile);
			break;
		case ActionType.Attack:
			AttackIt(action.TargetTile);
			break;
		case ActionType.Heal:
			HealIt(action.TargetChar);
			break;
		case ActionType.Fusion:
			Fuse((Enemy)action.TargetChar);
			break;
		case ActionType.MoveAndPlaceTrap:
			MoveAndPlaceTrap(action.TargetTile);
			break;
		case ActionType.MoveAndAlarm:
			MoveAndAlarm(action.TargetTile);
			break;
		case ActionType.Invoke:
			CreateAt(action.TargetTile);
			break;
		case ActionType.UseAccessory:
			UseAccessory(action.TargetTile);
			break;
		}
	}

	protected void SeeTarget () {
		if (!_CharStatus.IsBlinded()) {
			_SawPersonage = true;
			TargetChar.BeSaw(this);
			BeSaw(TargetChar);
			Logger.strLog += gameObject.name.Substring(0,gameObject.name.Length-7)+" saw you!\n";
		}
	}

	protected void Move (Tile target) {
		if (IsMoveable(target))
			AddMoveTo(target);
	}

	public bool IsMoveable (Tile tile) {
		return 
			(tile.OnTop == null ||
			 (tile.OnTop != null && !tile.OnTop.Blockable && !tile.OnTop.Unpathable &&
			 (PlaceablePrefab == null || 
			 (PlaceablePrefab!=null && !tile.OnTop.name.Equals(PlaceablePrefab.name+"(Clone)")))
			 ));
	}

	protected void MoveWithAttack (Tile target) {
		Move(target);
		ActivateMoveFowardAtk(MapController.Instance.GetDirection(MyTile,target));
	}

	protected void AttackIt (Tile target) {
		if (IsAttackReady())
			Attack(target);
	}

	protected void HealIt (Characther target) {
		target.Heal(_Strength);
	}

	protected void Fuse (Enemy e) {
		if (!e.IsDead()) {
			e._Strength += _Strength;
			e._MaxLife += _MaxLife;
			e.Heal(_Life);
			BeAttacked(this,_Life);
		}
	}

	protected void MoveAndPlaceTrap (Tile tile) {
		Tile t = MyTile;

		Move(tile);

		CreateAt(t);
	}

	protected void MoveAndAlarm (Tile tile) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile,_VisionRange);
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop != null && tiles[i].OnTop.GetBeAttackedTarget() is Enemy)
				((Enemy)tiles[i].OnTop.GetBeAttackedTarget()).RevealTargetCharacther(TargetChar);
		}

		Move(tile);
	}

	protected void CreateAt (Tile tile) {
		if (PlaceablePrefab!=null && tile.OnTop==null) {
			Interactive iObj = GeneralFabric.CreateObject<Interactive>(PlaceablePrefab, transform.parent);
			MapController.Instance.PlaceItAt(iObj, tile);
			if (iObj is Enemy)
				FindObjectOfType<EnemiesController>().AddEnemy((Enemy)iObj);
		}
	}

	protected void UseAccessory (Tile tile) {
		if (_CharAccessories.GetAccessory(0) != null)
			ActivateAccessory(0, tile);
	}
	#endregion

	#region AIActions --------------------------------------
	protected void AIPrepareAction (EnemyAI ai) {
		if (ai != null)
			ai.PrepareAction(this);
	}
	/**
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
		case AIAction.MoveRandAndHeal:
			MoveRandAndHeal();
			break;
		case AIAction.MoveRandAndFusion:
			MoveRandAndFusion();
			break;
		case AIAction.FollowAndFusion:
			FollowAndFusion();
			break;
		case AIAction.FollowAndPlaceTraps:
			FollowAndPlaceTrap();
			break;
		case AIAction.RunAndAlarm:
			RunAndAlarm();
			break;
		case AIAction.MoveRandAndInvoke:
			MoveRandAndInvoke();
			break;
		case AIAction.UseAccessoryOnTarget:
			UseAccessoryOnTarget();
			break;
		}
	}

	protected void MoveRandom () {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile);

		if (tiles.Count > 0) {
			Tile tile = tiles[Random.Range(0,tiles.Count)];
			SetPreparedAction(ActionType.Move, tile);
		}
	}

	protected void FollowPersonage (Characther target = null) {
		if (target==null) target = TargetChar;
		List<Tile> path = MapController.Instance.FindPath(MyTile, target.MyTile);
		
		// Start the movement
		if (path != null && path.Count > 0)
			SetPreparedAction(ActionType.MoveWithAttack, path[0]);
	}

	protected void AttackPersonage () {
		if (IsAttackReady() && TargetChar!=null)
			SetPreparedAction(ActionType.Attack, TargetChar.MyTile);
	}

	protected bool AttackRandomTile () {
		if (IsAttackReady()) {
			List<Tile> neighbours = MapController.Instance.GetNeighbours(MyTile);
			SetPreparedAction(ActionType.Attack, neighbours[Random.Range(0,neighbours.Count)]);

			return true;
		}
		return false;
	}

	protected void RunFromPersonage () {
		MapController map = MapController.Instance;
		Tile next = map.GetNextTile(MyTile, map.GetDirection(TargetChar.MyTile, MyTile));
		if (next!=null && (next.OnTop==null || (next.OnTop!=null && !next.OnTop.Blockable)) )
			SetPreparedAction(ActionType.Move, next);
		else 
			MoveRandom();
	}

	protected void RunFromPersonageAndAttackRandTile () {
		if (!AttackRandomTile())
			RunFromPersonage();
	}

	protected void MoveRandAndAttackRandTile () {
		if (!AttackRandomTile())
			MoveRandom();
	}

	protected void MoveRandAndHeal () {
		if (!FindToHeal())
			MoveRandom();
	}

	protected bool FindToHeal () {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile,GetVisionRange());

		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop!=null && tiles[i].OnTop.GetBeAttackedTarget() is Enemy) {
				Enemy e = (Enemy)tiles[i].OnTop.GetBeAttackedTarget();
				if (e.GetLife() < e.GetMaxLife()) {
					SetPreparedAction(ActionType.Heal, e);
					return true;
				}
			}
		}

		return false;
	}

	protected void MoveRandAndFusion () {
		if (!MoveToFusion())
			MoveRandom();
	}

	protected void FollowAndFusion () {
		if (!MoveToFusion())
			FollowPersonage();
	}

	protected bool MoveToFusion () {
		List<Tile> tiles = MapController.Instance.GetNeighbours(MyTile,GetVisionRange());
		
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop!=null && tiles[i].OnTop.GetBeAttackedTarget() is Enemy) {
				Enemy e = (Enemy)tiles[i].OnTop.GetBeAttackedTarget();
				if (e.gameObject.name.Equals(gameObject.name)) {
					if (MapController.Instance.GetDistance(MyTile,e.MyTile) > 1)
						FollowPersonage(e);
					else {
						SetPreparedAction(ActionType.Fusion, e);
						e.SetPreparedAction(ActionType.None);
					}
					return true;
				}
			}
		}
		
		return false;
	}

	protected void FollowAndPlaceTrap () {
		MapController map = MapController.Instance;

		Vector2 direction = 
			(map.GetDistance(MyTile,TargetChar.MyTile) > GetVisionRange()) ?
				map.GetDirection(MyTile, TargetChar.MyTile) : 
				map.GetDirection(TargetChar.MyTile, MyTile);

		List<Tile> tiles = map.GetFrontArea(MyTile, direction);

		for (int i=0; i<tiles.Count; i++) {
			if (IsMoveable(tiles[i])) {
				SetPreparedAction(ActionType.MoveAndPlaceTrap, tiles[i]);
				i = tiles.Count;// out of loop
			}
		}

	}

	protected void RunAndAlarm () {
		RunFromPersonage();
		SetPreparedAction(ActionType.MoveAndAlarm ,PreparedAction.TargetTile, PreparedAction.TargetChar);
	}

	protected void MoveRandAndInvoke () {
		List<Tile> neighbours = MapController.Instance.GetNeighbours(MyTile);
		for (int i=0; i<neighbours.Count; i++) {
			if (neighbours[i].OnTop == null) {
				SetPreparedAction(ActionType.Invoke, neighbours[i]);
				i = neighbours.Count;
			}
		}

		if (PreparedAction.Type == ActionType.None)
			MoveRandom();
	}

	protected void UseAccessoryOnTarget () {
		if (TargetChar!=null && !TargetChar.IsDead())
			SetPreparedAction(ActionType.UseAccessory,TargetChar.MyTile);
	}
	*/
	#endregion -------------------------------------------

	#region Explode Upon Death Effect --------------------
	public override void BeAttacked (Interactive iObj, int damage) {
		base.BeAttacked (iObj, damage);
		if (_ExplodeUponDeath && !_Exploded && IsDead()) {
			RefreshMyTile(); // So that it explodes where it was, even if it was moving
			_Exploded = true;
			Explode();
		}
		if (_DisappearUponDeath && IsDead()) {
			CharacthersHolder.Instance.RemoveChar(this);
			Destroy(gameObject);
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

	#region Enemy Vision ----------------------------------
	public void RevealTargetCharacther (Characther target) {
		if (!_CharStatus.IsBlinded() && !_SawPersonage &&
		    target != null && target.MyType == HuntType) {
			TargetChar = target;
			_SawPersonage = true;
			BeSaw(TargetChar);
		}
	}

	public void ForgetTargetCharacther () {
		TargetChar = null;
		_SawPersonage = false;
	}

	public bool SawTarget () {
		return _SawPersonage;
	}

	public int GetVisionRange () {
		int result = (TargetChar==null) ? _VisionRange : 
			_VisionRange + TargetChar.UseEnemyVisionModifier();
		return (result<1) ? 1 : result;
	}
	#endregion

	#region Visibility ----------------------------------
	protected override void BecomeInvisible () {
		ChangeLayersRecursively(transform,9);
	}

	protected override void BecomeVisible () {
		ChangeLayersRecursively(transform,0);
	}

	protected void ChangeLayersRecursively (Transform trans, int layer) {
		foreach (Transform child in trans) {
			child.gameObject.layer = layer;
			ChangeLayersRecursively(child, layer);
		}
	}
	#endregion

	public override void Resurrect () {
		base.Resurrect ();
		TargetChar = null;
		_SawPersonage = false;
	}
}
