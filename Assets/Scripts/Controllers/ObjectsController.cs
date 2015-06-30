using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsController : TurnController {

	// Objects
	public Creatable[] Objects;

	private List<Interactive> _Objects;
	private List<MicroturnObject> _MicroturnObjects;

	void Awake () {
		_Objects = new List<Interactive>();
		_MicroturnObjects = new List<MicroturnObject>();
	}

	public void CreateObjects () {
		for (int i=0; i<Objects.Length; i++){
			int quantity = Objects[i].RandQuantity;
			for (int q=0; q<quantity; q++){
				_Objects.Add(GeneralFabric.CreateObject<Interactive>(Objects[i].Prefab, transform));

				MapController.Instance.PlaceIt(
					MapController.Instance.GetMapTiles(), _Objects[_Objects.Count-1]);
			}
		}
	}

	public void LoadObjects (ObjectStance[] objsStances) {
		for (int i=0; i<objsStances.Length; i++) {
			_Objects.Add(GeneralFabric.CreateObject<Interactive>(objsStances[i].Prefab, transform));

			MapController.Instance.PlaceItAt(
				_Objects[_Objects.Count-1], objsStances[i].X, objsStances[i].Y);
		}
	}

	#region implemented abstract members of TurnController
	public override void StartMyTurn () {
		Publish();

		_IsMyTurn = false;
	}
	#endregion

	private void Publish () {
		for (int i=0; i<_MicroturnObjects.Count; i++)
			_MicroturnObjects[i].OnTurnChange();
	}

	public void Subscribe (MicroturnObject obj) {
		_MicroturnObjects.Add(obj);
	}

	public void Unsubscribe (MicroturnObject obj) {
		_MicroturnObjects.Remove(obj);
	}
}
