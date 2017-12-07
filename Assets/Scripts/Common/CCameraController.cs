using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour {

	[Header("Control")]
	[SerializeField]	protected CObjectController m_Follower;
	[SerializeField]	protected Vector3 m_PositionOffset;
	[SerializeField]	protected float m_Damping = 0.25f;

	protected Transform m_Transform;

	protected virtual void Awake ()
	{
		this.m_Transform = this.transform;
	}

	protected virtual void Update ()
	{
		if (this.m_Follower != null) {
			var target = this.m_PositionOffset + this.m_Follower.GetPosition ();
			this.m_Transform.position = Vector3.Lerp(this.m_Transform.position, target, this.m_Damping);
		}
	}

}
