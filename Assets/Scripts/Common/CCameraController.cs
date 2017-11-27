using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : CObjectController {

	[SerializeField]	protected CObjectController m_Follower;

	protected Vector3 m_PositionOffset;

	protected override void Awake ()
	{
		base.Awake ();
		this.m_PositionOffset = this.m_Transform.position;
	}

	protected override void Update ()
	{
		base.Update ();
		if (this.m_Follower != null) {
			var target = this.m_PositionOffset + this.m_Follower.GetPosition ();
			this.m_Transform.position = Vector3.Lerp(this.GetPosition(), target, 0.25f);
		}
	}

}
