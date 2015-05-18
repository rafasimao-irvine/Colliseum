using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : TurnController {

	//public LayerMask ClickLayerMask;

	public Personage PlayerPersonage;

	private Characther _SelectedChar;

	private bool _MadeAction;

	//****************** Swipe variables **************************
	private Vector3 _FirstPressPos;
	private Vector3 _SecondPressPos;
	private Vector3 _CurrentSwipe;
	private float _FirstPressTime;

	private const float _HoldTime = 0.7f;
	private float _PressTimer;
	public InfoHUD PCInfoHUD;

	// Update is called once per frame -----------------------------------
	void Update () {
#if UNITY_EDITOR
		SwipeMouse();
#elif UNITY_ANDROID || UNITY_IOS
		Swipe();
#endif
		if (_IsMyTurn)
			VerifyEndOfTurn();
	}

	private void VerifyEndOfTurn () {
		if (!_MadeAction && PlayerPersonage.MakeAction())
			_MadeAction = true; // Ends player turn

		if (_MadeAction && !PlayerPersonage.IsInAction())
			_MadeAction = _IsMyTurn = false;
	}

	// Swipe press actions -----------------------------------------------
	private void FirstPressAction (Vector3 pos, int id) {
		_FirstPressTime = Time.time;
		Tile t;
		GetClickHitInfo(pos, out _FirstPressPos, out t, id);
	}
	
	private void SecondPressAction (Vector3 pos, int id) {
		if (PCInfoHUD.IsShowing) {
			PCInfoHUD.HideInfo();
			return;
		}

		Tile tile;
		GetClickHitInfo(pos, out _SecondPressPos, out tile, id);
		
		// gets the swipe vector
		_CurrentSwipe = _SecondPressPos - _FirstPressPos;
		
		// If it a tap action
		if (_CurrentSwipe.magnitude<0.6f && (Time.time-_FirstPressTime)<0.2f && tile != null) {
			// Make an action with the selected char
			if(PlayerPersonage.MyTile != tile)
				PlayerPersonage.PrepareActionsTarget(tile); // prepare action
		}
	}

	private void HoldPressAction (Vector3 pos, int id) {
		if((Time.time - _PressTimer) < _HoldTime || PCInfoHUD.IsShowing)
			return;

		Vector3 dif;
		Tile tile;
		GetClickHitInfo(pos, out dif, out tile, id);

		dif = dif - _FirstPressPos;
		if(dif.magnitude < 1 && tile != null && tile.OnTop!=null)
			PCInfoHUD.ShowInfo(tile.OnTop);
	}

	private bool GetClickHitInfo (Vector3 pos, out Vector3 hitPos, out Tile hitTile, int id) {
		Ray ray = Camera.main.ScreenPointToRay (pos);
		RaycastHit hit = new RaycastHit();

		if (!EventSystem.current.IsPointerOverGameObject(id)) { // Used to block ui clcks!
			if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Tile"))) {
				hitPos = hit.point;
				hitTile = hit.transform.GetComponent<Tile>();
				return true;
			}
		}

		hitPos = new Vector3();
		hitTile = null;
		return false;
	}
	
	// Mouse swipe --------------------------------------------------------
	public void SwipeMouse()
	{
		// Beginning of the swipe, first press
		if(Input.GetMouseButtonDown(0)) {
			FirstPressAction(Input.mousePosition, -1);
			_PressTimer = Time.time;
		}

		if(Input.GetMouseButton(0))
			HoldPressAction(Input.mousePosition, -1);

		// End of the swipe, release the swipe
		if(Input.GetMouseButtonUp(0))
			SecondPressAction(Input.mousePosition, -1);
	}
	
	// Real swipe(Mobile!) ------------------------------------------------
	public void Swipe () {

		if (Input.touches.Length == 1) {
			// Beginning of the swipe, first press
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began) {
				FirstPressAction(t.position, 0);
				_PressTimer = Time.time;
			}

			if(t.phase == TouchPhase.Moved)
				HoldPressAction(t.position, 0);

			// End of the swipe, release the swipe
			if(t.phase == TouchPhase.Ended)
				SecondPressAction(t.position, 0);
		}

	}

	// Button Actions -----------------------------------------------------
	public void MakeWaitAction () {
		PlayerPersonage.PrepareWaitAction();
	}

	public void SwitchWeapons () {
		PlayerPersonage.SwitchWeapons();
	}

	// Turn Controller ----------------------------------------------------
	/**
	 * Stats the controller turn.
	 * */
	public override void StartMyTurn () {
		_IsMyTurn = true;
	}
	
}
