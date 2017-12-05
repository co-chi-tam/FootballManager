using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerChaseBallState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerChaseBallState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.RunSpeed();
		this.m_Controller.ballValue = Random.Range (1, 2f);
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		var ball = this.m_Controller.Team.Ball;
//		var nearestPosition = this.m_Controller
//			.startPoint
//			.GetNearestPosition (
//				this.m_Controller.GetPosition(),
//				ball.GetPosition(), 
//				1f);
		this.m_Controller.SetTargetPosition (ball.GetPosition());
		ball.UpdateBall (this.m_Controller);
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
