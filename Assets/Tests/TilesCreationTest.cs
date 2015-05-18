using UnityEngine;
using System.Collections;

[IntegrationTest.DynamicTestAttribute("TilesCreation Test")]
public class TilesCreationTest : MonoBehaviour {

	TileCreator _TileCreator;
	Tile[,] _MapTiles;

	public int xSize,zSize;
	public int RightNumberOfTiles;
	public int[] xNotGeneratedTiles;
	public int[] zNotGeneratedTiles;

	// Use this for initialization
	void Start () {
		_TileCreator = GetComponent<TileCreator>();
		_MapTiles = _TileCreator.CreateHexagonTiles();
		CreateHexagon_RightMatrixSize();
		CreateHexagon_RightNumberOfTiles();
		CreateHexagon_GeneratedRightTiles();
		IntegrationTest.Pass(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CreateHexagon_RightMatrixSize(){
		IntegrationTest.Assert(_MapTiles.GetLength(0)==xSize);
		IntegrationTest.Assert(_MapTiles.GetLength(1)==zSize);
	}

	private void CreateHexagon_RightNumberOfTiles(){
		int counter = 0;
		for (int x=0; x<_TileCreator.X_NTiles; x++) {
			for (int z=0; z<_TileCreator.Z_NTiles; z++) {
				if (_MapTiles[x,z] != null)
					counter++;
			}
		}

		IntegrationTest.Assert(counter == RightNumberOfTiles);
	}

	private void CreateHexagon_GeneratedRightTiles(){
		for(int i=0; i<xNotGeneratedTiles.Length; i++)
			IntegrationTest.Assert(_MapTiles[xNotGeneratedTiles[i],zNotGeneratedTiles[i]] == null);
	}
}
