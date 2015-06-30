using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Module : MonoBehaviour {

	public GameObject Prefab;

	public Image _Image;

	private bool _IsSelected = false;

	public void Reset (GameObject prefab) {
		Prefab = prefab;

#if UNITY_EDITOR
		StartCoroutine(ResetModuleImage(prefab));
#endif

		GetComponentInChildren<Text>().text = Prefab.name;
	}

#if UNITY_EDITOR
	private IEnumerator ResetModuleImage (GameObject prefab) {
		Texture2D preview = AssetPreview.GetAssetPreview(prefab);

		while (AssetPreview.IsLoadingAssetPreview(prefab.GetInstanceID()))
			yield return new WaitForSeconds(0.2f);

		_Image.sprite = Sprite.Create (
			preview, new Rect(0,0,preview.width,preview.height), Vector2.zero);

	}
#endif

	public void SelectPrefab () {
		_Image.color = Color.yellow;
		_IsSelected = true;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && _IsSelected)
			AddPrefabToMap(Input.mousePosition);
	}

	public void AddPrefabToMap (Vector3 data) {
		if (_IsSelected) {
			_IsSelected = false;
			_Image.color = Color.white;

			Tile target = null;
			Vector3 hitPos = Vector3.zero;

			if(GeneralInputs.GetClickHitInfo(data, out hitPos, out target, -1)) {
				if (target.OnTop == null) {
					MapController.Instance.PlaceItAt(
						GeneralFabric.CreateObject<Interactive>(Prefab, null),target);

					CreatorManager.Instance.AddGameObject(Prefab, target.X, target.Y);
				} 
			}

		}
	}

}
