using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SightView : MonoBehaviour {

	public Material FirstSightMat, SecondSightMat;

	public void ShowSingleSight (Tile origin, int range, bool show) {
		List<Tile> tiles = MapController.Instance.GetBlock(origin,range);
		for (int i=0; i<tiles.Count; i++)
			tiles[i].SelectTile(show,FirstSightMat);
	}

	public void ShowDoubleSight (Tile origin, int range1, int range2, bool show) {
		if (range2>range1)
			range2 = range1;

		MapController map = MapController.Instance;
		List<Tile> tiles = map.GetBlock(origin,range1);
		for (int i=0; i<tiles.Count; i++) {
			if (map.GetDistance(origin,tiles[i]) > range2)
				tiles[i].SelectTile(show,FirstSightMat);
			else
				tiles[i].SelectTile(show,SecondSightMat);
		}
	}
}
