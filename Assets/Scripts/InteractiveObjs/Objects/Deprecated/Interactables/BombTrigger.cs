using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombTrigger : Interactable {

	public GameObject TntPrefab;
	private static List<Tnt> _Tnts = null;
	private static int _NTnts = 5;

	private bool _Used = false;

	void Start () {
		if(_Tnts==null)
			BombTrigger.CreateTnts(TntPrefab, transform.parent);
	}

	private static void CreateTnts (GameObject prefab, Transform parent) {
		_Tnts = new List<Tnt>();
		for (int i=0; i<_NTnts; i++) {
			GameObject go = (GameObject)Instantiate(
				prefab, prefab.transform.position, Quaternion.identity);
			_Tnts.Add(go.GetComponent<Tnt>());
			MapController.Instance.PlaceIt(
				MapController.Instance.GetMapTiles(), go.GetComponent<Tnt>());
			go.transform.parent = parent;
		}
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		for (int i=0; i<_Tnts.Count; i++) {
			if(_Tnts[i] == null)
				_Tnts.RemoveAt(i);
		}
	
		if (!_Used) {
			_Used = true;
			if(_Tnts.Count>0) {
				_Tnts[Random.Range(0,_Tnts.Count)].BeAttacked(this,1);
				Logger.strLog += "\nO ativador acionou um explosivo.";
			}
		}else
			Logger.strLog += "\nEste acionador já foi usado.";
	}

}
