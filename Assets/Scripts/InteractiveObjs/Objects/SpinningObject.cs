using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

	[SerializeField]
	private Interactive _InteractiveObject;
	//[HideInInspector]
	public Vector2 FacingDirection;

	void Start () {
		FacingDirection = new Vector2(Random.Range(-1,2),Random.Range(-1,2));

		Tile nextTile = MapController.Instance.GetNextTile(_InteractiveObject.MyTile, FacingDirection);
		if(nextTile != null) {
			Quaternion rot =  Quaternion.LookRotation(nextTile.transform.position - transform.position);
			rot.x = rot.z = 0;
			transform.rotation = rot;
		}
	
	}
	
}
