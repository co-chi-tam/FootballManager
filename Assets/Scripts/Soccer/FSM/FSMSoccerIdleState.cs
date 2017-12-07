using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerIdleState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerIdleState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		// Return Point
		this.m_Controller.WalkSpeed();
		this.m_Controller.StandByPoint ();
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
