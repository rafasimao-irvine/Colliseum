using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatablesView : MonoBehaviour {
	

	public GameObject CreatableViewPrefab;

	public float OffsetX, OffsetY;
	public int PerRow;

	private CreatablesModel Creatables;
	private List<CreatableView> _Views;
	
	private Vector3 _InitialRect;
	private bool _Initialized = false;

	void Start () {
		_Views = new List<CreatableView>();
	}
	
	public void UpdateCreatables (CreatablesModel model) {
		FixModulesSize(model.Creatables.Length);
		ResetModules(model.Creatables);
	}
	
	private void FixModulesSize (int n) {
		if (_Views.Count < n) {
			for (int i=_Views.Count; i<n; i++)
				_Views.Add(GeneralFabric.CreateUIObject<CreatableView>(CreatableViewPrefab, transform));
		}
		else if (_Views.Count > n) {
			int dif = _Views.Count-n;
			for (int i =0; i<dif; i++) {
				Destroy(_Views[i].gameObject);
			}
			_Views.RemoveRange(0,dif);
		}
	}
	
	private void ResetModules (Creatable[] creatables) {
		float left=0f, top=0f;
		for (int i=0; i<_Views.Count && i<creatables.Length; i++) {
			_Views[i].Reset(creatables[i]);
			ChangePosition((_Views[i].transform as RectTransform),left,top);
			if ((i+1)%PerRow == 0) {
				left=0f;
				top+=OffsetY;
			}else{
				left+=OffsetX; 
			}
		}
	}
	
	private void ChangePosition (RectTransform modRect, float left, float top) {
		if (!_Initialized) {
			_InitialRect = modRect.localPosition;
			_Initialized = true;
		}
		
		modRect.localPosition = new Vector3(
			_InitialRect.x+left, _InitialRect.y-top, _InitialRect.z);
	}

}
