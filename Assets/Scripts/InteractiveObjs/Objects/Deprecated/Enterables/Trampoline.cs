using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trampoline : Enterable {

	private int _ThrowingRange = 3;
	private Characther _CharInto = null;

	public override bool BeEntered (Characther c) {
		Tile targetTile = null;
		List<Tile> line = MapController.Instance.GetLine(
			MyTile,MapController.Instance.GetDirection(c.MyTile,MyTile),_ThrowingRange);

		for (int i=0; i<line.Count; i++) {
			// Is it a valid map tile?
			if (line[i]!=null && (line[i].OnTop==null || !line[i].OnTop.Blockable))
				targetTile = line[i];
			// If it is a Obstacle, stop searching
			else if(line[i]!=null && line[i].OnTop is Obstacle)
				i = line.Count;
		}

		if (targetTile != null && targetTile != MyTile) {
			c.AddMovementFeat(new JumpFeat(targetTile));
			if (c is Personage) ((Personage)c).InterruptActions();
			return true;
		}

		_CharInto = c;
		Blockable = Attackable = true;
		return true;
	}

	public override bool BeLeft (Characther c) {
		if (c ==_CharInto) {
			_CharInto = null;
			Blockable = Attackable = false;
		}
		return true;
	}

	public override void BeAttacked (Interactive i, int damage) {
		base.BeAttacked (i, damage);
		if (_CharInto != null)
			_CharInto.BeAttacked(i,damage);
	}

	public override Interactive GetBeAttackedTarget () {
		return _CharInto;
	}

}
