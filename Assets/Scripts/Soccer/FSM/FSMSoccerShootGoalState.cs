using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerShootBallState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerShootBallState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		// Return Point
		this.m_Controller.WalkSpeed();
		this.m_Controller.StandByPoint ();
		var ball = this.m_Controller.Team.Ball;
		var goal = this.m_Controller.Team.EnemyGoal as IBallControlObject;
		ball.isBallActive = false;
		ball.SetBallTo (goal);
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
