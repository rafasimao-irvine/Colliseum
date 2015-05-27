using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectCreator : MonoBehaviour {

	enum CreationArea {
		Neighbours, Line, Randomly
	}

	[SerializeField]
	private GameObject _ObjectsPrefab;
	[SerializeField]
	private CreationArea _CreationArea;
	[SerializeField]
	private int _AreaRange = 1;
	private List<Interactive> _Objects;

	[SerializeField]
	private bool _DestroyObjectsUponDestruction;

	void Start () {
		_Objects = new List<Interactive>();

		switch (_CreationArea) {
		case CreationArea.Neighbours:
			CreateNeighbourObjects();
			break;
		case CreationArea.Line:
			CreateLineObjects();
			break;
		case CreationArea.Randomly:
			CreateRandomObjects();
			break;
		}

	}

	// Create objects through all neighbours in the area range
	void CreateNeighbourObjects () {
		List<Tile> tiles  = MapController.Instance.GetNeighbours(
			GetComponent<Interactive>().MyTile, _AreaRange);
		CreateObjects(tiles);
	}

	// Create objects in a line random direction
	void CreateLineObjects () {
		List<Tile> tiles  = MapController.Instance.GetLine(
			GetComponent<Interactive>().MyTile, new Vector2(Random.Range(-1,2),Random.Range(-1,2)), _AreaRange);

		// Rotate towards
		if (tiles.Count>0) {
			Quaternion rot =  Quaternion.LookRotation(tiles[0].transform.position - transform.position);
			rot.x = rot.z = 0;
			transform.rotation = rot;
		}

		CreateObjects(tiles);
	}

	// Create a object at a random place
	void CreateRandomObjects () {
		List<Tile> tiles = new List<Tile>();
		for (int i=0; i<_AreaRange; i++)
			tiles.Add(MapController.Instance.GetRandomFreeTile());
		CreateObjects(tiles);
	}

	// Create the objects and place them at the specified tiles
	void CreateObjects (List<Tile> tiles) {
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop == null) {
				_Objects.Add(GeneralFabric.CreateObject<Interactive>(_ObjectsPrefab, transform.parent));
				MapController.Instance.PlaceItAt(_Objects[_Objects.Count-1], tiles[i]);
			}
		}
	}

	//Destroy the objects created
	void DestroyObjects () {
		for (int i=0; i<_Objects.Count; i++)
			_Objects[i].BeDestroyed();
	}

	// If setted to do so, it destroies the objects with it
	void OnDestroy () {
		if(_DestroyObjectsUponDestruction)
			DestroyObjects();
	}

	public List<Interactive> GetObjects() {
		return _Objects;
	}

}
