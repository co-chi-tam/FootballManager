﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		this.m_Controller.WalkSpeed();
		// STAND BY
		this.m_Controller.SetTargetPosition (this.m_Controller.GetPosition());
		// FIND ALLY
		var soccerColliders = Physics.OverlapSphere (
			this.m_Controller.GetPosition (), 
			this.m_Controller.interactiveRadius, 
			this.m_Controller.targetLayerMask);
		var goal = this.m_Controller.Team.EnemyGoal;
		// IF ALLY NEAREST ENEMY GOAL 
		var allyPlayers = soccerColliders
			.Where((x) => {
				var objController = x.GetComponent<CSoccerPlayerController> (); 
				return objController != null 
					&& objController != this.m_Controller
					&& objController.Team.teamName == this.m_Controller.Team.teamName;
			}).OrderBy((a) => {
				if (a.gameObject == this.m_Controller.gameObject)
					return 9999f;
				var distance = (goal.GetPosition() - a.transform.position).sqrMagnitude;
				return distance;
			}).ToArray();
		// SET UP BALL
		var ball = this.m_Controller.Team.Ball;
		ball.isBallActive = false;
		CSoccerPlayerController nearestAlly = null;
		if (allyPlayers.Length > 0) {
			nearestAlly = allyPlayers [0].GetComponent<CSoccerPlayerController> ();
			nearestAlly.WalkSpeed ();
			ball.SetBallTo (nearestAlly);
			this.m_Controller.Ball = null;
		} 
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
