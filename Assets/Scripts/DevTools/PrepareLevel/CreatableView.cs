using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreatableView : MonoBehaviour {

	public Text NameText;
	public Image CreatableImage;
	public Text QuantityText;

	public Creatable MyCreatable;

	public void Reset (Creatable creatable) {
		MyCreatable = creatable;

#if UNITY_EDITOR
		StopAllCoroutines();
		StartCoroutine(ResetModuleImage(creatable.Prefab));
#endif
		
		NameText.text = creatable.Prefab.name;
		UpdateQuantity ();
	}

#if UNITY_EDITOR
	private IEnumerator ResetModuleImage (GameObject prefab) {
		Texture2D preview = AssetPreview.GetAssetPreview(prefab);
		
		while (AssetPreview.IsLoadingAssetPreview(prefab.GetInstanceID()))
			yield return new WaitForSeconds(0.2f);
		
		CreatableImage.sprite = Sprite.Create (
			preview, new Rect(0,0,preview.width,preview.height), Vector2.zero);
	}
#endif
	
	public void IncreaseQuantity () {
		if (MyCreatable!=null) {
			MyCreatable.Quantity++;
			UpdateQuantity();
		}
	}
	
	public void DecreaseQuantity () {
		if (MyCreatable!=null) {
			MyCreatable.Quantity--;
			UpdateQuantity();
		}
	}

	private void UpdateQuantity () {
		QuantityText.text = ""+MyCreatable.Quantity;
	}

}
