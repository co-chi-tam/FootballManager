using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMTeamIdleState : FSMBaseState {

	protected CTeamController m_Controller;

	public FSMTeamIdleState (IContext context) : base (context)
	{
		this.m_Controller = context as CTeamController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.SpawnTeam ();
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
