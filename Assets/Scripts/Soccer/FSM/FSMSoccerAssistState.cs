using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSMSoccerAssistState : FSMBaseState {

	protected CSoccerPlayerController m_Controller;
	protected float m_Degree = 45;

	public FSMSoccerAssistState (IContext context) : base (context)
	{
		this.m_Controller = context as CSoccerPlayerController;
	}

	public override void StartState()
	{
		base.StartState ();
		this.m_Controller.RunSpeed();
		this.m_Degree = Random.Range (0f, 90f) - 45f;
	}

	public override void UpdateState(float dt)
	{
		base.UpdateState (dt);
		// GET BALL
		var soccerHaveBall = this.m_Controller.Team.GetSoccerHaveBall ();
		var forward = soccerHaveBall
			.transform
			.TransformDirection (Vector3.forward);
		// GET NEAREST POINT
		var nearestPosition = this.m_Controller
			.startPoint
			.GetDegreeNearestPosition (	
				this.m_Degree,	
				this.m_Controller.GetPosition(),
				forward,
				soccerHaveBall.GetPosition(), 
				this.m_Controller.interactiveRadius);
		// UPDATE POSITION
		this.m_Controller.SetTargetPosition (nearestPosition);
//		this.m_Controller.UpdateCurrentNavAgent (soccerHaveBall.GetPosition());
//		this.m_Controller.UpdateLookingBall ();
#if UNITY_EDITOR
		Debug.DrawLine (this.m_Controller.GetPosition (), nearestPosition, Color.green);
#endif
	}

	public override void ExitState()
	{
		base.ExitState ();
	}

}
