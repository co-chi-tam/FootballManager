﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjectController : MonoBehaviour {

	#region Fields 

	[Header("Collider")]
	[SerializeField]	protected Collider m_Collider;

	protected Transform m_Transform;
	protected Vector3 m_StartPosition;
	protected bool m_Active = true;

	#endregion

	#region Implemetation Monobehaviour

	public virtual void Init() {
	
	}

	protected virtual void Awake() {
		this.m_Transform = this.transform;
		this.m_StartPosition = this.m_Transform.position;
	}

	protected virtual void Start() {
	
	}

	protected virtual void Update() {
		
	}

	protected virtual void LateUpdate() {
		
	}

	protected virtual void OnDrawGizmos() {

	}

	#endregion

	#region Utility

	public virtual bool IsNearestPosition(Vector3 value) {
		var position = this.GetPosition ();
		var direction = position - value;
		var nearestDistance = 0.1f;
		return direction.sqrMagnitude <= nearestDistance;
	}

	#endregion

	#region Getter && Setter

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public override bool Equals (object other)
	{
		return base.Equals (other);
	}

	public virtual void SetData(CObjectData value) {
	
	}

	public virtual CObjectData GetData() {
		return null;
	}

	public virtual bool GetActive() {
		return this.m_Active;
	}

	public virtual void SetActive(bool value) {
		this.m_Active = value;
	}

	public virtual void SetPosition(Vector3 position) {
		if (this.m_Transform == null)
			return;
		this.m_Transform.position = position;
	}

	public virtual Vector3 GetPosition() {
		if (this.m_Transform == null)
			return Vector3.zero;
		return this.m_Transform.position;
	}

	public virtual Vector3 GetNearestPosition(Vector3 value) {
		return this.GetPosition ();
	}

	#endregion

}
