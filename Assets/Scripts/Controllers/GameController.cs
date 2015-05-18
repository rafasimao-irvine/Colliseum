using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	enum Turns {
		PlayerTurn, EnemiesTurn, ObjectsTurn
	}
	private Turns _CurrentTurn = Turns.PlayerTurn;

	public PlayerController GCPlayerController;
	public EnemiesController GCEnemiesController;
	public ObjectsController GCObjectsController;
	public MapController GCMapController;

	// Use this for initialization
	void Start () {

		GCMapController.CreateMap(); // Create arena
		GCObjectsController.CreateObjects(); // And its objects

		// Gives the player controller to the enemies
		GCEnemiesController.PlayerController = GCPlayerController;
		GCEnemiesController.CreateEnemies(); // Create enemies

		// Place all the personages
		GCMapController.PlaceIt(GCMapController.GetMapTiles(),
		                        GCPlayerController.PlayerPersonage);

		// Sets the player to start the game
		_CurrentTurn = Turns.PlayerTurn;
		GCPlayerController.StartMyTurn();
	}

	// Update is called once per frame
	void Update () {

		// Verify if players turn ended, if so starts enemies turn
		if(_CurrentTurn==Turns.PlayerTurn && !GCPlayerController.GetIsMyTurn()) {
			_CurrentTurn = Turns.EnemiesTurn;
			GCEnemiesController.StartMyTurn();
		
		// Verify if the enemies turn ended, if so starts players turn
		}else if(_CurrentTurn==Turns.EnemiesTurn && !GCEnemiesController.GetIsMyTurn()){
			_CurrentTurn=Turns.ObjectsTurn;
			GCObjectsController.StartMyTurn();
		}
		// Verify if if objects turn ended
		else if (_CurrentTurn==Turns.ObjectsTurn && !GCObjectsController.GetIsMyTurn()) {
			_CurrentTurn=Turns.PlayerTurn;
			GCPlayerController.StartMyTurn();
		}
	}

	// DEBUG ============================================
	public Tile previous;

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		if(GCPlayerController.PlayerPersonage != null && GCPlayerController.PlayerPersonage.MyTile != null)
			foreach(Tile t in GCMapController.GetNeighbours(GCPlayerController.PlayerPersonage.MyTile, 3))
			//foreach(Tile t in GCMapController.GetFlowerTiles(GCPlayerController.PlayerPersonage.MyTile, new Vector2(-1,0)))
				Gizmos.DrawCube(t.transform.position, Vector3.one);
	}
}
