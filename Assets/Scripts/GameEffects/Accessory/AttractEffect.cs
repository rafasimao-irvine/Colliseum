using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractEffect : GameEffect {

	[SerializeField]
	protected bool _IsReverse;

	public enum AttractionCenter {Origin, Target}
	[SerializeField]
	protected AttractionCenter _Center;

	[SerializeField]
	protected bool _IsPresetDirection;
	protected Vector2 _Direction; // For Preset direction only

	public enum AttractionArea {Target, Around}
	[SerializeField]
	protected AttractionArea _Area;
	[SerializeField]
	protected int _AreaRadius; // For Around only

	void Start () {
		if (_IsPresetDirection)
			_Direction = new Vector2(Random.Range(-1,2),Random.Range(-1,2));
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		Tile center = null;
		switch (_Center) {
		case AttractionCenter.Origin:
			center = origin.MyTile;
			break;
		case AttractionCenter.Target:
			center = targetTile;
			break;
		}

		switch (_Area) {
		case AttractionArea.Target:
			AttractTarget(center,targetTile);
			break;
		case AttractionArea.Around:
			AttractAround(center);
			break;
		}
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		DoEffect(origin,target.MyTile);
	}

	private void AttractTarget (Tile center, Tile targetTile) {
		Attract(targetTile,GetDirection(targetTile,center));
	}

	private void AttractAround (Tile center) {
		List<Tile> neighbours = MapController.Instance.GetNeighbours(center, _AreaRadius);
		List<Tile> selectedTiles = new List<Tile>();

		for (int i=0; i<neighbours.Count; i++) {
			if (neighbours[i].OnTop!=null && neighbours[i].OnTop is Characther) {
				Vector2 direction = GetDirection(neighbours[i],center);
				Tile next = MapController.Instance.GetNextTile(neighbours[i], direction);
				if (!selectedTiles.Contains(next) && Attract(neighbours[i],direction))
					selectedTiles.Add(next);
			}
		}
	}

	private Vector2 GetDirection (Tile target, Tile center) {
		Vector2 direction = 
			(_IsPresetDirection) ? 
				_Direction : ((!_IsReverse) ?
				              MapController.Instance.GetDirection(target, center) :
				              MapController.Instance.GetDirection(center, target) );
		return direction;
	}

	private bool Attract (Tile targetTile, Vector2 direction) {
		// If there is a actual characther to be attracted
		if (targetTile.OnTop!=null &&
			targetTile.OnTop.GetBeAttackedTarget()!=null && 
		    targetTile.OnTop.GetBeAttackedTarget() is Characther) {
			// Get target
			Characther target = (Characther)targetTile.OnTop.GetBeAttackedTarget();
			// Get tile that will slide to
			Tile tile = MapController.Instance.GetNextTile(targetTile, direction);

			Debug.Log("Foi1 "+tile.X);

			// Add the slideFeat that will move the characther
			if (tile!=null && (tile.OnTop==null || (tile.OnTop!=null && !tile.OnTop.Blockable))) {
				target.AddMovementFeat(new SlideFeat(tile));
				if (target is Personage) ((Personage)target).InterruptActions();

				Debug.Log("Foi");

				return true;
			}
		}
		return false;
	}

}
