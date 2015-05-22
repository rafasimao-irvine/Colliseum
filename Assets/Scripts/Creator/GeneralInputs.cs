using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GeneralInputs {


	public static bool GetClickHitInfo (Vector3 pos, out Vector3 hitPos, out Tile hitTile, int id) {
		Ray ray = Camera.main.ScreenPointToRay (pos);
		RaycastHit hit = new RaycastHit();
		
		if (!EventSystem.current.IsPointerOverGameObject(id)) { // Used to block ui clcks!
			if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Tile"))) {
				hitPos = hit.point;
				hitTile = hit.transform.GetComponent<Tile>();
				return true;
			}
		}
		
		hitPos = new Vector3();
		hitTile = null;
		return false;
	}
}
