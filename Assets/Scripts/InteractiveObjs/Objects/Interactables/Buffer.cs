using UnityEngine;
using System.Collections;

public class Buffer : Interactable {

	//private int _NBuffTurs = 7;
	//private int _AtkRangeBuff = 0;
	private int _StrBuff = 1;

	private bool _Used = false;
	
	public override void BeAttacked (Interactive iObj, int damage) {
		if (!_Used && iObj is Characther) {
			((Characther)iObj).BeBuffered(_StrBuff);//,_AtkRangeBuff,_NBuffTurs);
			_Used = true;
		} else
			Logger.strLog += "\nEste buffer já foi utilizado.";
	}

}
