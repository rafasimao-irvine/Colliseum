using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : TurnController {

	// Used to know if its currently playing
	private bool _StartTurnCoroutine = false;
	private bool _MadeAction;

	// Enemies
	public Creatable[] Enemies;

	private List<Enemy> _Enemies;

	public PlayerController PlayerController;

	void Awake () {
		_Enemies = new List<Enemy>();
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
			if(!anyPlaying)
				_MadeAction = _IsMyTurn=false;
		}
	}
	
	private IEnumerator MakeEnemiesAIActions() {
		foreach (Enemy e in _Enemies)
			e.PrepareTurnAction();

		ChainEnemiesSeeTarget();

		CheckEnemiesMoves();

		foreach (Enemy e in _Enemies) {
			while (!e.MakeAction())
				yield return new WaitForSeconds(0.1f);
		}

		foreach (Enemy e in _Enemies) {
			while (e.IsInAction()) // Makes it iterate one by one
				yield return new WaitForSeconds(0.1f);
		}

		_MadeAction = true;
	}

	private void ChainEnemiesSeeTarget () {
		List<Enemy> eWhoSaw = new List<Enemy>();
		foreach (Enemy e in _Enemies) {
			if (e.PreparedAction.Type == Enemy.ActionType.SeeTarget &&
			    e.TargetChar == PlayerController.PlayerPersonage)
				eWhoSaw.Add(e);
		}

		MapController map = MapController.Instance;

		foreach (Enemy eSaw in eWhoSaw) {
			foreach (Enemy e in _Enemies) {
				if (e.PreparedAction.Type != Enemy.ActionType.SeeTarget && 
				    !e.SawTarget() && 
				    eSaw.TargetChar == e.TargetChar && 
				    map.GetDistance(eSaw.MyTile, e.MyTile) <= eSaw.GetVisionRange())
					e.SetPreparedAction(Enemy.ActionType.SeeTarget);
			}
		}
	}

	private void CheckEnemiesMoves () {
		List<Tile> placed = new List<Tile>();
		foreach (Enemy e in _Enemies) {
			if (e.PreparedAction.Type == Enemy.ActionType.Move || 
			    e.PreparedAction.Type == Enemy.ActionType.MoveWithAttack) {
				if (!placed.Contains(e.PreparedAction.Target))
					placed.Add (e.PreparedAction.Target);
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
				_Enemies[_Enemies.Count-1].TargetChar = PlayerController.PlayerPersonage;

				MapController.Instance.PlaceIt(
					MapController.Instance.GetMapTiles(), _Enemies[_Enemies.Count-1]);
			}
		}
	}

	public void LoadEnemies (ObjectStance[] enemiesStances) {
		for (int i=0; i<enemiesStances.Length; i++) {
			_Enemies.Add(GeneralFabric.CreateObject<Enemy>(enemiesStances[i].Prefab, transform));
			_Enemies[_Enemies.Count-1].TargetChar = PlayerController.PlayerPersonage;

			MapController.Instance.PlaceItAt(
				_Enemies[_Enemies.Count-1],enemiesStances[i].X,enemiesStances[i].Y);
		}
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
