using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[IntegrationTest.DynamicTest("FindPathTest")]
public class FindPathTest : MonoBehaviour {

	MapController _MapController;
	Tile[,] _MapTiles;
	public GameObject Obstacle;

	// Use this for initialization
	void Start () {
		_MapController = GetComponent<MapController>();
		_MapController.CreateMap();
		_MapTiles = _MapController.GetMapTiles();

		FindPath_Null_Null();
		FindPath_Free_ShorterPath();
		FindPath_Block_ShorterPath();
		//FindPath_BlockAndBlockedEnd_ShorterPath();
		IntegrationTest.Pass(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void FindPath_Null_Null () {
		List<Tile> path = _MapController.FindPath(null,null);
		IntegrationTest.Assert(path == null); // return a path
	}

	private void FindPath_Free_ShorterPath () {
		List<Tile> path = _MapController.FindPath(_MapTiles[5,3],_MapTiles[8,8]);
		IntegrationTest.Assert(path != null); // return a path
		IntegrationTest.Assert(path.Count == 8); // smaller path
		IntegrationTest.Assert(AreNeighbours(path)); // path tiles are neighbours
		// First tile is neighbour of the origin:
		IntegrationTest.Assert(_MapController.GetNeighbours(_MapTiles[5,3],1).Contains(path[0]));
		IntegrationTest.Assert(path[path.Count-1] == _MapTiles[8,8]); // Found the target tile
	}

	private void FindPath_Block_ShorterPath () {
		CreateObstacle(4,6);
		CreateObstacle(5,6);
		CreateObstacle(6,5);
		CreateObstacle(7,4);
		CreateObstacle(8,4);
		List<Tile> path = _MapController.FindPath(_MapTiles[5,3],_MapTiles[8,8]);

		IntegrationTest.Assert(path != null); // return a path
		IntegrationTest.Assert(path.Count == 10); // smaller path
		IntegrationTest.Assert(AreNeighbours(path)); // path tiles are neighbours
		// First tile is neighbour of the origin:
		IntegrationTest.Assert(_MapController.GetNeighbours(_MapTiles[5,3],1).Contains(path[0]));
		IntegrationTest.Assert(path[path.Count-1] == _MapTiles[8,8]); // Found the target tile
	}

	private void FindPath_BlockAndBlockedEnd_ShorterPath () {
		CreateObstacle(8,8);
		List<Tile> path = _MapController.FindPath(_MapTiles[5,3],_MapTiles[8,8]);
		
		IntegrationTest.Assert(path != null); // return a path
		Debug.Log(path.Count);
		IntegrationTest.Assert(path.Count == 9); // smaller path
		IntegrationTest.Assert(AreNeighbours(path)); // path tiles are neighbours
		// First tile is neighbour of the origin:
		IntegrationTest.Assert(_MapController.GetNeighbours(_MapTiles[5,3],1).Contains(path[0]));
		// Found the target tile:
		IntegrationTest.Assert(_MapController.GetNeighbours(_MapTiles[8,8],1).Contains(path[path.Count-1]));
	}

	private void CreateObstacle (int x, int z) {
		GameObject go = 
			Instantiate(Obstacle, 
			            new Vector3(_MapTiles[x,z].transform.position.x, 
			            Obstacle.transform.position.y, _MapTiles[x,z].transform.position.z),
		                Quaternion.identity) as GameObject;

		_MapTiles[x,z].TryGetIn(go.GetComponent<Interactive>());
	}

	private bool AreNeighbours (List<Tile> path) {
		for (int i=0; i<path.Count-1; i++) {
			if (!_MapController.GetNeighbours(path[i],1).Contains(path[i+1]))
				return false;
		}
		return true;
	}

}
