using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostRunEffect : GameEffect {

	[SerializeField]
	private float _GhostSpeed;

	protected override void DoEffect (Interactive origin, Interactive target) {
		// Do nothing
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		if (origin.MyTile != targetTile && 
		    (targetTile.OnTop == null || (targetTile.OnTop!=null && !targetTile.OnTop.Blockable)) ) {

			if (MapController.Instance.GetNeighbours(origin.MyTile, _EffectRange).Contains(targetTile)) {
				if (origin is Characther) 
					((Characther)origin).AddMovementFeat(new GhostRunFeat(targetTile, _GhostSpeed));
				if (origin is Personage) ((Personage)origin).InterruptActions();
			}
		}
	}

}
