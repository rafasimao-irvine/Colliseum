using UnityEngine;
using System.Collections;

public class CreatorEffect : GameEffect {

	[SerializeField]
	private GameObject _ObjectPrefab;

	protected override void DoEffect (Interactive origin, Interactive target) {
		// Do nothing
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		if (_ObjectPrefab!=null && targetTile!=null && targetTile.OnTop==null) {
			Interactive interactive = GeneralFabric.CreateObject<Interactive>(_ObjectPrefab,null);
			MapController.Instance.PlaceItAt(interactive, targetTile);
		}
	}

}
