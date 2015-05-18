using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Root : Attackable {

	public GameObject ParalyzerPrefab;
	List<Paralyzer> _Paralyzers;

	void Start () {
		_Paralyzers = new List<Paralyzer>();
		CreateParalyzers();
	}

	void CreateParalyzers () {
		List<Tile> neighbours  = MapController.Instance.GetNeighbours(MyTile);
		for (int i=0; i<neighbours.Count; i++) {
			if (neighbours[i].OnTop == null) {
				GameObject go = (GameObject)Instantiate(
					ParalyzerPrefab, ParalyzerPrefab.transform.position, 
					ParalyzerPrefab.transform.rotation);
				go.transform.parent = transform;
				
				_Paralyzers.Add(go.GetComponent<Interactive>() as Paralyzer);
				MapController.Instance.PlaceItAt(go.GetComponent<Interactive>(), neighbours[i]);
			}
		}
	}

	void DestroyParalyzers () {
		for (int i=0; i<_Paralyzers.Count; i++)
			_Paralyzers[i].BeDestroyed();
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		DestroyParalyzers();
		Destroy(gameObject);
	}

}
