using System;
using UnityEngine;
using FSM;

public interface ISoccerContext : IContext {

	bool IsTeamAttacking();
	bool HaveBall();
	bool PassTheBall();
	bool DidToManyChaseBall();
	bool IsNearAllyGoal();
	bool IsNearEnemyGoal();
	bool IsBallInRange();

}
