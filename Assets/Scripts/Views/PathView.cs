using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathView : MonoBehaviour {

	public Material[] SelectedMaterials, TargetSelectedMaterials;
	// 0 - Move; 1 - Interact; 2 - Attack;

	private Tile _PreviousTarget;

	public void SelectPath (List<Tile> path, Tile target, bool selected) {
		if (_PreviousTarget!=null)
			_PreviousTarget.SelectTile(false,null);

		int index = GetPathType(target);
		if (path!=null)
			foreach (Tile t in path)
				if (t!=null) t.SelectTile(selected,SelectedMaterials[index]);

		if (target!=null)
			target.SelectTile(selected,TargetSelectedMaterials[index]);

		_PreviousTarget = target;
	}

	private int GetPathType (Tile target) {
		int result = 0;
		if (target!=null) {
			if (target.OnTop!=null) {
				if (target.OnTop.Interactable)
					result = 1;
				else if (target.OnTop.Attackable)
					result = 2;
				else 
					result = 0;
			}
			else
				result = 0;
		}

		return result;
	}
}
