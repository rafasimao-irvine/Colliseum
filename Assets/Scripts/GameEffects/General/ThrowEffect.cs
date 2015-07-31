using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Throws a Throwable with the given origin and target.
 * */
public class ThrowEffect : GameEffect {

	[SerializeField]
	private GameObject _ThrowablePrefab;
	private List<Throwable> _Throwable;
	[SerializeField]
	private int _StockSize = 1;
	private int _Counter;

	void OnAwake () {
		_Counter = 0;
		CreateThrowable();
	}

	private void CreateThrowable () {
		_Throwable = new List<Throwable>();
		for (int i=0; i<_StockSize; i++)
			_Throwable.Add( GeneralFabric.CreateObject<Throwable>(_ThrowablePrefab, null) );
	}

	protected override void DoEffect (Interactive origin, Tile targetTile) {
		if (_Throwable==null)
			CreateThrowable();
		
		_Counter = ((_Counter+1)<_Throwable.Count) ? _Counter+1 : 0;
		
		_Throwable[_Counter].BeThrown(origin,targetTile);
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target!=null && target.MyTile!=null)
			DoEffect(origin,target.MyTile);
	}

}
