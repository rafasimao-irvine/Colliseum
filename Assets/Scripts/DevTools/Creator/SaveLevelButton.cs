using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveLevelButton : MonoBehaviour {

	public InputField LevelName;

	public void SaveLevel () {
#if UNITY_EDITOR
		if (LevelName.text == "") LevelName.text = "Undefined";

		MoldedLevel level = CreatorManager.Instance.GetMoldedLevel();

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (
			"Assets/Levels/"+LevelName.text+".asset");
		AssetDatabase.CreateAsset (level, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
#endif
	}

}
