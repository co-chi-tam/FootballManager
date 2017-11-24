using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAttackState : FSMBaseState { 

	protected CSoccerPlayerController m_Controller;

	public FSMSoccerAttackState (IContext context) : base (context)
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
		// MOVE FORWARD TARGET;
		var point = this.m_Controller.currentPoint;
		var nearestPoint = point.GetNearestPoint (this.m_Controller.GetPosition ());



		this.m_Controller.SetTargetPosition (nearestPoint.GetPosition());
		if (this.m_Controller.IsNearestPosition (nearestPoint.GetPosition())) {
			this.m_Controller.currentPoint = nearestPoint;
		}
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
