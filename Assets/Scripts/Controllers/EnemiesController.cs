using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : TurnController {

	// Used to know if its currently playing
	private bool _StartTurnCoroutine = false;
	private bool _MadeAction;

	// Enemies
	public Creatable[] Enemies;

	private List<Enemy> _Enemies, _EnemiesOnWait;

	public PlayerController PlayerController;

	void Awake () {
		_Enemies = new List<Enemy>();
		_EnemiesOnWait = new List<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_StartTurnCoroutine) {
			StartCoroutine("MakeEnemiesAIActions");
			_StartTurnCoroutine = false;
		}

		if (_IsMyTurn)
			VerifyEndOfTurn();
	}

	private void VerifyEndOfTurn () {
		if (_MadeAction) {
			bool anyPlaying = false;
			foreach(Enemy e in _Enemies)
				if(e.IsInAction())
					anyPlaying = true;
			if(!anyPlaying) {
				_MadeAction = _IsMyTurn=false;
				IntegrateAddedEnemies();
			}
		}
	}

	private void IntegrateAddedEnemies () {
		if (_EnemiesOnWait.Count > 0) {
			_Enemies.AddRange(_EnemiesOnWait);
			_EnemiesOnWait.Clear();
		}
	}

	/**
	 * Go through all the enemies prepare their actions and
	 * performs collective enemies arrangements.
	 * */
	private IEnumerator MakeEnemiesAIActions() {
		// Wait possible effects from player turn
		foreach (Enemy e in _Enemies) {
			while (e.IsInAction())
				yield return new WaitForSeconds(0.1f);
		}

		// Prepare enemies
		foreach (Enemy e in _Enemies)
			e.PrepareTurnAction();

		// Group Vision
		ChainEnemiesSeeTarget();
		// Group Movements
		CheckEnemiesMoves();

		// Make actions
		foreach (Enemy e in _Enemies) {
			while (!e.MakeAction())
				yield return new WaitForSeconds(0.1f);
		}

		// Verify end of actions
		foreach (Enemy e in _Enemies) {
			while (e.IsInAction()) // Makes it iterate one by one
				yield return new WaitForSeconds(0.1f);
		}

		_MadeAction = true;
	}

	// Chains the target revealing vision to the near enemies
	private void ChainEnemiesSeeTarget () {
		List<Enemy> eWhoSaw = new List<Enemy>();
		foreach (Enemy e in _Enemies) {
			if (e.PreparedAction.Type == Enemy.ActionType.SeeTarget)
			    //&& e.TargetChar == PlayerController.PlayerPersonage)
				eWhoSaw.Add(e);
		}

		MapController map = MapController.Instance;

		foreach (Enemy eSaw in eWhoSaw) {
			foreach (Enemy e in _Enemies) {
				if (e.PreparedAction.Type != Enemy.ActionType.SeeTarget && 
				    !e.SawTarget() && 
				    eSaw.HuntType == e.HuntType && 
				    //eSaw.TargetChar != e &&
				    map.GetDistance(eSaw.MyTile, e.MyTile) <= eSaw.GetVisionRange()) {
					e.SetPreparedAction(Enemy.ActionType.SeeTarget);
					e.TargetChar = eSaw.TargetChar;
				}
			}
		}
	}

	// Checks if the enemies moves dont tresspass one another
	private void CheckEnemiesMoves () {
		List<Tile> placed = new List<Tile>();
		foreach (Enemy e in _Enemies) {
			if (e.PreparedAction.Type == Enemy.ActionType.Move || 
			    e.PreparedAction.Type == Enemy.ActionType.MoveWithAttack || 
			    e.PreparedAction.Type == Enemy.ActionType.MoveAndPlaceTrap ||
			    e.PreparedAction.Type == Enemy.ActionType.MoveAndAlarm ||
			    e.PreparedAction.Type == Enemy.ActionType.Invoke) {
				if (!placed.Contains(e.PreparedAction.TargetTile))
					placed.Add (e.PreparedAction.TargetTile);
				else
					e.SetPreparedAction(Enemy.ActionType.None);
			}
		}
	}

	/**
	 * Create Enemies
	 */
	public void CreateEnemies() {
		for (int i=0; i<Enemies.Length; i++) {
			int quantity = Enemies[i].RandQuantity;
			for (int q=0; q<quantity; q++){
				_Enemies.Add(GeneralFabric.CreateObject<Enemy>(Enemies[i].Prefab, transform));
				//_Enemies[_Enemies.Count-1].TargetChar = PlayerController.PlayerPersonage;

				MapController.Instance.PlaceIt(
					MapController.Instance.GetMapTiles(), _Enemies[_Enemies.Count-1]);
			}
		}
	}

	public void LoadEnemies (ObjectStance[] enemiesStances) {
		for (int i=0; i<enemiesStances.Length; i++) {
			_Enemies.Add(GeneralFabric.CreateObject<Enemy>(enemiesStances[i].Prefab, transform));
			//_Enemies[_Enemies.Count-1].TargetChar = PlayerController.PlayerPersonage;

			MapController.Instance.PlaceItAt(
				_Enemies[_Enemies.Count-1],enemiesStances[i].X,enemiesStances[i].Y);
		}
	}

	public void AddEnemy (Enemy e) {
		_EnemiesOnWait.Add(e);
		//e.TargetChar = PlayerController.PlayerPersonage;
	}

	/**
	 * Get the list of enemies
	 */
	public List<Enemy> GetEnemies() {
		return _Enemies;
	}

	/**
	 * Stats the controller turn.
	 * */
	public override void StartMyTurn() {
		_IsMyTurn = true;
		_StartTurnCoroutine = true;
	}
}
