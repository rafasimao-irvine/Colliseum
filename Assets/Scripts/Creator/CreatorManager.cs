using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatorManager : MonoBehaviour {

	List<GameObject> _Objs = new List<GameObject>();
	List<int> _Xs = new List<int>(), _Ys= new List<int>();

	#region Singleton ------------------------------------------
	public static CreatorManager Instance {get; private set;}
	
	void Awake () {
		// First we check if there are any other instances conflicting
		if(Instance != null && Instance != this)
			// If that is the case, we destroy other instances
			Destroy(gameObject);
		
		// Here we save our singleton instance
		Instance = this;
	}
	#endregion

	void Start () {
		MapController.Instance.CreateMap();
	}

	public void AddGameObject (GameObject prefab, int x, int y) {
		_Objs.Add(prefab);
		_Xs.Add(x);
		_Ys.Add(y);
	}

	public MoldedLevel GetMoldedLevel () {

		MoldedLevel level = ScriptableObject.CreateInstance("MoldedLevel") as MoldedLevel;
		level.Objects = _Objs.ToArray();
		level.Xs = _Xs.ToArray();
		level.Ys = _Ys.ToArray();

		return level;
	}

}
