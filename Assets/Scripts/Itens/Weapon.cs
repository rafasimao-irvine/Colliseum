using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weapon {

	[HideInInspector]
	public GameObject MyGameObject;

	enum AttackArea {
		Target, FrontArea, Block, Flower
	}

	[SerializeField]
	private bool _Permanent = false;

	#pragma warning disable 0649
	[SerializeField]
	private GameEffect _AtkEffect;

	[SerializeField]
	private AttackArea _AtkArea;
	[SerializeField]
	private int _AtkDamage, _AtkRange, _AtkDelay, _Durability;
	private int _DelayCounter;
	#pragma warning restore 0649

	public void UpdateWeapon () {
		if (_DelayCounter <= _AtkDelay)
			_DelayCounter++;
	}

	public void Attack (Characther personage, Interactive target) {
		if (IsBroken() || !IsReady())
			return;

		if (target.Interactable) {
			target.BeAttacked(personage,1);
			return;
		}

		switch (_AtkArea) {
		case AttackArea.Target:
			PerformAttack(personage,target);
			break;
		case AttackArea.FrontArea:
			AttackFrontArea(personage,target);
			break;
		case AttackArea.Block:
			AttackBlock(personage,target);
			break;
		case AttackArea.Flower:
			AttackFlower(personage,target);
			break;
		}

		DecreaseDurability();
		_DelayCounter=0; //reboot cooldown
	}

	private void AttackFrontArea (Characther personage, Interactive target) {
		AttackTiles(
			personage,
			MapController.Instance.GetFrontArea(
			personage.MyTile,
			MapController.Instance.GetDirection(personage.MyTile, target.MyTile)));
	}

	private void AttackBlock (Characther personage, Interactive target) {
		AttackTiles(
			personage,
			MapController.Instance.GetBlock(target.MyTile));
	}

	private void AttackFlower (Characther personage, Interactive target) {
		AttackTiles(
			personage,
			MapController.Instance.GetFlower(
			target.MyTile,
			MapController.Instance.GetDirection(personage.MyTile, target.MyTile)));
	}

	private void AttackTiles (Characther personage, List<Tile> tiles) {
		for (int i=0; i<tiles.Count; i++) {
			if(tiles[i].OnTop != null && tiles[i] != personage.MyTile)
				PerformAttack(personage, tiles[i].OnTop);
		}
	}

	private void PerformAttack (Characther personage, Interactive target) {
		if (_AtkDamage > 0 )
			target.BeAttacked(personage, _AtkDamage+personage.UseStrengthModifiers(target));

		if (_AtkEffect!=null)
			_AtkEffect.MakeEffect(personage,target);

	}

	private void DecreaseDurability () {
		if (!_Permanent && _Durability>0)
			_Durability--;
	}

	public int GetWeaponAttackRange () {
		return _AtkRange;
	}

	public bool IsBroken () {
		return (!_Permanent && _Durability < 1);
	}

	public bool IsReady () {
		return (_DelayCounter > _AtkDelay);
	}

	public void BeDestroyed () {
		MyGameObject.GetComponent<Interactive>().BeDestroyed();
	}

}
