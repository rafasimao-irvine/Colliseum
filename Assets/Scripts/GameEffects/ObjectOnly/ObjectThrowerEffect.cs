using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectThrowerEffect : GameEffect {

	[SerializeField]
	private GameObject _Prefab;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (_Prefab==null) return;

		Vector2 direction = MapController.Instance.GetDirection(target.MyTile, origin.MyTile);
		List<Tile> tiles = MapController.Instance.GetFrontArea(origin.MyTile, direction);
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop==null) {
				Interactive interactive = GeneralFabric.CreateObject<Interactive>(_Prefab,transform.parent);
				MapController.Instance.PlaceItAt(interactive, tiles[i]);
			}
		}
	}

}
