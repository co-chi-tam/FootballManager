using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMTeamAttackState : FSMBaseState {

	protected CTeamController m_Controller;

	public FSMTeamAttackState (IContext context) : base (context)
	{
		this.m_Controller = context as CTeamController;
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
