using UnityEngine;
using System.Collections;

public class PersonageMoveAttackTest : MonoBehaviour {
	
	public Personage TestPersonage;
	public Obstacle TestObstacle;
	public GameObject AttackableGO, EnterableGO;
	private StubAttackable TestAttackable;
	private StubEnterable TestEnterable;
	private Tile[,] _MapTiles;
	
	bool _MadeAction = false;
	int _Testing;
	
	// Use this for initialization
	void Start () {
		// Create map
		MapController.Instance.CreateMap();
		_MapTiles = MapController.Instance.GetMapTiles();
		// Place char
		PutIn(TestPersonage,_MapTiles[5,3]);
		
		// Create necessary objs
		TestAttackable = AttackableGO.AddComponent<StubAttackable>() as StubAttackable;
		TestEnterable = AttackableGO.AddComponent<StubEnterable>() as StubEnterable;
		
		// Put objs in
		PutIn(TestObstacle, _MapTiles[3,5]);
		PutIn(TestAttackable, _MapTiles[4,7]);
		PutIn(TestEnterable, _MapTiles[6,4]);
		
		// Start tests
		_Testing = 0;
		TestPersonage.PrepareActionsTarget(TestObstacle.MyTile);
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
		
		if (!_MadeAction && TestPersonage.MakeAction())
			_MadeAction = true;
		
		if (_MadeAction && !TestPersonage.IsInAction()) {
			_MadeAction = false;
			
			switch (_Testing) {
			case 0:
				AssertChangeTileInfos(TestPersonage, _MapTiles[4,4], _MapTiles[5,3]);
				break;
			case 2:
				AssertChangeTileInfos(TestPersonage, _MapTiles[4,5], _MapTiles[4,4]);
				break;
			case 3:
				AssertChangeTileInfos(TestPersonage, _MapTiles[4,6], _MapTiles[4,5]);
				IntegrationTest.Assert(!TestAttackable.WasAttacked);
				break;
			case 4:
				IntegrationTest.Assert(TestPersonage.MyTile == _MapTiles[4,6]);
				IntegrationTest.Assert(TestAttackable.WasAttacked);
				break;
			case 6:
				AssertChangeTileInfos(TestPersonage, _MapTiles[5,5], _MapTiles[4,6]);
				break;
			case 7:
				IntegrationTest.Assert(_MapTiles[5,5] == null);
				IntegrationTest.Assert(TestPersonage.MyTile == _MapTiles[6,4]);
				IntegrationTest.Assert(TestEnterable.OnTop == TestPersonage);
				break;
			case 8:
				IntegrationTest.Assert(TestEnterable.OnTop == null);
				IntegrationTest.Assert(TestPersonage.MyTile == _MapTiles[7,3]);
				IntegrationTest.Assert(_MapTiles[7,3].OnTop == TestPersonage);
				break;
			}
			_Testing++;
		}
		
		if(_Testing==1) {
			IntegrationTest.Assert(!TestPersonage.MakeAction());
			TestPersonage.PrepareActionsTarget(TestAttackable.MyTile);
			_Testing++;
		}
		if (_Testing==5) {
			IntegrationTest.Assert(!TestPersonage.MakeAction());
			TestPersonage.PrepareActionsTarget(_MapTiles[7,3]);
			_Testing++;
		}
		if (_Testing==9) {
			IntegrationTest.Assert(!TestPersonage.MakeAction());
			TestPersonage.PrepareWaitAction();
			IntegrationTest.Assert(TestPersonage.MakeAction());
			IntegrationTest.Pass(gameObject);
		}
		
	}
	
	private void AssertChangeTileInfos (Characther c, Tile tCurrent, Tile tPrevious) {
		IntegrationTest.Assert(c.MyTile == tCurrent);
		IntegrationTest.Assert(tCurrent.OnTop == c);
		IntegrationTest.Assert(tPrevious.OnTop == null);
	}
	
	class StubAttackable: Attackable {
		public bool WasAttacked = false;
		
		public override void BeAttacked (Interactive iObj, int damage)
		{
			WasAttacked = true;
		}
	}
	
	class StubEnterable : Enterable {
		
		public Characther OnTop;
		
		public override bool BeEntered (Characther c)
		{
			OnTop = c;
			return true;
		}
		
		public override bool BeLeft (Characther c)
		{
			if(c==OnTop)
				OnTop = null;
			return true;
		}
	}
}

