using UnityEngine;
using System.Collections;

public class EnemyAIFabric {

	public enum AIAction {
		None, MoveRandom, FollowPersonage, AttackPersonage, 
		RunFromPersonage, RunAttackRand, MoveRandAttackRand,
		MoveRandAndHeal, MoveRandAndFusion, FollowAndFusion,
		FollowAndPlaceTraps, RunAndAlarm, MoveRandAndInvoke,
		UseAccessoryOnTarget
	}

	public static EnemyAI CreateEnemyAI (AIAction action) {
		EnemyAI enemyAI = null;
		switch (action) {
		case AIAction.None:
			break;
		case AIAction.MoveRandom:
			enemyAI = new MoveRandomAI();
			break;
		case AIAction.FollowPersonage:
			enemyAI = new FollowCharAI();
			break;
		case AIAction.AttackPersonage:
			enemyAI = new AttackCharAI();
			break;
		case AIAction.RunFromPersonage:
			enemyAI = new RunFromCharAI();
			break;
		case AIAction.RunAttackRand:
			enemyAI = new RunFromCharAndAttackRandTileAI();
			break;
		case AIAction.MoveRandAttackRand:
			enemyAI = new MoveRandomAndAttackRandomTile();
			break;
		case AIAction.MoveRandAndHeal:
			enemyAI = new MoveRandomAndHealAI();
			break;
		case AIAction.MoveRandAndFusion:
			enemyAI = new MoveRandomAndFusionAI();
			break;
		case AIAction.FollowAndFusion:
			enemyAI = new FollowAndFusionAI();
			break;
		case AIAction.FollowAndPlaceTraps:
			enemyAI = new FollowAndPlaceTrapAI();
			break;
		case AIAction.RunAndAlarm:
			enemyAI = new RunAndAlarmAI();
			break;
		case AIAction.MoveRandAndInvoke:
			enemyAI = new MoveRandomAndInvokeAI();
			break;
		case AIAction.UseAccessoryOnTarget:
			enemyAI = new UseAccessoryOnTargetAI();
			break;
		}

		return enemyAI;
	}

}
