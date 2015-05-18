using UnityEngine;
using System.Collections;

/**
 * Used to adjust the unity GUI to the screen
 */
public class ScreenAdjuster : MonoBehaviour {
	
	// Original reolution used to create the GUI's
	public static float originalWidth = 1024f, originalHeight = 768f;
	
	// Screen scales
	private static Vector3 scale = new Vector3(Screen.width/originalWidth,Screen.height/originalHeight,1f);
	private static Vector3 sameScale = new Vector3((scale.y < scale.x) ? scale.y : scale.x,
	                                              (scale.x < scale.y) ? scale.x : scale.y, 1f);
	private static Vector3 centerSameScale = 
		new Vector3((scale.x*originalWidth - sameScale.x*originalWidth)/2f,0f,0f);
	
	
	/** 
	 * Adjust the GUI screen stretching it to the actual resolution
	 */
	public static void AdjustScreen() {
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);		
	}
	
	/**
	 * Adjust the GUI to the screen without stretching it,
	 * keeps the adjusted gui in the middle of the screen
	 */ 
	public static void AdjustScreenSameScale() {
		GUI.matrix = Matrix4x4.TRS(centerSameScale, Quaternion.identity, sameScale);
	}
}
