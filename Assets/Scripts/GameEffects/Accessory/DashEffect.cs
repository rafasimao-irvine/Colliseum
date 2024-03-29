﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Adds a DashFeat to the origin (if it is a Characther) towards a
 * target Tile in line without obstacles, using the targetTile as reference.
 * */
public class DashEffect : GameEffect {

	// Characther Speed Modifier
	[SerializeField]
	protected float _DashSpeed;

	// Number of tiles the Dash can tresspass
	[SerializeField]
	protected int _DashRange;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target.MyTile!=null)
			DoEffect(origin,target.MyTile);
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		if (origin.MyTile != targetTile) {
			Vector2 direction = MapController.Instance.GetDirection(origin.MyTile,targetTile);
			List<Tile> line = MapController.Instance.GetLine(origin.MyTile, direction, _DashRange);

			int selected = -2;
			for (int i=0; i<line.Count; i++) {
				if (line[i].OnTop != null) {
					selected = i-1;
					i = line.Count; // Break out of the loop
				} else if (line[i] == targetTile || i==line.Count-1) {
					selected = i;
					i = line.Count; // Break out of the loop
				}
			}

			if (selected >= 0 && origin != null) {
				if (origin is Characther) 
					((Characther)origin).AddMovementFeat(new DashFeat(line[selected], _DashSpeed));
				if (origin is Personage) ((Personage)origin).InterruptActions();
			}
		}
	}
}
