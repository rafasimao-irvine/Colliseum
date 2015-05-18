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
		foreach(Enemy e in _Enemies) {
			e.MakeAction();
			yield return null;
		}
		_MadeAction = true;
	}

	/**
	 * Create Enemies
	 */
	public void CreateEnemies() {
		for (int i=0; i<Enemies.Length; i++) {
			int quantity = Enemies[i].RandQuantity;
			for (int q=0; q<quantity; q++){
				_Enemies.Add(GeneralFabric.CreateObject<Enemy>(Enemies[i].Prefab, transform));
				_Enemies[_Enemies.Count-1].PlayerPersonage = PlayerController.PlayerPersonage;
				MapController.Instance.PlaceIt(
					MapController.Instance.GetMapTiles(), _Enemies[_Enemies.Count-1]);
			}
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
