using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGoalController : CObjectController, IBallControlObject {

	#region Fields

	[Header("Ball")]
	[SerializeField]	protected GameObject m_BallWorldPosition;
	public GameObject ballWorldPosition {
		get { return this.m_BallWorldPosition; }
		set { this.m_BallWorldPosition = value; }
	}

	[Header("Team")]
	[SerializeField]	protected CTeamController m_TeamController;
	public CTeamController Team {
		get { return this.m_TeamController; }
		set { this.m_TeamController = value; }
	}

	#endregion

	#region Getter && Setter

	public override Vector3 GetNearestPosition(Vector3 value) {
		if (this.m_Collider == null)
			return base.GetNearestPosition(value);
		return this.m_Collider.ClosestPoint (value);
	}

	public virtual Vector3 GetBallWorldPosition ()
	{
		return this.m_BallWorldPosition.transform.position;
	}

	public virtual float GetBallValue ()
	{
		return 0f;
	}

	public virtual void SetBall(CBallController value) {
		
	}

	public virtual CTeamController GetTeam() {
		return this.m_TeamController;
	}

	#endregion

}
