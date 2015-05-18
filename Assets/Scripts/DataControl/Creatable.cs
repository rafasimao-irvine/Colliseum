using UnityEngine;
using System.Collections;

[System.Serializable]
public class Creatable {
	public GameObject Prefab;
	[SerializeField]
	protected int Min, Max;

	public int RandQuantity {
		get {
			int q = Random.Range(Min,Max+1);
			return (q<0) ? 0 : q;
		}
	}
}
