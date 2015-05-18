using UnityEngine;
using System.Collections;

public class GeneralFabric : MonoBehaviour {

	public static T CreateObject<T> (GameObject prefab, Transform parent) where T : MonoBehaviour {
		GameObject go = (GameObject)Instantiate(
			prefab, prefab.transform.position, prefab.transform.rotation);
		go.transform.parent = parent;
		return go.GetComponent<T>();
	}
}
