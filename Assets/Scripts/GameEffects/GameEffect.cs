using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameEffect : MonoBehaviour {

	[SerializeField]
	protected VisualEffect _VisualEffect;

	public AudioClip SoundEffect;

	public enum EffectTarget {
		None, Neighbours, Line, Target
	}

	[SerializeField]
	protected EffectTarget _EffectTarget;
	[SerializeField]
	protected int _EffectRange;

	public void MakeEffect (Interactive origin, Interactive target) {
		// Visual Effect
		if (_VisualEffect != null)
			_VisualEffect.MakeEffect(origin, target);
		// Sound Effect
		if (SoundEffect!=null && Sounds.Instance!=null)
			Sounds.Instance.PlaySoundEffect(SoundEffect);

		// Make Effect
		switch (_EffectTarget) {
		case EffectTarget.Neighbours:
			DoEffectToNeighbours(origin);
			break;
		case EffectTarget.Line:
			DoEffectToLine(origin, target);
			break;
		case EffectTarget.Target:
			DoEffect(origin, target);
			break;
		}
	}


	protected void DoEffectToNeighbours (Interactive origin) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(origin.MyTile, _EffectRange);
		DoEffectToManyTargets(origin,tiles);
	}

	protected void DoEffectToLine (Interactive origin, Interactive target) {
		List<Tile> tiles = MapController.Instance.GetLine(
			origin.MyTile, MapController.Instance.GetDirection(target.MyTile,origin.MyTile), _EffectRange);
		DoEffectToManyTargets(origin,tiles);
	}

	protected void DoEffectToManyTargets (Interactive origin, List<Tile> tiles) {
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop != null)
				DoEffect(origin, tiles[i].OnTop);
		}
	}

	abstract protected void DoEffect (Interactive origin, Interactive target);

}
