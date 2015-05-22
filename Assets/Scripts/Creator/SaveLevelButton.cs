using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class SaveLevelButton : MonoBehaviour {

	public InputField LevelName;

	public void SaveLevel () {
		if (LevelName.text == "") LevelName.text = "Undefined";

		MoldedLevel level = CreatorManager.Instance.GetMoldedLevel();

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (
			"Assets/Levels/"+LevelName.text+".asset");
		AssetDatabase.CreateAsset (level, assetPathAndName);
		
		AssetDatabase.SaveAssets ();

	}

}
