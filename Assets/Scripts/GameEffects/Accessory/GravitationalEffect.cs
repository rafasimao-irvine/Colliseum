using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravitationalEffect : GameEffect {

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target.MyTile != null)
			DoEffect(origin, target.MyTile);
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {

		List<Tile> neighbours = MapController.Instance.GetNeighbours(targetTile,_EffectRange);
		//MapController.Instance.SortTilesByDistanceTo(neighbours, targetTile);

		List<Tile> selectedTiles = new List<Tile>();

		for (int i=0; i<neighbours.Count; i++) {
			if (neighbours[i].OnTop!=null && neighbours[i].OnTop is Characther) {
				Vector2 direction = MapController.Instance.GetDirection(neighbours[i],targetTile);
				Tile next = MapController.Instance.GetNextTile(neighbours[i], direction);

				if (next!=null && 
				    (next.OnTop==null || (next.OnTop!=null && !next.OnTop.Blockable)) &&
				    !selectedTiles.Contains(next) ) {
					((Characther)neighbours[i].OnTop).AddMovementFeat(new SlideFeat(next));
					if (neighbours[i].OnTop is Personage) 
						((Personage)neighbours[i].OnTop).InterruptActions();

					selectedTiles.Add(next);
				}
			}
		}

	}

}
