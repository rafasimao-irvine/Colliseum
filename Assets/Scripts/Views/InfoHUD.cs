using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoHUD : MonoBehaviour {

	public GameObject InfoPanel;
	public Text InfoText;

	public SightView EnemiesSightView;
	private Enemy _PreviousEnemy;

	public bool IsShowing {get; protected set;}

	public void ShowInfo (Interactive iObj) {
		InfoPanel.SetActive(true);
		InfoText.text = iObj.name.Substring(0,iObj.name.Length-7);
		IsShowing = true;
		// Show Enemy Sight
		if (iObj is Enemy) {
			_PreviousEnemy = (Enemy)iObj;
			EnemiesSightView.ShowDoubleSight(
				_PreviousEnemy.MyTile, 
				_PreviousEnemy.GetVisionRange(), 
				_PreviousEnemy.GetCurrentAttackRange(), 
				true);
		}
	}

	public void HideInfo () {
		InfoPanel.SetActive(false);
		IsShowing = false;
		// Hide enemy sight
		if(_PreviousEnemy!=null) {
			EnemiesSightView.ShowDoubleSight(
				_PreviousEnemy.MyTile, 
				_PreviousEnemy.GetVisionRange(), 
				_PreviousEnemy.GetCurrentAttackRange(), 
				false);
			_PreviousEnemy = null;
		}
	}

}
