using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerDefendState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerDefendState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.RunSpeed();
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		var ball = this.m_Controller.Team.Ball;
		var nearestPoint = this.m_Controller
			.startPoint
			.GetNearestPoint (ball.GetPosition());
		this.m_Controller.SetTargetPosition (nearestPoint.GetPosition());
//		this.m_Controller.UpdateLookingBall ();
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
