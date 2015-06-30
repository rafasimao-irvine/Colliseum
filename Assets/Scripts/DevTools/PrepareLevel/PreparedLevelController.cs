using UnityEngine;
using System.Collections;

public class PreparedLevelController : MonoBehaviour {

	public CreatablesModel ObjectsModel, ItensModel, EnemiesModel;

	public Creatable[] Objects, Enemies;
	int ItensIndex;

	public static PreparedLevelController Instance {get; private set;}

	public bool SaveStats;

	void Awake () {
		if (Instance == null) {
			//If I am the first instance, make me the Singleton
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else {
			//If a Singleton already exists and you find another reference in scene, destroy it!
			if(this != Instance)
				Destroy(this.gameObject);
		}
	}

	void OnLevelWasLoaded (int level) {
		if (Instance != null && Instance == this)
			Instance.InitiateLevel();
	}

	private void InitiateLevel () {
		ObjectsController objectsControll = FindObjectOfType<ObjectsController>();
		EnemiesController enemiesControll = FindObjectOfType<EnemiesController>();

		if (objectsControll!=null && enemiesControll!=null) {
			objectsControll.Objects = Objects;
			enemiesControll.Enemies = Enemies;
		}
		else {
			CreatablesModel[] models = FindObjectsOfType<CreatablesModel>();

			if (models.Length == 3) {
				for (int i=0; i<3; i++)
					AssignModel(models[i]);
			}
		}
	}

	private void AssignModel (CreatablesModel model) {
		if (model.name.Equals("ObjectsButton")) {
			ObjectsModel = model;
			if (SaveStats)
				CopyToArray(model.Creatables, Objects, 0);
		}
		else if (model.name.Equals("ItensButton")){
			ItensModel = model;
			if (SaveStats)
				CopyToArray(model.Creatables, Objects, 0, ItensIndex);
		}
		else if (model.name.Equals("EnemiesButton")){
			EnemiesModel = model;
			if (SaveStats)
				model.Creatables = Enemies;
		}
	}


	public void SaveLevelAndPlay () {
		if (Objects.Length <= 0)
			Objects = new Creatable[ObjectsModel.Creatables.Length + ItensModel.Creatables.Length];
		CopyToArray(Objects, ObjectsModel.Creatables, 0);

		ItensIndex = ObjectsModel.Creatables.Length;
		CopyToArray(Objects, ItensModel.Creatables, ObjectsModel.Creatables.Length);

		if (Enemies.Length <= 0)
			Enemies = new Creatable[EnemiesModel.Creatables.Length];
		CopyToArray(Enemies, EnemiesModel.Creatables, 0);

		Application.LoadLevel("Battle");
	}

	private void CopyToArray (Creatable[] to, Creatable[] from, int toOffset, int fromOffset = 0) {
		for (int i=0; i<from.Length && i<to.Length; i++)
			to[i+toOffset] = from[i+fromOffset];
	}

}
