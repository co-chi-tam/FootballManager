using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerDefendState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;
	protected CBallController m_BallController;

	public FSMSoccerDefendState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_BallController = this.m_Controller.Team.Ball;
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		var nearestPoint = this.m_Controller
			.startPoint
			.GetNearestPoint (this.m_BallController.GetPosition());
		this.m_Controller.SetTargetPosition (nearestPoint.GetPosition());
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
