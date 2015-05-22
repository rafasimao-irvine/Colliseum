using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatorManager : MonoBehaviour {

	List<ObjectStance> _Objs = new List<ObjectStance>();
	List<ObjectStance> _Enemies = new List<ObjectStance>();

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
		if (prefab.GetComponent<Characther>()!=null)
			_Enemies.Add(new ObjectStance(prefab,x,y));
		else
			_Objs.Add(new ObjectStance(prefab,x,y));
	}

	public MoldedLevel GetMoldedLevel () {

		MoldedLevel level = ScriptableObject.CreateInstance("MoldedLevel") as MoldedLevel;
		level.Objects = _Objs.ToArray();
		level.Enemies = _Enemies.ToArray();

		return level;
	}

}
