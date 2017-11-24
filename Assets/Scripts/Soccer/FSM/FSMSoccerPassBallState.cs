using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerPassBallState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerPassBallState (IContext context) : base (context)
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
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
