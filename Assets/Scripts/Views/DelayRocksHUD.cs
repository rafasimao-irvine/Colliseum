using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DelayRocksHUD : MonoBehaviour {

	public Sprite RockOff, RockOn;

	public GameObject _RockPrefab;

	[SerializeField]
	private int _IniXY, _NPerEdge, _BlockSize, _InnerSpace;

	private List<Image> _RocksImages;

	void Start () {
		_RocksImages = new List<Image>();
	}

	public void FixRocksSize (int n) {
		if (n > 0) {
			// If there is less than necessary
			if (_RocksImages.Count < n) {
				for (int i=_RocksImages.Count; i<n; i++)
					_RocksImages.Add(GeneralFabric.CreateUIObject<Image>(_RockPrefab, transform));
			}
			// if there is more than necessary
			else if (_RocksImages.Count > n) {
				int dif = _RocksImages.Count-n;
				for (int i=0; i<dif; i++) {
					Destroy(_RocksImages[i].gameObject);
				}
				_RocksImages.RemoveRange(0,dif);
			}

			ResetRocksPosition();
		}
	}

	private void ResetRocksPosition () {
		int column = -1;
		for (int i=0; i<_RocksImages.Count; i++) {
			int row = i%_NPerEdge;
			if (row == 0)
				column++;

			int space = _BlockSize+_InnerSpace;
			_RocksImages[i].rectTransform.anchoredPosition = 
				new Vector2 (_IniXY+column*space, -_IniXY-row*space);
		}
	}

	public void ActivateRocks (int n) {
		if (n>=0 && n <= _RocksImages.Count) {
			for (int i=0; i<_RocksImages.Count; i++) {
				if (i<n)
					_RocksImages[i].sprite = RockOn;
				else
					_RocksImages[i].sprite = RockOff;
			}
		}
	}

	public void ResetRocks () {
		for (int i=0; i<_RocksImages.Count; i++)
			_RocksImages[i].sprite = RockOff;
	}
}
