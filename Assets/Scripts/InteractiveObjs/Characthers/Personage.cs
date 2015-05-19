using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Personage : Characther {

	// Audios
	public AudioClip NoWeaponAtkSound, WeaponAtkSound, BeHitSound, DeathSound;

	// Interactions Types
	protected enum Interactions{
		None = 0,
		Move = 1,
		Attack = 2,
		Interact = 3,
		ActivateAccessory = 4
	};
	protected Interactions _InteractionType = Interactions.None;

	// Targeting
	protected Tile _TargetTile;
	protected Interactive _TargetInteractive;

	protected int _AccessoryIndex;

	public List<Tile> path;

	// Wait action
	protected bool _WaitAction = false;

	void OnAwake () {
		base.Start ();
		path = null;
		_TargetTile = null;
		_TargetInteractive = null;
		_WaitAction = false;
		_InteractionType = Interactions.None;
	}

	/**
	 * Prepare the next moves based in the interaction type performed.
	 */
	public void PrepareActionsTarget (Tile targetTile) {
		// Set the interaction type
		if (targetTile.OnTop != null) {
			// Attack
			if (targetTile.OnTop.Attackable)
				_InteractionType = Interactions.Attack;
			// Interact
			else if (targetTile.OnTop.Interactable)
				_InteractionType = Interactions.Interact;
			// Move to obstacle/enterable
			else
				_InteractionType = Interactions.Move;
		// Movement
		} else
			_InteractionType = Interactions.Move;

		// Set target
		if (_InteractionType == Interactions.Move)
			SetTarget(targetTile, null);
		else
			SetTarget(null, targetTile.OnTop);
	}

	protected void SetTarget (Tile targetTile, Interactive targetInteractive) {
		_TargetTile = targetTile;
		_TargetInteractive = targetInteractive;
	}

	public void PrepareWaitAction () {
		_WaitAction = true;
	}

	public void PrepareAccessoryAction (int index, Tile tile) {
		_AccessoryIndex = index;
		SetTarget(tile, null);
		_InteractionType = Interactions.ActivateAccessory;
	}

	/**
	 * Makes an action related to the given tile.
	 * Returns 0 if it was a terminal action, otherwise returns > 0.
	 */
	protected override bool MakeTurnAction () {
		if (_WaitAction) {
			_WaitAction = false;
			InterruptActions();
			return true;
		}

		bool result = false;
		SelectPath(false);
		switch (_InteractionType) {
		case Interactions.None:
			result = false; // No action occurred
			break;
		case Interactions.Attack:
			result = MakeAttackAction(GetCurrentAttackRange());
			break;
		case Interactions.Interact:
			result = MakeAttackAction(1);
			break;
		case Interactions.Move:
			result = MakeMoveAction(_TargetTile);
			break;
		case Interactions.ActivateAccessory:
			result = MakeAccessoryAction();
			break;
		}
		SelectPath(true);
		return result;
	}

	private bool MakeAttackAction (int range) {
		if (_TargetInteractive.Attackable && !IsAttackReady()) { 
			InterruptActions();
			return false;
		}

		// If it is close to the target personage, attack!
		if (MapController.Instance.GetNeighbours(MyTile,range).Contains(_TargetInteractive.MyTile)) {
			AttackAction(_TargetInteractive);
			InterruptActions();
		} 
		else {
			if (_TargetInteractive!=null)
				MakeMoveAction(_TargetInteractive.MyTile);
		}

		return true;
	}

	private void AttackAction (Interactive target) {
		if (Sounds.Instance !=null) {
			if (_CharWeapons.IsAnyFirstWeaponEquipped() && !target.Interactable)
				Sounds.Instance.PlaySoundEffect(WeaponAtkSound);
			else
				Sounds.Instance.PlaySoundEffect(NoWeaponAtkSound);
		}
		Attack(_TargetInteractive);
	}

	private bool MakeMoveAction (Tile target) {
		// Take the path to the target
		path = (target!=null) ? MapController.Instance.FindPath(MyTile, target) : null;

		// If there is no path, than return no action
		if (path == null || path.Count<1) 
			return false; // Path not found: Return no action

		// Otherwise, move
		AddMoveTo(path[0]);
		if(path[0]==target) InterruptActions();

		return true; // Return that an action occurred
	}

	private bool MakeAccessoryAction () {
		ActivateAccessory(_AccessoryIndex,_TargetTile);
		InterruptActions();
		return true;
	}

	public void InterruptActions () {
		SetTarget(null, null);
		_InteractionType = Interactions.None;
		if (path!=null) {
			SelectPath(false);
			path = null;
		}
	}

	public override void BeSaw (Interactive target) {
		InterruptActions ();
		base.BeSaw(target);
	}

	public override void BeAttacked (Interactive iObj, int damage) {
		base.BeAttacked (iObj,damage);
		InterruptActions();

		if (Sounds.Instance !=null) {
			if (IsDead())
				Sounds.Instance.PlaySoundEffect(DeathSound);
			else
				Sounds.Instance.PlaySoundEffect(BeHitSound);
		}
	}

	// Select path tiles
	private void SelectPath (bool selected) {
		if(path!=null)
			foreach(Tile t in path)
				if (t!=null) t.SelectTile(selected);
	}

}
