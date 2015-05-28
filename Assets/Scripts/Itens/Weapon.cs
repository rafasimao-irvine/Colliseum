using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weapon {

	[HideInInspector]
	public GameObject MyGameObject;

	enum AttackArea {
		Target, FrontArea, Block, Flower, Line
	}

	[SerializeField]
	private bool _Permanent = false;

	#pragma warning disable 0649
	[SerializeField]
	private GameEffect _AtkEffect;

	[SerializeField]
	private AttackArea _AtkArea;
	[SerializeField]
	private int _AtkDamage, _AtkReachRange, _AtkAreaRange, _AtkDelay, _Durability;
	private int _DelayCounter;
	#pragma warning restore 0649

	public void UpdateWeapon () {
		if (_DelayCounter <= _AtkDelay)
			_DelayCounter++;
	}

	public void Attack (Characther personage, Interactive target) {
		Attack(personage,target.MyTile);
	}

	public void Attack (Characther personage, Tile targetTile) {
		if (IsBroken() || !IsReady())
			return;
		
		if (targetTile.OnTop!=null && targetTile.OnTop.Interactable) {
			targetTile.OnTop.BeAttacked(personage,1);
			return;
		}
		
		switch (_AtkArea) {
		case AttackArea.Target:
			PerformAttack(personage,targetTile);
			break;
		case AttackArea.FrontArea:
			AttackFrontArea(personage,targetTile);
			break;
		case AttackArea.Block:
			AttackBlock(personage,targetTile);
			break;
		case AttackArea.Flower:
			AttackFlower(personage,targetTile);
			break;
		case AttackArea.Line:
			AttackLine(personage,targetTile);
			break;
		}
		
		DecreaseDurability();
		_DelayCounter=0; //reboot cooldown
	}

	private void DecreaseDurability () {
		if (!_Permanent && _Durability>0)
			_Durability--;
	}

	#region Area Attacks
	private void AttackFrontArea (Characther personage, Tile targetTile) {
		AttackTiles(
			personage,
			MapController.Instance.GetFrontArea(
			personage.MyTile,
			MapController.Instance.GetDirection(personage.MyTile, targetTile) ));
	}

	private void AttackBlock (Characther personage, Tile targetTile) {
		AttackTiles(
			personage,
			MapController.Instance.GetBlock(targetTile) );
	}

	private void AttackFlower (Characther personage, Tile targetTile) {
		AttackTiles(
			personage,
			MapController.Instance.GetFlower(
			targetTile,
			MapController.Instance.GetDirection(personage.MyTile, targetTile) ));
	}

	private void AttackLine (Characther personage, Tile targetTile) {
		AttackTiles(
			personage,
			MapController.Instance.GetLine(
			personage.MyTile,
			MapController.Instance.GetDirection(personage.MyTile, targetTile),
			_AtkAreaRange));
	}

	private void AttackTiles (Characther personage, List<Tile> tiles) {
		for (int i=0; i<tiles.Count; i++)
			PerformAttack(personage, tiles[i]);
	}
	#endregion

	#region Performe Attacks
	private void PerformAttack (Characther personage, Tile targetTile) {
		if (targetTile.OnTop!=null && targetTile!=personage.MyTile)
			PerformAttack(personage, targetTile.OnTop);
	}

	private void PerformAttack (Characther personage, Interactive target) {
		if (_AtkDamage > 0 )
			target.BeAttacked(personage, _AtkDamage+personage.UseStrengthModifiers(target));

		if (_AtkEffect!=null)
			_AtkEffect.MakeEffect(personage,target);
	}
	#endregion

	#region Getters
	public int GetWeaponAttackRange () {
		return _AtkReachRange;
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
	#endregion

}
