﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Adds a JumpFeat to the target or a previous tile in case 
 * there is a obstacle in the way, respecting the throwing range.
 * */
public class JumpEffect : GameEffect {

	[SerializeField]
	private int _ThrowingRange;

	protected override void DoEffect (Interactive origin, Interactive target) {
		Tile targetTile = null;
		List<Tile> line = MapController.Instance.GetLine(
			origin.MyTile,MapController.Instance.GetDirection(target.MyTile,origin.MyTile),_ThrowingRange);
		
		for (int i=0; i<line.Count; i++) {
			// Is it a valid map tile?
			if (line[i]!=null && (line[i].OnTop==null || !line[i].OnTop.Blockable))
				targetTile = line[i];
			// If it is a Obstacle, stop searching
			else if(line[i]!=null && line[i].OnTop is Obstacle)
				i = line.Count;
		}
		
		if (targetTile != null && targetTile != origin.MyTile) {
			if (target is Characther) ((Characther)target).AddMovementFeat(new JumpFeat(targetTile));
			if (target is Personage) ((Personage)target).InterruptActions();
		}
	}
}
