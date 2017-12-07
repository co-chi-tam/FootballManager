using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBallController : CObjectController {

	#region Fields

	[Header("Data")]
	[SerializeField]	protected GameObject m_BallObject;
	[SerializeField]	protected float m_BallRadius = 2f;
	public float ballRadius {
		get { return this.m_BallRadius; }
	}
	[SerializeField]	protected float m_BallSpeed = 15f;
	public float ballSpeed {
		get { return this.m_BallSpeed; }
	}
	[SerializeField]	protected AnimationCurve m_BallCurve;

	protected bool m_IsBallActive = true;

	protected IBallControlObject m_BallOwner;
	public IBallControlObject owner {
		get { return this.m_BallOwner; }
		private set { this.m_BallOwner = value; }
	}

	protected float m_BallHeight = 3f;
	public float ballHeight {
		get { return this.m_BallHeight; }
	}

	protected float m_MaxLength = 0f;

	protected float m_WaitActiveTime = 0f;
	protected float m_WaitActiveInterval = 2f;
	public bool isBallActive {
		get { 
			return this.m_IsBallActive && this.m_WaitActiveTime <= 0f; 
		}
		set { 
			this.m_IsBallActive = value; 
			this.m_WaitActiveTime = value ? 0f : this.m_WaitActiveInterval;
		}
	}

	#endregion

	#region Implementation Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (this.m_WaitActiveTime > 0f) {
			this.m_WaitActiveTime -= Time.deltaTime;
		} else {
			this.m_IsBallActive = true;
		}
		if (this.m_BallOwner != null) {
			this.UpdatePosition (this.m_BallOwner.GetBallWorldPosition());
		}
	}

	#endregion

	#region Getter && Setter

	public virtual void SetBallTo(IBallControlObject target) {
		this.UpdateOwner (target);
		this.m_BallObject.transform.localPosition = Vector3.zero; 
		this.m_MaxLength = (target.GetBallWorldPosition() - this.GetPosition ()).sqrMagnitude - this.ballRadius;
		this.m_BallHeight = Random.Range (0, 3f);
	}

	public virtual void UpdatePosition(Vector3 value) {
		var direction = value - this.m_Transform.position;
		if (direction.sqrMagnitude < this.m_BallRadius * this.m_BallRadius) {
			this.m_Transform.position = Vector3.Lerp (this.m_Transform.position, value, 0.65f);
			this.m_BallObject.transform.localPosition = Vector3.zero; 
		} else {
			var speed = this.m_BallSpeed * Time.deltaTime;
			var movePoint = direction.normalized * speed;
			this.m_Transform.position += movePoint;
			// UPDATE BALL OBJECT
			var curveValue = 1f - (direction.sqrMagnitude / this.m_MaxLength);
			var currentPosition = this.m_BallObject.transform.localPosition; 
			currentPosition.y = this.m_BallCurve.Evaluate (curveValue) * this.m_BallHeight;
			if (currentPosition.sqrMagnitude > 0f) {
				this.m_BallObject.transform.localPosition = currentPosition;
			}
		}
	}

	public virtual void UpdateBall(IBallControlObject value) {
		// NOT ACTIVE
		if (this.isBallActive == false)
			return;
		// NOT HAVE TEAM
		if (value.GetTeam () == null)
			return;
		// IS INRANGE
		var direction = value.GetPosition () - this.GetPosition ();
		if (direction.sqrMagnitude >= this.m_BallRadius * this.m_BallRadius)
			return;
		// SET OWNER
		if (this.m_BallOwner == null) {
			this.UpdateOwner (value);
		} else {
			if (value.GetTeam ().teamName == this.m_BallOwner.GetTeam ().teamName)
				return;
			// CHECK BALL VALUE
			if (value.GetBallValue () >= this.m_BallOwner.GetBallValue ()) {
				this.m_BallOwner.SetBall (null);
				this.UpdateOwner (value);
			}
		}
	}

	protected virtual void UpdateOwner(IBallControlObject value) {
		this.m_BallOwner = value;
		this.m_BallOwner.SetBall (this);
	}

	#endregion

}
