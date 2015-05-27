using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Attack the referenced ObjectCreator's created objects with a
 * damage of 1.
 * */
public class ActivateObjectEffect : GameEffect {

	[SerializeField]
	private ObjectCreator _ObjectCreator;

	protected override void DoEffect (Interactive origin, Interactive target) {
		if (_ObjectCreator!=null) {
			List<Interactive> objects = _ObjectCreator.GetObjects();
			if (objects.Count>0) {
				int element = Random.Range(0,objects.Count);
				if(objects[element]!=null)
					objects[element].BeAttacked(origin,1);
			}
		}
	}
}
