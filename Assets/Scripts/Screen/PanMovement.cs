using UnityEngine;
using System.Collections;

public class PanMovement : MonoBehaviour {

	public Vector2 horizontalClamp, verticalClamp;
	public float panSpeed = 3f;
	public Camera panCam;

	void Update () {
		if (Input.touchCount == 1) {
			Touch t = Input.GetTouch(0);

			Vector3 pos = transform.position;

			Vector3 point = panCam.ScreenToWorldPoint(t.position);
			Vector3 previousPoint = panCam.ScreenToWorldPoint(t.position+t.deltaPosition);

			Vector3 vector = point-previousPoint;
			vector.y=0f;

			pos += vector*panSpeed;

			float horCamSize = panCam.orthographicSize * (Screen.width/Screen.height);
			pos.x = Mathf.Clamp(pos.x, horizontalClamp.x+horCamSize, horizontalClamp.y-horCamSize);
			pos.z = Mathf.Clamp(pos.z, horizontalClamp.x+panCam.orthographicSize, 
			                    horizontalClamp.y-panCam.orthographicSize);

			transform.position = pos;
		}
	}

}
