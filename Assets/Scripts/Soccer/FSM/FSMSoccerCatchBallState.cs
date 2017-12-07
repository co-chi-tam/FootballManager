using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerCatchBallState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerCatchBallState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.RunSpeed();
		this.m_Controller.UpdateTackleBall ();
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		var ball = this.m_Controller.Team.Ball;
		this.m_Controller.SetTargetPosition (ball.GetPosition());
		ball.UpdateBall (this.m_Controller);
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
