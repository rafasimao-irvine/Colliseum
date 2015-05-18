using UnityEngine;
using System.Collections;

public class SlideEffect : GameEffect {

	[SerializeField]
	private bool _IsRandomDefinedDirection;
	private Vector2 _Direction;

	[SerializeField]
	private bool _Knockback;

	void Start () {
		if (_IsRandomDefinedDirection)
			_Direction = new Vector2(Random.Range(-1,2),Random.Range(-1,2));
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (!_IsRandomDefinedDirection)
			_Direction = (_Knockback) ? 
				MapController.Instance.GetDirection(origin.MyTile, target.MyTile) :
				MapController.Instance.GetDirection(target.MyTile, origin.MyTile);

		Tile tile = (_Knockback) ? 
			MapController.Instance.GetNextTile(target.MyTile,_Direction) :
				MapController.Instance.GetNextTile(origin.MyTile,_Direction);

		if (tile!=null && (tile.OnTop==null || (tile.OnTop!=null && !tile.OnTop.Blockable))) {
			if (target is Characther) ((Characther)target).AddMovementFeat(new SlideFeat(tile));
			if (target is Personage) ((Personage)target).InterruptActions();
		}
	}

}
