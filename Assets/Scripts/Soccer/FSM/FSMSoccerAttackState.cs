using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAttackState : FSMBaseState { 

	protected CSoccerPlayerController m_Controller;
	protected CObjectController m_Goal;

	public FSMSoccerAttackState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.WalkSpeed();
		this.m_Goal = this.m_Controller.Team.EnemyGoal;
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		// MOVE TO GOAL TARGET;
		this.m_Controller.SetTargetPosition (this.m_Goal.GetPosition());
//		this.m_Controller.UpdateCurrentNavAgent (this.m_Goal.GetPosition());
//		this.m_Controller.UpdateLookingBall ();
#if UNITY_EDITOR
		Debug.DrawLine (this.m_Controller.GetPosition (), this.m_Goal.GetPosition(), Color.green);
#endif
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
