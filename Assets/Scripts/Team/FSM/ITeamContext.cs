using System;
using UnityEngine;
using FSM;

public interface ITeamContext : IContext {

	bool IsPlaying();
	bool IsTeamHaveBall();
	bool IsNearAllyGoal();
	bool IsNearEnemyGoal();

}
