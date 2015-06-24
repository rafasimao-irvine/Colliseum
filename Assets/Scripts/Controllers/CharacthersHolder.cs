using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacthersHolder : MonoBehaviour {

	public EnemiesController EnemiesControll;
	public PlayerController PlayerControll;

	public List<Characther> Personages, Enemies, AllChars;

	#region Singleton ------------------------------------------
	public static CharacthersHolder Instance {get; private set;}
	
	void Awake () {
		// First we check if there are any other instances conflicting
		if(Instance != null && Instance != this)
			// If that is the case, we destroy other instances
			Destroy(gameObject);
		
		// Here we save our singleton instance
		Instance = this;
	}
	#endregion


	public void Initiate () {
		Personages = new List<Characther>();
		Enemies = new List<Characther>();

		AllocateChar(PlayerControll.PlayerPersonage);
		List<Enemy> enemies = EnemiesControll.GetEnemies();
		for (int i=0; i<enemies.Count; i++)
			AllocateChar(enemies[i]);
	}

	private void AllocateChar (Characther c) {
		switch (c.MyType) {
		case Characther.Types.None:
			break;
		case Characther.Types.Personage:
			Personages.Add(c);
			break;
		case Characther.Types.Enemy:
			Enemies.Add(c);
			break;
		}
		AllChars.Add(c);
	}

	private void RemoveChar (Characther c) {
		switch (c.MyType) {
		case Characther.Types.None:
			break;
		case Characther.Types.Personage:
			Personages.Remove(c);
			break;
		case Characther.Types.Enemy:
			Enemies.Remove(c);
			break;
		}
		AllChars.Remove(c);
	}

	public void AddChar (Characther c) {
		AllocateChar(c);
		if (c is Enemy)
			EnemiesControll.AddEnemy((Enemy)c);
	}

	public void ChangeCharType (Characther c, Characther.Types newType) {
		RemoveChar(c);
		c.MyType = newType;
		AllocateChar(c);
	}

	public void ChangeCharHuntType (Characther c, Characther.Types newHuntType) {
		c.HuntType = newHuntType;
	}

	public List<Characther> GetChars (Characther.Types type) {
		List<Characther> chars = new List<Characther>();

		switch (type) {
		case Characther.Types.None:
			break;
		case Characther.Types.Personage:
			chars = Personages;
			break;
		case Characther.Types.Enemy:
			chars = Enemies;
			break;
		}

		return chars;
	}

	public List<Characther> GetAllChars () {
		return AllChars;
	}

}
