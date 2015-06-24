using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResurrectEffect : GameEffect {

	[SerializeField]
	protected Characther.Types NewType, NewHuntType;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target != null)
			DoEffect(origin,target.MyTile);
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		if (targetTile.OnTop == null || (targetTile.OnTop!=null && !targetTile.OnTop.Blockable)) {
			List<Characther> chars = CharacthersHolder.Instance.GetAllChars();
			for (int i=0; i<chars.Count; i++) {
				if (chars[i].IsDead() && chars[i].MyTile == targetTile) {
					chars[i].Resurrect();
					CharacthersHolder.Instance.ChangeCharHuntType(chars[i], NewHuntType);
					CharacthersHolder.Instance.ChangeCharType(chars[i], NewType);
					i = chars.Count;
				}
			}
		}
	}
}
