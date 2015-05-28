using UnityEngine;
using System.Collections;

public class AccessoryEffect : GameEffect {

	[SerializeField]
	private Accessory _Accessory;
	
	void Start () {
		if (_Accessory != null)
			_Accessory.MyGameObject = gameObject;
	}
	
	protected override void DoEffect (Interactive origin, Interactive target) {
		if (target is Personage) {
			Accessory a = ((Characther)target).TryToEquip(_Accessory);
			if (a!=_Accessory) {
				origin.MyTile.TryGetOut(origin);
				if (a!=null) {
					// w assumes this weapon position
					a.MyGameObject.transform.parent = transform.parent;
					a.MyGameObject.transform.position = 
						new Vector3(transform.position.x, 
						            a.MyGameObject.transform.position.y, 
						            transform.position.z);
					a.MyGameObject.GetComponent<MeshRenderer>().enabled = true;
					
					origin.MyTile.TryGetIn(a.MyGameObject.GetComponent<Interactive>());
					a.MyGameObject.GetComponent<Interactive>().RefreshMyTile();
				}
				
				// Disable this weapon and make it go under the personage
				transform.parent = target.transform;
				GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}
	
	public Accessory GetAccessory () {
		return _Accessory;
	}

}
