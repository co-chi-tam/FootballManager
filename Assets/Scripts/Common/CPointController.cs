using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPointController : CObjectController {

	#region Fields

	[SerializeField]	protected float m_Radius = 3f;

	[Header("Sub Points")]
	[SerializeField]	protected List<CPointController> m_SubPoints;

	#endregion

	#region Implementation Monobehaviour

	protected override void Start ()
	{
		base.Start ();
	}

	#endregion

	#region Utility

	public void FindAllSubPoint(GameObject parent) {
		var childCount = parent.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			var child = parent.transform.GetChild (i);
			var childPoint = child.GetComponent<CPointController> ();
			if (childPoint != null) {
				this.m_SubPoints.Add (childPoint);
				this.FindAllSubPoint (child.gameObject);
			}
		}
	}

	#endregion

	#region Getter && Setter

	public virtual Vector3 GetRandomInrangePosition() {
		var randomPosition = UnityEngine.Random.insideUnitCircle * this.m_Radius;
		var currentPosition = this.GetPosition ();
		var nearestPostion = new Vector3 (
			currentPosition.x + randomPosition.x,
			currentPosition.y,
			currentPosition.z + randomPosition.y
		);
		return nearestPostion;
	}

	public virtual CPointController GetNearestPoint(Vector3 position) {
		var nearestPoint = this;
		var nearestDistance = float.MaxValue;
		var direction = Vector3.zero;
		for (int i = 0; i < this.m_SubPoints.Count; i++) {
			var point = this.m_SubPoints [i];
			direction = position - point.GetPosition ();
			if (direction.sqrMagnitude <= nearestDistance) {
				nearestPoint = point;
				nearestDistance = direction.sqrMagnitude;
			}
		}
		return nearestPoint;
	}

	#endregion

}
