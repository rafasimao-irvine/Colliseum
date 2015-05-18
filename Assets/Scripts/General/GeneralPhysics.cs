using UnityEngine;
using System.Collections;

public class GeneralPhysics {

	public static void Throw (Rigidbody rigidbody, Vector3 pos) {
		if (rigidbody != null) {
			float vY = 10f;
			
			float distZ = pos.z- rigidbody.transform.position.z;
			float distX = pos.x- rigidbody.transform.position.x;
			
			float t = 2*(-vY/Physics.gravity.y);
			
			float vZ = distZ/t;
			float vX = distX/t;
			
			rigidbody.velocity = new Vector3(vX,vY,vZ);
		}
	}

}
