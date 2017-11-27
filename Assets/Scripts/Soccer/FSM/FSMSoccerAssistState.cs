using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAssistState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerAssistState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		var soccerHaveBall = this.m_Controller.Team.GetSoccerHaveBall ();
		var nearestPosition = this.m_Controller
			.startPoint
			.GetNearestPosition (
				this.m_Controller.GetPosition(),
				soccerHaveBall.GetPosition(), 
				this.m_Controller.interactiveRadius);
		this.m_Controller.SetTargetPosition (nearestPosition);
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
