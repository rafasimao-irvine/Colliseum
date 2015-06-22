using UnityEngine;
using System.Collections;

[IntegrationTest.DynamicTest("CharactherTest")]
public class CharactherTest : MonoBehaviour {

	public GameObject TestCharGO, TestCharGO2;
	private StubCharacther _TestChar, _TestChar2;
	private Tile[,] _MapTiles;

	int _Testing;

	// Use this for initialization
	void Start () {
		// Create char
		_TestChar = TestCharGO.AddComponent<StubCharacther>() as StubCharacther;
		_TestChar2 = TestCharGO2.AddComponent<StubCharacther>() as StubCharacther;
		// Create map
		MapController.Instance.CreateMap();
		_MapTiles = MapController.Instance.GetMapTiles();
		// Place char
		PutIn(_TestChar,_MapTiles[5,5]);
		PutIn(_TestChar2,_MapTiles[7,6]);
		// Make char move
		_TestChar.TestMoveTo(_MapTiles[6,6]);
		_Testing =0;
	}

	private void PutIn (Interactive i, Tile tile) {
		i.transform.position = new Vector3(tile.transform.position.x,
		                                   i.transform.position.y,
		                                   tile.transform.position.z);
		tile.TryGetIn(i);
		i.RefreshMyTile();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_TestChar.IsMoving() && _Testing==0) {
			IntegrationTest.Assert(_TestChar.MyTile==_MapTiles[6,6]);
			IntegrationTest.Assert(_MapTiles[6,6].OnTop == _TestChar);
			IntegrationTest.Assert(_MapTiles[5,5].OnTop == null);

			_TestChar2.TestSetLife(2);
			_TestChar.TestAttack(_TestChar2);
			_Testing=1;
		}

		if (!_TestChar.IsInAction() && _Testing==1) {
			IntegrationTest.Assert(!_TestChar2.MakeAction()); // No actions
			IntegrationTest.Assert(!_TestChar2.IsDead());
			IntegrationTest.Assert(_TestChar2.TestGetLife() == 1);

			IntegrationTest.Assert(!_TestChar.IsBuffed());
			_TestChar.BeBuffered(1); // increased its damage
			IntegrationTest.Assert(_TestChar.IsBuffed());
			_TestChar.TestAttack(_TestChar2);
			_Testing = 2;
		}

		if (!_TestChar.IsInAction() && _Testing==2) {
			IntegrationTest.Assert(!_TestChar.IsBuffed());
			IntegrationTest.Assert(_TestChar2.TestGetLife() == -1);
			IntegrationTest.Assert(_TestChar2.IsDead());
			IntegrationTest.Assert(_TestChar2.MakeAction()); // Dead always return true
			
			IntegrationTest.Assert(!_TestChar.MakeAction()); // No actions
			_TestChar.BeTrapped(2);
			IntegrationTest.Assert(_TestChar2.MakeAction()); // Trapped returns true
			IntegrationTest.Assert(_TestChar2.MakeAction()); // Trapped returns true
			IntegrationTest.Assert(!_TestChar2.MakeAction()); // No actions, not trapped anymore

			IntegrationTest.Pass(gameObject);
		}
	}

	class StubCharacther : Characther {
		public void TestMoveTo (Tile tile) {
			AddMoveTo(tile);
		}

		protected override bool MakeTurnAction () {
			return true;
		}

		public void TestAttack (Interactive target) {
			Attack(target);
		}

		public int TestGetLife(){
			return _Life;
		}

		public void TestSetLife (int life){
			_Life = life;
		}

		public bool IsBuffed(){
			return true;
		}

		protected override void BecomeInvisible () {}
		protected override void BecomeVisible () {}
	}
}
