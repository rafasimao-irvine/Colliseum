using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapController : MonoBehaviour { 
	
	// Tiles
	public TileCreator MapTileCreator;

	// MapTiles
	protected Tile[,] _MapTiles;

	public static MapController Instance {get; private set;}

	void Awake () {
		// First we check if there are any other instances conflicting
		if(Instance != null && Instance != this)
			// If that is the case, we destroy other instances
			Destroy(gameObject);
		
		// Here we save our singleton instance
		Instance = this;
	}

	/**
	 * Create the map and put on the obstacles of the arena.
	 */
	public void CreateMap () {
		//_MapTiles= MapTileCreator.CreateHexagonTiles();
		//_MapTiles= MapTileCreator.CreatePrismTiles();
		_MapTiles= MapTileCreator.CreateFullTiles();
	}

	public Tile GetRandomFreeTile () {
		Tile tile = null;
		// Randomly finds a spot in the arena
		while (tile == null || tile.GetTileType()==0 || tile.OnTop != null) {
			tile = _MapTiles[Random.Range(0,_MapTiles.GetLength(0)),
			                Random.Range(0,_MapTiles.GetLength(1))];
		}
		return tile;
	}

	/**
	 * Place It in one of the map tiles.
	 * */
	public void PlaceIt (Tile[,] mapTiles, Interactive iObj) {
		PlaceItAt(iObj, GetRandomFreeTile());
	}

	public void PlaceItAt (Interactive iObj, Tile tile) {
		// Sets the iObj position
		iObj.transform.position = new Vector3(
			tile.transform.position.x,
			iObj.transform.position.y,
			tile.transform.position.z );
		
		// Sets the iObj on top of the tile spot
		tile.TryGetIn(iObj);
		// And tells it is on its top
		iObj.RefreshMyTile();
	}


	/**
	 * Finds the best path from one tile to another.
	 * return the list of tiles that represent the path.
	 */
	public List<Tile> FindPath (Tile origin, Tile target) {
		if(origin==null || target == null)
			return null;

		//target = ValidadeTarget(origin, target);

		// Set of nodes to be evaluated
		List<Tile> open = new List<Tile>();
		//Heap<Tile> open = new Heap<Tile>(_MapTiles.GetLength(0)*_MapTiles.GetLength(1));  
		HashSet<Tile> closed = new HashSet<Tile>(); // Set of nodes already evaluated
		// Initialize origin tile
		origin.GCost = 0;
		origin.HCost = GetDistance(origin,target);
		origin.Parent = null;
		open.Add(origin); // Add it to the open list to start the algorithm

		// While there is tiles to search, keep searching
		while (open.Count > 0) {
			// Get the closest tile to the target
			Tile current = GetNodeWithLowestFCost(open);
			open.Remove(current);
			//Tile current = open.RemoveFirst();
			closed.Add(current);

			if(current == target) // Path has been found
				return RetracePath(origin, target); // return the path

			foreach (Tile neighbour in GetNeighbours(current,1)) {
				// Go to next neighbour
				if((neighbour.OnTop!=null && neighbour.OnTop.Blockable) || closed.Contains(neighbour))
					continue;

				// If the new GCost is less than a previous one or neighbour is not in the open list
				int newMovementCostToNeighbour = current.GCost + GetDistance(current, neighbour);
				if (newMovementCostToNeighbour < neighbour.GCost || !open.Contains(neighbour)) {
					// Sets the tile elements
					neighbour.GCost = newMovementCostToNeighbour;
					neighbour.HCost = GetDistance(neighbour,target);
					neighbour.Parent = current;

					// Add it to the open list if needed
					if(!open.Contains(neighbour))
						open.Add(neighbour);
					//else
					//	open.UpdateItem(neighbour);
				}
			}

		}

		//if(closed!=null)
		return RetracePath(origin, GetNodeWithLowestFCost(closed.ToList()));
		//return new List<Tile>();
	}

	private Tile ValidadeTarget (Tile origin, Tile target) {
		Tile chosen = null;
		if (target.OnTop != null && target.OnTop.Blockable) {
			List<Tile> neighbours = GetNeighbours(target,1);
			for (int i=0; i<neighbours.Count; i++) {
				if (neighbours[i].OnTop == null || !neighbours[i].OnTop.Blockable) {
					if(chosen == null || GetDistance(origin, neighbours[i]) < GetDistance(origin, chosen))
						chosen = neighbours[i];
				}
			}
		}

		if(chosen==null)
			return target;
		else
			return chosen;
	}

	/**
	 * Get the node with the lowest FCost in the given list of nodes.
	 * return the lowest node.
	 */
	private Tile GetNodeWithLowestFCost(List<Tile> nodes) {
		Tile current = nodes[0];
		for (int i=1; i<nodes.Count; i++)
			if(nodes[i].FCost < current.FCost || 
			   (nodes[i].FCost==current.FCost && nodes[i].HCost < current.HCost))
				current = nodes[i];

		return current;
	}

	/**
	 * Retrace the path through the parents of the given node,
	 * create a list of tiles as the path and return it.
	 */
	private List<Tile> RetracePath (Tile startNode, Tile endNode) {
		List<Tile> path = new List<Tile>();

		Tile current = endNode;
		while (current != startNode) {
			path.Add(current);
			current = current.Parent;
		}
		path.Reverse();

		return path;
	}

	/**
	 * Get the distance between two tiles.
	 */
	public int GetDistance(Tile start, Tile end) {
		return GetCubeDistance(GetCubeCoordinates(start.X,start.Y), GetCubeCoordinates(end.X,end.Y));
		//return (int)(end.transform.position - start.transform.position).magnitude;
	}

	public Vector3 GetCubeCoordinates (int q, int r) {
		return new Vector3(q, -q-r, r);
	}

	public int GetCubeDistance (Vector3 a, Vector3 b) {
		return (int)((Mathf.Abs(a.x-b.x) + Mathf.Abs(a.y-b.y) + Mathf.Abs(a.z-b.z))/2);
	}

	//Get all the tiles neighbours of the referred tile
	public List<Tile> GetNeighbours (Tile tile, int range =1) {
		if(tile==null || range<1)
			return null;

		List<Tile> neighbours = new List<Tile>();

		for (int x=tile.X-range; x<tile.X+range+1; x++) {
			int iniY = (x<tile.X) ? tile.Y-range+(tile.X-x) : tile.Y-range;
			int endY = (x>tile.X) ? tile.Y+range+1-(x-tile.X) : tile.Y+range+1;
			for (int y=iniY; y<endY; y++) {
				if (IsItValidTileXY(x,y) && _MapTiles[x,y] != tile)
					neighbours.Add(_MapTiles[x,y]); // Add the neighbour to the list
			}
		}

		return neighbours;
	}

	// Get the full block
	public List<Tile> GetBlock (Tile tile, int range =1) {
		List<Tile> blocks = GetNeighbours(tile, range);
		blocks.Add(tile);
		return blocks;
	}

	// Get the three Front tiles
	public List<Tile> GetFrontArea (Tile origin, Vector2 direction) {
		if(origin==null)
			return null;

		direction = GetNormalizedVector(direction);

		List<Tile> front = new List<Tile>();

		AddTileWithDxDyToList(origin, front, (int)direction.x, (int)direction.y);

		if (direction.y == 0) {
			// Top tile
			AddTileWithDxDyToList(origin, front, GetRoundedToOne((int)direction.x-1), 1);
			// Low tile
			AddTileWithDxDyToList(origin, front, GetRoundedToOne((int)direction.x+1), -1);
		} 
		else {
			// Top tile
			int dx = (int)direction.x - 1;
			int dy = (int)direction.y;
			if (dx < -1 || (dx < 0 && dy < 0)) {
				dx = -1;
				dy = 0;
			}
			AddTileWithDxDyToList(origin, front, dx, dy);
			// Low tile
			dx = (int)direction.x + 1;
			dy = (int)direction.y;
			if (dx > 1 || (dx > 0 && dy > 0)) {
				dx = 1;
				dy = 0;
			}
			AddTileWithDxDyToList(origin, front, dx, dy);
		}

		return front;
	}

	// Sub-Routine
	private void AddTileWithDxDyToList (Tile origin, List<Tile> tiles, int dx, int dy) {
		int x = origin.X + dx;
		int y = origin.Y + dy;
		if (IsItValidTileXY(x, y) && _MapTiles[x,y]!=origin)
			tiles.Add(_MapTiles[x,y]);
	}

	// Get the flower of tiles following the direction
	public List<Tile> GetFlower (Tile origin, Vector2 direction) {
		List<Tile> flower = GetFrontArea (origin, direction);
		flower.Add(origin);
		return flower;
	}

	// Get the next tile following the given direction, if any is ther returns null
	public Tile GetNextTile (Tile origin, Vector2 direction) {
		if(origin==null)
			return null;

		Tile tile = null;

		direction = GetNormalizedVector(direction);

		int x = origin.X + (int)direction.x;
		int y = origin.Y + (int)direction.y;

		if( IsItValidTileXY(x,y) && _MapTiles[x,y]!=origin)
			tile = _MapTiles[x,y];

		return tile;
	}

	/**
	 * Get a range of tiles in the given direction beginning by the origin tile
	 */
	public List<Tile> GetLine (Tile origin, Vector2 direction, int range) {
		if(origin==null || range<1)
			return null;

		direction = GetNormalizedVector(direction);

		List<Tile> line = new List<Tile>();

		for (int i=1; i<=range; i++) {
			int x = origin.X + (int)direction.x*i;
			int y = origin.Y + (int)direction.y*i;
			if (IsItValidTileXY(x,y) && _MapTiles[x,y] != origin) 
				line.Add(_MapTiles[x,y]);
		}

		return line;
	}

	public Vector2 GetDirection (Tile origin, Tile destin) {
		Vector2 result = new Vector2();
		result.x = destin.X - origin.X;
		result.y = destin.Y - origin.Y;
		return result;
	}

	public Vector2 GetNormalizedVector (Vector2 vector) {
		vector.x = GetRoundedToOne((int)vector.x);
		vector.y = GetRoundedToOne((int)vector.y);

		if (vector.x==vector.y) vector.x = 0;
		return vector;
	}

	public int GetRoundedToOne (int result) {
		if(result>1) result=1;
		if(result<-1) result=-1;
		return result;
	}

	private bool IsItValidTileXY (int x, int y) {
		if ((x>-1 && x<_MapTiles.GetLength(0)) && 
		    (y>-1 && y<_MapTiles.GetLength(1)) && 
		    (_MapTiles[x,y] != null)) 
			return true;

		return false;
	}

	/**
	 * Returns the arena matrix.
	 */
	public Tile[,] GetMapTiles(){
		return _MapTiles;
	}

}