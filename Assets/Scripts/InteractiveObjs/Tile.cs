using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IHeapItem<Tile> {

	// Number of tiles that exist in the game
	public static readonly int NTiles = 10;
	
	// Tiles types
	public static readonly int TEmpty	 = 0;
	public static readonly int TGrass1 	 = 1;
	public static readonly int TGrass2 	 = 2;
	public static readonly int TGrass3 	 = 3;
	public static readonly int TGrassBurn = 4;
	public static readonly int TGround    = 5;
	public static readonly int TGroundDry = 6;
	public static readonly int TLava	  = 7;
	public static readonly int TSand 	 = 8;
	public static readonly int TStones	 = 9;
	public static readonly int TWater	 = 10;

	// List of Materials
	public Material[] Materials;
	//public Material SelectedMaterial;

	// Tile position in map matrix
	public int X, Y; // TODO put only get
	protected int _Type; // tile type: grass, sand, etc

	// OnTop variables
	public Interactive OnTop ;//{ get; protected set; }
	//public int OnTopType { get; protected set; }

	// Used to make the path finding
	public int GCost;
	public int HCost;
	public Tile Parent;

	public int HeapIndex {get; set;} // TODO heap improvent

	public int FCost {
		get {
			return GCost + HCost;
		}
	}

	public int CompareTo (Tile tileToCompare) {
		int compare = FCost.CompareTo(tileToCompare.FCost);
		if(compare==0)
			compare = HCost.CompareTo(tileToCompare.HCost);
		return -compare;
	}
	//----------------------



	/**
	 * Try to be setted onTop of the tile. 
	 * If the setting ocurred returns true, returns false otherwise.
	 */
	public bool TryGetIn (Interactive onTop) {
		if (onTop==null)
			return false;
		if (OnTop != null) {
			if(onTop is Characther)
				return OnTop.BeEntered((Characther)onTop);
			else
				return false;
		}

		OnTop = onTop;
		//OnTopType = type;

		return true;
	}

	/**
	 * The Interactive object try to get out of the tile.
	 * Returns true if it got out, returns false otherwise.
	 */
	public bool TryGetOut (Interactive onTop) {
		// If top is leaving, than its ok
		if (onTop == OnTop) 
			ClearOnTop();
		// Otherwise ask the current on top
		else if(OnTop != null && onTop is Characther)
			return OnTop.BeLeft((Characther)onTop);

		return true;
	}

	/**
	 * Clean on top markers
	 */
	protected void ClearOnTop () {
		OnTop = null;
		//OnTopType = 0;
	}


	public void SelectTile (bool selected, Material SelectedMaterial) {
		if (selected && SelectedMaterial!=null)
			GetComponent<Renderer>().material = SelectedMaterial;
		else
			SetTileType(_Type);
	}

	/**
	 * Set the tile x and y position
	 */
	public void SetPosition (int x, int y) {
		X = x;
		Y = y;
	}

	/**
	 * Set its type and changes the current material
	 */
	public void SetTileType (int t) {
		if(t>0 && t<=Materials.Length) {
			_Type = t;
			GetComponent<Renderer>().material = Materials[t-1];
		}
		if(t == TWater)
			GetComponent<WaterSimple>().enabled = true;
	}

	/**
	 * Get the tile type
	 */
	public int GetTileType() {
		return _Type;
	}

}
