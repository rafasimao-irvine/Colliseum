using UnityEngine;
using System.Collections;

public abstract class Interactive : MonoBehaviour {
	
	public Tile MyTile { get; protected set; }
	
	public bool Blockable { get; protected set; }
	public bool Attackable { get; protected set; }
	public bool Interactable { get; protected set; }

	public bool Unpathable;

	// Get the tile that is below the interactive object and set it as it's tile
	public void RefreshMyTile () {
		RaycastHit hit;
		if (Physics.Raycast(transform.position + Vector3.up*4f, 
		                    -Vector3.up, out hit, 50f, LayerMask.GetMask("Tile"))) {
			Tile t = hit.transform.GetComponent<Tile>();
			if (t != null)
				MyTile = t;
		}
	}

	abstract public void BeAttacked (Interactive iObj, int damage);

	abstract public bool BeEntered (Characther c);

	abstract public bool BeLeft (Characther c);

	virtual public Interactive GetBeAttackedTarget () {
		return this;
	}

	// Self destruction
	virtual public void BeDestroyed () {
		MyTile.TryGetOut(this);
		Destroy(gameObject);
	}

}
