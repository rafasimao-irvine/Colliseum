using UnityEngine;
using System.Collections;

public class TileCreator : MonoBehaviour {

	public const int GEN_RANDOM = 0;
	public const int GEN_INARRAY = 1;
	public const int GEN_MANUAL = 2;

	public int Z_NTiles, X_NTiles;
	public int Tiles_Size;
	public GameObject TilePrefab;

	public int GenType;

	public int[] TilesTypes;

	private int[] ManualEntries = new int[] {
		      1,1,1,3,3,3,3,//1º fila
		     1,10,10,1,2,2,3,3,//2º fila
		    1,10,10,10,2,2,2,3,3,//3º fila
		   2,10,10,10,2,2,2,2,2,2,//4º fila
		  2,1,10,10,3,3,3,1,1,1,1,//5º fila
		 2,2,1,2,2,3,3,3,3,1,1,1,//6º fila
		2,1,1,1,1,3,3,3,3,1,1,1,1,//7º fila
		 2,2,1,3,3,3,3,1,2,2,1,1,//8º fila
		  2,2,2,3,3,5,5,1,2,1,1,//9º fila
		   2,2,3,2,5,5,10,10,10,3,//10º fila
		    2,3,3,2,5,10,10,10,3,//11º fila
		     1,1,3,5,5,5,10,10,//12º fila
		      1,1,1,5,5,10,10,//13º fila
	};

	private int _Entry = -1;

	private Tile[,] _MapTiles;
	private float InitialX, InitialZ; // x,z 0,0 coordinates

	private int[] _FullIniZ = new int[] {
		17,16,15,14,14,13,12,11,10,9,9,8,7,6,5,5,4,3,2,1,1,0,2,6,10
	};

	private int[] _FullEndZ = new int[] {
		19,23,27,29,28,27,27,26,25,24,23,23,22,21,20,19,19,18,17,16,15,15,14,13,12
	};

	void Awake () {
		_MapTiles = new Tile[X_NTiles,Z_NTiles];
		// calculate initial coordination begin
		InitialX = transform.position.x - X_NTiles/2 * Tiles_Size - Z_NTiles/2 * Tiles_Size*0.5f;
		InitialZ = transform.position.z - Z_NTiles/2 * Tiles_Size*0.75f;
	}

	/**
	 * Create a square arena
	 * */
	public Tile[,] CreatePrismTiles () {
		// loops through the x & z parts of the matrix
		for(int x=0; x<X_NTiles; x++) {
			for(int z=0; z<Z_NTiles; z++) {
				CreateTile(x, z);// create tile obj
			}
		}

		return _MapTiles;
	}

	/**
	 * Create a hexagon arena
	 * */
	public Tile[,] CreateHexagonTiles () {

		int zMid = Z_NTiles/2;

		// Creates the upward part of the arena
		int xMax = X_NTiles-(Z_NTiles-1-zMid);
		for (int z=Z_NTiles-1; z>zMid; z--){
			for (int x=0; x<xMax; x++)
				CreateTile(x,z);
			xMax++;
		}

		// Creates the X center of the arena
		for(int x=0; x<X_NTiles; x++)
			CreateTile(x,zMid);

		// Creates the downward part of the arena
		int xIni = 1;
		for (int z=zMid-1; z>-1; z--){
			for (int x=xIni; x<X_NTiles; x++)
				CreateTile(x,z);
			xIni++;
		}

		return _MapTiles;
	}

	public Tile[,] CreateSquareTiles () {
		for (int x=0; x<X_NTiles; x++) {
			for (int z=-x; z<Z_NTiles-x; z++) {
				CreateTile(x,z);
			}
		}

		return _MapTiles;
	}

	public Tile[,] CreateFullTiles () {
		X_NTiles = 25;
		Z_NTiles = 29;
		_MapTiles = new Tile[X_NTiles,Z_NTiles];

		for (int x=0; x<X_NTiles; x++) {
			int iniZ = _FullIniZ[x];
			int endZ = _FullEndZ[x];
			for (int z=iniZ; z<endZ; z++) {
				CreateTile(x,z);
			}
		}
		return _MapTiles;
	}

	/**
	 * Finds the x and z coordinates.
	 * Returns a Vector2, so the y factor is the z coordinate.
	 * */
	private Vector2 CalculateTileXZ (int x, int z) {
		Vector2 result = new Vector2();

		result.x = InitialX+Tiles_Size*x + Tiles_Size*0.5f*z;
		result.y = InitialZ+Tiles_Size*0.75f*z;// y is the z coordinate

		return result;
	}

	/**
	 * Creates a tile in the named position
	 * */
	private void CreateTile(int x, int z) {
		// Returns without creating in case it is out of bound
		if(x<0 || z<0 || x>=_MapTiles.GetLength(0) || z>=_MapTiles.GetLength(1))
			return;

		// Calculate the position
		Vector2 tileXZ = CalculateTileXZ(x,z);

		// Create it and save in the matrix
		_MapTiles[x,z] = 
			((GameObject)Instantiate(TilePrefab, 
			                         new Vector3(tileXZ.x, transform.position.y, tileXZ.y), 
			                         transform.rotation)).GetComponent<Tile>();
		_MapTiles[x,z].SetTileType(GenerateTile(GenType,x,z));
		_MapTiles[x,z].SetPosition(x,z);
		_MapTiles[x,z].transform.parent = transform;
	}

	/**
	 * Generates the tile type based in the generation type
	 * */
	public int GenerateTile (int genType, int x, int z) {
		int result = 0;
		switch(genType) {
		// Random
		case GEN_RANDOM:
			result = Random.Range(1,Tile.NTiles+1);
			break;
		// Random in array types
		case GEN_INARRAY:
			result = TilesTypes[Random.Range(0,TilesTypes.Length)];
			break;
		// Get the next tile inside the manual entries
		case GEN_MANUAL:
			result = NextManualEntry();
			break;
		}

		return result;
	}

	private int NextManualEntry () {
		return (++_Entry < ManualEntries.Length) ? ManualEntries[_Entry] : 1;
	}

	public int[] GetManualEntries () {
		return ManualEntries;
	}

}
