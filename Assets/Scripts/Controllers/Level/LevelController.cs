using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public WinLoseMenu WinLoseScreen;

	[SerializeField]
	private int _CurrentLevel = 0;
	[SerializeField]
	private int _CurrentBranch = 0;

	[SerializeField]
	private LevelBranch[] _Branches;

	private EnemiesController _EnemiesController;
	private Personage _Personage;
	
	private bool _GameEnded;

	public static LevelController Instance {get; private set;}

	void Awake () {
		if(Instance == null) {
			//If I am the first instance, make me the Singleton
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else {
			//If a Singleton already exists and you find another reference in scene, destroy it!
			if(this != Instance)
				Destroy(this.gameObject);
		}

		Instance.InitiateLevel();
	}

	private void InitiateLevel () {
		_GameEnded = false;

		_Branches[_CurrentBranch].PrepareLevel(_CurrentLevel);

		GameController gameController = FindObjectOfType<GameController>();

		gameController.GCMapController.MapTileCreator.TilesTypes = _Branches[_CurrentBranch].TilesTypes;
		
		InitiatePersonage(gameController.GCPlayerController);
		
		_EnemiesController = gameController.GCEnemiesController;
		//_EnemiesController.Enemies = _Levels[_CurrentLevel].Enemies;
		_EnemiesController.Enemies = _Branches[_CurrentBranch].Enemies;
		
		//gameController.GCObjectsController.Objects = _Levels[_CurrentLevel].Objects;
		gameController.GCObjectsController.Objects = _Branches[_CurrentBranch].Objects;
	}

	private void InitiatePersonage (PlayerController playerController) {
		if(_Personage==null) {
			_Personage = playerController.PlayerPersonage;
			_Personage.transform.parent = transform;
		}
		else {
			// Get HUD's
			WeaponsHUD weaponsHUD = playerController.PlayerPersonage.CharWeaponsHUD;
			AccessoriesHUD accessoriesHUD = playerController.PlayerPersonage.CharAccessoriesHUD;
			// Destroy current PlayerPersonage
			Destroy(playerController.PlayerPersonage.gameObject);
			// PlayerController update
			playerController.PlayerPersonage = _Personage;
			FindObjectOfType<Logger>().LogPersonage = _Personage;
			// Weapons HUD update
			_Personage.SetCharWeaponsHUD(weaponsHUD);
			//weaponsHUD.WeaponsChar = _Personage;
			weaponsHUD.UpdateWeapons();
			// Accessories HUD update
			_Personage.SetCharAccessoriesHUD(accessoriesHUD);
			accessoriesHUD.AccessoriesChar = _Personage;
			accessoriesHUD.UpdateAccessories();
		}
	}

	void Update () {
		bool isEnemiesDead = true;
		for (int i=0; i<_EnemiesController.GetEnemies().Count; i++) {
			if(_EnemiesController.GetEnemies()[i].GetLife() > 0)
				isEnemiesDead = false;
		}
		if (!_GameEnded && (isEnemiesDead || (_Personage!=null && _Personage.GetLife() < 1)) ) {
			_GameEnded = true;
			EndLevel(_Personage.GetLife() > 0);
		}
	}

	private void EndLevel (bool passed) {
		FindObjectOfType<PlayerController>().enabled = false;
		_Personage.InterruptActions();

		WinLoseScreen.Activate(passed);

	}

	public void RestartLevel () {
		_CurrentBranch = 0;
		_CurrentLevel = 1;
		if (_Personage != null) {
			_Personage.transform.parent = transform.parent;
			_Personage = null;
		}

		LoadBattle();
	}

	public void NextLevel () {
		_CurrentLevel++;
		if (_CurrentLevel > _Branches[_CurrentBranch].MaxLevel)
			_CurrentBranch += (_CurrentBranch<(_Branches.Length-1)) ? 1 : 0;

		LoadBattle();
	}

	private void LoadBattle() {
		Application.LoadLevel("Battle");
	}

	public int GetLevel () {
		return _CurrentLevel;
	}
	
}
