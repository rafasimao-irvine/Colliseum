using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SightView : MonoBehaviour {

	public Material FirstSightMat, SecondSightMat;

	List<Tile> _PreviousSight;

	public void ShowSingleSight (Tile origin, int range) {
		List<Tile> tiles = MapController.Instance.GetBlock(origin,range);
		for (int i=0; i<tiles.Count; i++)
			tiles[i].SelectTile(true,FirstSightMat);

		_PreviousSight = tiles;
	}

	public void ShowDoubleSight (Tile origin, int range1, int range2) {
		if (range2>range1)
			range2 = range1;

		MapController map = MapController.Instance;
		List<Tile> tiles = map.GetBlock(origin,range1);
		for (int i=0; i<tiles.Count; i++) {
			if (map.GetDistance(origin,tiles[i]) > range2)
				tiles[i].SelectTile(true,FirstSightMat);
			else
				tiles[i].SelectTile(true,SecondSightMat);
		}

		_PreviousSight = tiles;
	}

	public void ShutOffLastSight () {
		if (_PreviousSight != null) {
			for (int i=0; i<_PreviousSight.Count; i++)
				_PreviousSight[i].SelectTile(false, FirstSightMat);

			_PreviousSight = null;
		}
	}
}
