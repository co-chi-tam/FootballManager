using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM;

[RequireComponent(typeof(NavMeshAgent))]
public class CSoccerPlayerController : CObjectController, ISoccerContext {

	#region Fields

	[Header("FSM")]
	[SerializeField]	protected TextAsset m_FSMTextAsset;
#if UNITY_EDITOR
	[SerializeField]	protected string m_FSMStateName;
#endif

	[Header("Control")]
	[SerializeField]	protected float m_MoveSpeed = 5f;
	[SerializeField]	protected float m_BallValue = 1f;
	public float ballValue {
		get { return this.m_BallValue; }
		set { this.m_BallValue = value; }
	}
	[SerializeField]	protected float m_InteractiveRadius = 3f;
	public float interactiveRadius {
		get { return this.m_InteractiveRadius; }
		set { this.m_InteractiveRadius = value; }
	}
	[SerializeField]	protected CPointController m_StartPoint;
	public CPointController startPoint {
		get { return this.m_StartPoint; }
		set { this.m_StartPoint = value; }
	}
	[SerializeField]	protected CPointController m_CurrentPoint;
	public CPointController currentPoint {
		get { return this.m_CurrentPoint; }
		set { this.m_CurrentPoint = value; }
	}
	[SerializeField]	protected LayerMask m_TargetLayerMask;
	[Header("Ball")]
	[SerializeField]	protected CBallController m_Ball;
	public CBallController Ball {
		get { return this.m_Ball; }
		set { this.m_Ball = value; }
	}
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

	protected NavMeshAgent m_NavMeshAgent;
	protected FSMManager m_FSMManager;

	protected float m_MaxSpeed = 3.5f;
	protected float[] m_AngleCheckings = new float[] { 0, -15, 15, -45, 45 }; 
	protected float[] m_AngleAvoidances = new float[] { 10, 40, -40, 40, -40 }; 
	protected float[] m_LengthAvoidances = new float[] { 3f, 3f, 3f, 3f, 3f };

	#endregion

	#region Monobehaviour Implementation

	protected override void Awake ()
	{
		base.Awake ();
		this.m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
		this.m_NavMeshAgent.speed = this.m_MoveSpeed + Random.Range (0, 2f); 
		this.m_MaxSpeed = this.m_NavMeshAgent.speed;

		this.m_FSMManager = new FSMManager ();

		this.m_FSMManager.RegisterState ("FSMSoccerIdleState", 		new FSMSoccerIdleState(this));
		this.m_FSMManager.RegisterState ("FSMSoccerAttackState", 	new FSMSoccerAttackState(this));
		this.m_FSMManager.RegisterState ("FSMSoccerDefendState", 	new FSMSoccerDefendState(this));
		this.m_FSMManager.RegisterState ("FSMSoccerPassBallState", 	new FSMSoccerPassBallState(this));
		this.m_FSMManager.RegisterState ("FSMSoccerAssistState", 	new FSMSoccerAssistState(this));
		this.m_FSMManager.RegisterState ("FSMSoccerChaseBallState", new FSMSoccerChaseBallState(this));

		this.m_FSMManager.RegisterCondition ("IsTeamAttacking",		this.IsTeamAttacking);
		this.m_FSMManager.RegisterCondition ("HaveBall", 			this.HaveBall);
		this.m_FSMManager.RegisterCondition ("PassTheBall", 		this.PassTheBall);

		this.m_FSMManager.LoadFSM (this.m_FSMTextAsset.text);
	}

	protected override void Update ()
	{
		base.Update ();		
		this.m_FSMManager.UpdateState (Time.deltaTime);
#if UNITY_EDITOR
		this.m_FSMStateName = this.m_FSMManager.currentStateName;
#endif
	}

	protected override void OnDrawGizmos ()
	{
		base.OnDrawGizmos ();
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (this.GetPosition (), this.m_InteractiveRadius);
	}

	#endregion

	#region Main methods

	public virtual void ReturnStartPoint() {
		if (this.m_StartPoint == null) {
			this.SetTargetPosition (this.m_StartPosition);
		} else {
			this.SetTargetPosition (this.m_StartPoint.GetPosition ());
		}
	}

	public virtual void UpdateCurrentNavAgent() {
		var currentTransform = this.m_Transform;
		var forward = currentTransform.forward;
		var tmpSpeedThreshold = 1f;
		var radiusBase = this.m_NavMeshAgent.radius;
		for (int i = 0; i < m_AngleCheckings.Length; i++) {
			var rayCast = Quaternion.AngleAxis(m_AngleCheckings[i], currentTransform.up) * forward * m_LengthAvoidances[i];
			RaycastHit rayCastHit;
			var direction = currentTransform.position + (rayCast.normalized * radiusBase);
			if (Physics.Raycast (direction, rayCast, out rayCastHit, m_LengthAvoidances[i], this.m_TargetLayerMask)) {

			} 
#if UNITY_EDITOR
			Debug.DrawRay (direction, rayCast, Color.white);
#endif
		}
		this.m_NavMeshAgent.speed = this.m_MaxSpeed * tmpSpeedThreshold;
	}

	#endregion

	#region ISoccerContext implementation

	public virtual bool IsTeamAttacking() {
		return this.m_TeamController.IsTeamHaveBall();
	}

	public virtual bool HaveBall ()
	{
		return this.m_Ball != null;
	}

	public virtual bool PassTheBall ()
	{
		return false;
	}

	#endregion

	#region Getter && Setter 

	public virtual void SetTargetPosition(Vector3 position) {
		this.m_NavMeshAgent.destination = position;
	}

	public virtual Vector3 GetTargetPosition() {
		return this.m_NavMeshAgent.destination;
	}

	#endregion
}
