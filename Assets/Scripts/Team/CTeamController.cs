using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class CTeamController : CObjectController, ITeamContext {

	#region Fields

	[Header("FSM")]
	[SerializeField]	protected TextAsset m_FSMTextAsset;
#if UNITY_EDITOR
	[SerializeField]	protected string m_FSMStateName;
#endif

	[Header("Map")]
	[SerializeField]	protected GameObject m_AllyGoal;
	[SerializeField]	protected GameObject m_EnemyGoal;
	[SerializeField]	protected CPointController[] m_StrikerPoints;
	[SerializeField]	protected CPointController[] m_DefenderPoints;
	[SerializeField]	protected CPointController[] m_GoalPoints;

	[Header("Team")]
	[SerializeField]	protected int m_StrikerCount = 5;
	[SerializeField]	protected int m_DefenderCount = 5;
	[SerializeField]	protected int m_GoalKeeperCount = 1;
	[SerializeField]	protected CSoccerPlayerController[] m_StrikerSoccers;
	[SerializeField]	protected CSoccerPlayerController[] m_DefenderSoccers;
	[SerializeField]	protected CSoccerPlayerController[] m_GoalKeeperSoccers;

	[Header("Soccer")]
	[SerializeField]	protected string m_SoccerPrefabPath;

	protected FSMManager m_FSMManager;

	#endregion

	#region MonoBehaviour Implementation

	protected override void Awake ()
	{
		base.Awake ();
		this.m_FSMManager = new FSMManager ();

		this.m_FSMManager.RegisterState ("FSMTeamIdleState", 	new FSMTeamIdleState(this));
		this.m_FSMManager.RegisterState ("FSMTeamAttackState", 	new FSMTeamAttackState(this));
		this.m_FSMManager.RegisterState ("FSMTeamDefendState", 	new FSMTeamDefendState(this));

		this.m_FSMManager.RegisterCondition ("IsPlaying", 		this.IsPlaying);
		this.m_FSMManager.RegisterCondition ("HaveBall", 		this.HaveBall);

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

	#endregion

	#region Team Spawner

	public virtual void SpawnTeam() {
		// STRIKER
		this.m_StrikerSoccers = new CSoccerPlayerController[this.m_StrikerCount];
		for (int i = 0; i < this.m_StrikerCount; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_SoccerPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_StrikerPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.Init ();
			this.m_StrikerSoccers [i] = soccer;
		}
		// DEFENDER
		this.m_DefenderSoccers = new CSoccerPlayerController[this.m_DefenderCount];
		for (int i = 0; i < this.m_DefenderCount; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_SoccerPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_DefenderPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.Init ();
			this.m_DefenderSoccers [i] = soccer;
		}
		// GOALKEEPER
		this.m_GoalKeeperSoccers = new CSoccerPlayerController[this.m_GoalKeeperCount];
		for (int i = 0; i < this.m_GoalKeeperCount; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_SoccerPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_GoalPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.Init ();
			this.m_GoalKeeperSoccers [i] = soccer;
		}
	}

	public virtual CSoccerPlayerController SpawnSoccerPlayer(string path) {
		var prefab = Resources.Load<CSoccerPlayerController> (path);
		var soccerPlayer = Instantiate (prefab);
		soccerPlayer.transform.SetParent (this.transform);
		return soccerPlayer;
	}

	#endregion

	#region ITeamContext implementation

	public virtual bool IsPlaying ()
	{
		return true;
	}

	public virtual bool IsAttacking() {
		return this.HaveBall();
	}

	public virtual bool HaveBall ()
	{
		for (int i = 0; i < this.m_StrikerSoccers.Length; i++) {
			if (this.m_StrikerSoccers [i].HaveABall) {
				return true;
			}
		}
		for (int i = 0; i < this.m_DefenderSoccers.Length; i++) {
			if (this.m_DefenderSoccers [i].HaveABall) {
				return true;
			}
		}
		return false;
	}

	#endregion
}
