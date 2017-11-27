using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBallController : CObjectController {

	[SerializeField]	protected float m_BallRadius = 2f;

	protected List<CSoccerPlayerController> m_Soccers;

	protected override void Awake ()
	{
		base.Awake ();
		this.m_Soccers = new List<CSoccerPlayerController> ();
	}

	public virtual void UpdatePosition(Vector3 value) {
		this.SetPosition (value);
	}

	public virtual void UpdateBall(CSoccerPlayerController value) {
		if (this.m_Soccers.Contains (value) == false) {
			this.m_Soccers.Add (value);
		}
		CSoccerPlayerController soccerSelected = null;
		var soccerBiggestScore = 0f;
		for (int i = 0; i < this.m_Soccers.Count; i++) {
			var direction = this.m_Soccers [i].GetPosition () - this.GetPosition ();
			if (direction.sqrMagnitude <= this.m_BallRadius * this.m_BallRadius) {
				if (this.m_Soccers [i].ballValue >= soccerBiggestScore) {
					soccerBiggestScore = this.m_Soccers [i].ballValue;
					soccerSelected = this.m_Soccers [i];
				}
			}
			this.m_Soccers [i].Ball = null;
		}
		if (soccerSelected != null) {
			soccerSelected.Ball = this;
		}
	}

}
