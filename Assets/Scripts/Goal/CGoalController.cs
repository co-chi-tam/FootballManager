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

	#endregion

	#region Getter && Setter

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

	#endregion
}
