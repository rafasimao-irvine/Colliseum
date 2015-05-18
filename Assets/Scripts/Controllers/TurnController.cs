using UnityEngine;
using System.Collections;

public abstract class TurnController : MonoBehaviour {

	// Used to know if its currently playing
	protected bool _IsMyTurn = false;

	/**
	 * Get if its turn is running. 
	 */
	public bool GetIsMyTurn() {
		return _IsMyTurn;
	}

	/**
	 * Stats the controller turn.
	 * */
	public abstract void StartMyTurn();
}
