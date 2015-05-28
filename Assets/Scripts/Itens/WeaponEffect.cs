using UnityEngine;
using System.Collections;

public class WeaponEffect : GameEffect {

	[SerializeField]
	private Weapon _Weapon;

	void Start () {
		if (_Weapon != null)
			_Weapon.MyGameObject = gameObject;
	}

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Personage) {
			Weapon w = ((Characther)target).TryToEquip(_Weapon);
			if (w!=_Weapon) {
				origin.MyTile.TryGetOut(origin);
				if (w!=null) {
					// w assumes this weapon position
					w.MyGameObject.transform.parent = transform.parent;
					w.MyGameObject.transform.position = 
						new Vector3(transform.position.x, 
					    	        w.MyGameObject.transform.position.y, 
					        	    transform.position.z);
					w.MyGameObject.GetComponent<MeshRenderer>().enabled = true;

					origin.MyTile.TryGetIn(w.MyGameObject.GetComponent<Interactive>());
					w.MyGameObject.GetComponent<Interactive>().RefreshMyTile();
				}

				// Disable this weapon and make it go under the personage
				transform.parent = target.transform;
				GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	public Weapon GetWeapon () {
		return _Weapon;
	}
}
