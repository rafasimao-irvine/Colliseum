using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectStance {

	public ObjectStance (GameObject prefab, int x, int y) {
		Prefab = prefab;
		X= x;
		Y= y;
	}

	public GameObject Prefab;
	public int X, Y;

}
