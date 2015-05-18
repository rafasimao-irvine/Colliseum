using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	public Personage LogPersonage;
	public EnemiesController LogEnemiesController;
	
	public static string strLog;
	public Vector2 scrollPosition = Vector2.zero;

	bool isEnemiesDead;

	private bool _ShowLog = true;

	void Awake () {
		strLog = "";
	}

	public void ToggleLog () {
		_ShowLog = !_ShowLog;
	}

	void OnGUI () {
		if (_ShowLog) {

			GUI.Label(new Rect(10, 10, 100, 20), "Hero");
			GUI.Label(new Rect(10, 30, 100, 20), "hp: "+LogPersonage.GetLife());
			GUI.Label(new Rect(10, 60, 100, 20), "Enemies");

			isEnemiesDead = true;
			for (int i=0; i<LogEnemiesController.GetEnemies().Count; i++) {
				GUI.Label(new Rect(10, 80+i*20, 100, 20), "hp: "+LogEnemiesController.GetEnemies()[i].GetLife());
				if(LogEnemiesController.GetEnemies()[i].GetLife() > 0)
					isEnemiesDead = false;
			}

			scrollPosition = GUI.BeginScrollView(new Rect(690, 10, 200, 250), scrollPosition, new Rect(0, 0, 180, 800));
			GUI.Label(new Rect(0,0,180,800), strLog);
			GUI.EndScrollView();
			
			if (isEnemiesDead)
				GUI.Label(new Rect(450,200,200,50),"<color=green><size=50>Win!</size></color>");
			if (LogPersonage.GetLife() < 1)
				GUI.Label(new Rect(450,200,200,50),"<color=red><size=50>Lose</size></color>");
		}
	}
}
