using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBallController : CObjectController {

	[SerializeField]	protected GameObject m_BallObject;
	[SerializeField]	protected float m_BallRadius = 2f;
	public float ballRadius {
		get { return this.m_BallRadius; }
	}
	[SerializeField]	protected AnimationCurve m_BallCurve;

	[Header("Owner")]
	[SerializeField]	protected bool m_IsBallActive = true;

	protected IBallControlObject m_BallControlObject;
	protected float m_WaitActiveTime = 0f;
	protected float m_WaitActiveInterval = 3f;
	public bool isBallActive {
		get { 
			return this.m_IsBallActive && this.m_WaitActiveTime <= 0f; 
		}
		set { 
			this.m_IsBallActive = value; 
			if (value == false) {
				this.m_WaitActiveTime = this.m_WaitActiveInterval;
			}
		}
	}

	protected List<IBallControlObject> m_Soccers;

	protected override void Awake ()
	{
		base.Awake ();
		this.m_Soccers = new List<IBallControlObject> ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (this.m_WaitActiveTime > 0f) {
			this.m_WaitActiveTime -= Time.deltaTime;
		} else {
			this.m_IsBallActive = true;
		}
		if (this.m_BallControlObject != null && this.m_IsBallActive) {
			this.UpdatePosition (this.m_BallControlObject.GetBallWorldPosition());
		}
	}

	public virtual void SetBallTo(IBallControlObject target) {
		if (this.m_BallControlObject == target)
			return;
		this.m_IsBallActive = false;
		StartCoroutine (this.HandleSetUpBallTo (target));
	}

	protected virtual IEnumerator HandleSetUpBallTo(IBallControlObject target) {
		var direction = target.GetBallWorldPosition() - this.GetPosition ();
		var maxLength = direction.sqrMagnitude;
		this.m_BallControlObject =	target;
		this.m_BallControlObject.SetBall (this);
		while (direction.sqrMagnitude >= 0.2f) {
			this.m_Transform.position += direction.normalized * 10f * Time.deltaTime;
			direction = target.GetBallWorldPosition() - this.GetPosition ();
			var currentLength = 1f - (direction.sqrMagnitude / maxLength);
			var currentPosition = this.m_BallObject.transform.localPosition; 
			currentPosition.y = Mathf.Lerp (currentPosition.y, m_BallCurve.Evaluate (currentLength) * 3f, 0.75f);
			this.m_BallObject.transform.localPosition = currentPosition; 
			yield return WaitHelper.WaitFixedUpdate;
		}
		this.m_IsBallActive = true;
		this.m_BallObject.transform.localPosition = Vector3.zero; 
	}

	public virtual void UpdatePosition(Vector3 value) {
		for (int i = 0; i < this.m_Soccers.Count; i++) {
			if (this.m_Soccers [i] != this.m_BallControlObject) {
				this.m_Soccers [i].SetBall (null);
			}
		}
		this.m_Transform.position = Vector3.Lerp (this.m_Transform.position, value, 0.95f);
//		this.SetPosition (value);
	}

	public virtual void UpdateBall(IBallControlObject value) {
		if (this.isBallActive == false)
			return;
		if (this.m_Soccers.Contains (value) == false) {
			this.m_Soccers.Add (value);
		}
		IBallControlObject soccerSelected = null;
		var soccerBiggestScore = 0f;
		for (int i = 0; i < this.m_Soccers.Count; i++) {
			var direction = this.m_Soccers [i].GetPosition () - this.GetPosition ();
			if (direction.sqrMagnitude <= this.m_BallRadius * this.m_BallRadius) {
				if (this.m_Soccers [i].GetBallValue() >= soccerBiggestScore) {
					soccerBiggestScore = this.m_Soccers [i].GetBallValue();
					soccerSelected = this.m_Soccers [i];
				}
			}
		}
		if (soccerSelected != null) {
			this.m_BallControlObject = soccerSelected;
			this.m_BallControlObject.SetBall (this);
		}
	}

}
