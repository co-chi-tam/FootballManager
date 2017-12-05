using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAttackState : FSMBaseState { 

	protected CSoccerPlayerController m_Controller;
	protected Vector3 m_GoalPosition;

	public FSMSoccerAttackState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.WalkSpeed();
		var goal = this.m_Controller.Team.EnemyGoal as CGoalController;
		this.m_GoalPosition = goal.GetNearestPosition (this.m_Controller.GetPosition ());
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		// MOVE TO GOAL TARGET;
		this.m_Controller.SetTargetPosition (this.m_GoalPosition);
//		this.m_Controller.UpdateCurrentNavAgent (this.m_Goal.GetPosition());
//		this.m_Controller.UpdateLookingBall ();
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
