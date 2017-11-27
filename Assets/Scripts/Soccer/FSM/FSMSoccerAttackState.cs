using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAttackState : FSMBaseState { 

	protected CSoccerPlayerController m_Controller;
	protected CObjectController m_Goal;

	public FSMSoccerAttackState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Goal = this.m_Controller.Team.EnemyGoal;
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		// MOVE TO GOAL TARGET;
		var point = this.m_Goal;
		this.m_Controller.SetTargetPosition (point.GetPosition());
		var ball = this.m_Controller.Ball;
		if (ball != null) {
			var ballWorldPosition = this.m_Controller.ballWorldPosition.transform.position;
			ball.UpdatePosition (ballWorldPosition);
		}
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
