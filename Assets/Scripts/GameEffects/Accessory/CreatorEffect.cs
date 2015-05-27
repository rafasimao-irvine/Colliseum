using UnityEngine;
using System.Collections;

/**
 * Instantiates the Interactive Prefab at the targetTile.
 * PS: Does nothing if the the target is an Interactive.
 * */
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
