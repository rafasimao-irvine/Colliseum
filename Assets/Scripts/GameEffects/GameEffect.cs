using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameEffect : MonoBehaviour {

	[SerializeField]
	protected VisualEffect _VisualEffect;

	public AudioClip SoundEffect;

	public enum EffectTarget {
		None, OriginNeighbours, Line, Target, TargetBlock
	}

	[SerializeField]
	protected EffectTarget _EffectTarget;
	[SerializeField]
	protected int _EffectRange;

	public void MakeEffect (Interactive origin, Interactive target) {
		PlayVisualEffect(origin,target.MyTile);
		PlaySoundEffect();

		// Make Effect
		switch (_EffectTarget) {
		case EffectTarget.OriginNeighbours:
			DoEffectToNeighbours(origin);
			break;
		case EffectTarget.Line:
			DoEffectToLine(origin, target.MyTile);
			break;
		case EffectTarget.Target:
			DoEffect(origin, target);
			break;
		case EffectTarget.TargetBlock:
			DoEffectToBlock(origin, target.MyTile);
			break;
		}
	}

	public void MakeEffect (Interactive origin, Tile targetTile) {
		PlayVisualEffect(origin,targetTile);
		PlaySoundEffect();
		
		// Make Effect
		switch (_EffectTarget) {
		case EffectTarget.OriginNeighbours:
			DoEffectToNeighbours(origin);
			break;
		case EffectTarget.Line:
			DoEffectToLine(origin, targetTile);
			break;
		case EffectTarget.Target:
			DoEffect(origin, targetTile);
			break;
		case EffectTarget.TargetBlock:
			DoEffectToBlock(origin, targetTile);
			break;
		}
	}

	protected void PlayVisualEffect (Interactive origin, Tile target) {
		// Visual Effect
		if (_VisualEffect != null)
			_VisualEffect.MakeEffect(origin, target);
	}

	protected void PlaySoundEffect () {
		// Sound Effect
		if (SoundEffect!=null && Sounds.Instance!=null)
			Sounds.Instance.PlaySoundEffect(SoundEffect);
	}


	protected void DoEffectToNeighbours (Interactive origin) {
		List<Tile> tiles = MapController.Instance.GetNeighbours(origin.MyTile, _EffectRange);
		DoEffectToManyTargets(origin,tiles);
	}

	protected void DoEffectToLine (Interactive origin, Tile targetTile) {
		List<Tile> tiles = MapController.Instance.GetLine(
			origin.MyTile, MapController.Instance.GetDirection(targetTile, origin.MyTile), _EffectRange);
		DoEffectToManyTargets(origin,tiles);
	}

	protected void DoEffectToBlock (Interactive origin, Tile targetTile) {
		List<Tile> tiles = MapController.Instance.GetBlock(targetTile, _EffectRange);
		DoEffectToManyTargets(origin,tiles);
	}

	protected void DoEffectToManyTargets (Interactive origin, List<Tile> tiles) {
		for (int i=0; i<tiles.Count; i++) {
			if (tiles[i].OnTop != null)
				DoEffect(origin, tiles[i].OnTop);
		}
	}

	abstract protected void DoEffect (Interactive origin, Interactive target);

	virtual protected void DoEffect (Interactive origin, Tile targetTile) {
		if (targetTile.OnTop != null)
			DoEffect(origin, targetTile.OnTop);
	}

}
