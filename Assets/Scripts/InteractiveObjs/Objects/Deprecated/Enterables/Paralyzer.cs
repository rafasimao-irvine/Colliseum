using UnityEngine;
using System.Collections;

public class Paralyzer : Enterable {

	Characther _CharInto;

	public override void BeAttacked (Interactive iObj, int damage) {
		base.BeAttacked (iObj, damage);
		if(_CharInto!=null)
			_CharInto.BeAttacked(iObj,damage);
	}

	public override bool BeEntered (Characther c) {
		if (_CharInto==null) {
			_CharInto = c;
			Blockable = Attackable = true;
			return true;
		}

		return false;
	}

	public override bool BeLeft (Characther c) {
		if (c==_CharInto) {
			if (c.IsDead()) {
				_CharInto = null;
				Blockable = Attackable = false;
			} else
				return false;
		}
		return true;
	}

	public override Interactive GetBeAttackedTarget () {
		return _CharInto;
	}

	public override void BeDestroyed () {
		MyTile.TryGetOut(this);
		if (_CharInto != null)
			MyTile.TryGetIn(_CharInto);
		Destroy(gameObject);
	}
}
