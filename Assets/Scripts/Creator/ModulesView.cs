using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulesView : MonoBehaviour {

	public GameObject Module;

	public float OffsetX, OffsetY;
	public int PerRow;

	private List<Module> _Modules;

	private Vector3 _InitialRect;
	private bool _Initialized = false;

	void Start () {
		_Modules = new List<Module>();
	}

	public void UpdateModules (ModulesModel model) {
		FixModulesSize(model.Prefabs.Length);
		ResetModules(model.Prefabs);
	}

	private void FixModulesSize (int n) {
		if (_Modules.Count < n) {
			for (int i=_Modules.Count; i<n; i++)
				_Modules.Add(GeneralFabric.CreateUIObject<Module>(Module, transform));
		}
		else if (_Modules.Count > n) {
			int dif = _Modules.Count-n;
			for (int i =0; i<dif; i++) {
				Destroy(_Modules[i].gameObject);
			}
			_Modules.RemoveRange(0,dif);
		}
	}

	private void ResetModules (GameObject[] prefabs) {
		float left=0f, top=0f;
		for (int i=0; i<_Modules.Count && i<prefabs.Length; i++) {
			_Modules[i].Reset(prefabs[i]);
			ChangePosition((_Modules[i].transform as RectTransform),left,top);
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
