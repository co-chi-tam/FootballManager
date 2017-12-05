using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using SimpleSingleton;

public class CTeamController : CMonoSingleton<CTeamController>, ITeamContext {

	#region Fields

	[Header("FSM")]
	[SerializeField]	protected TextAsset m_FSMTextAsset;
#if UNITY_EDITOR
	[SerializeField]	protected string m_FSMStateName;
#endif

	[Header("Map")]
	[SerializeField]	protected CBallController m_BallController;
	public CBallController Ball {
		get { return this.m_BallController; }
		set { this.m_BallController = value; }
	}
	[SerializeField]	protected CObjectController m_AllyGoal;
	public CObjectController AllyGoal {
		get { return this.m_AllyGoal; }
		set { this.m_AllyGoal = value; }
	}
	[SerializeField]	protected CObjectController m_EnemyGoal;
	public CObjectController EnemyGoal {
		get { return this.m_EnemyGoal; }
		set { this.m_EnemyGoal = value; }
	}
	[SerializeField]	protected CPointController[] m_StrikerPoints;
	[SerializeField]	protected CPointController[] m_DefenderPoints;
	[SerializeField]	protected CPointController[] m_GoalPoints;

	[Header("Team")]
	[SerializeField]	protected string m_TeamName;
	public string teamName {
		get { return this.m_TeamName; }
		set { this.m_TeamName = value; }
	}
	[SerializeField]	protected int m_StrikerCount = 5;
	[SerializeField]	protected int m_DefenderCount = 5;
	[SerializeField]	protected int m_GoalKeeperCount = 1;
	[SerializeField]	protected TextAsset m_StrikerFSMAsset;
	[SerializeField]	protected TextAsset m_DefenderFSMAsset;
	[SerializeField]	protected TextAsset m_GoalKeeperFSMAsset;
	[SerializeField]	protected CSoccerPlayerController[] m_StrikerSoccers;
	[SerializeField]	protected CSoccerPlayerController[] m_DefenderSoccers;
	[SerializeField]	protected CSoccerPlayerController[] m_GoalKeeperSoccers;

	[Header("MiniMap")]
	[SerializeField]	protected CMiniMap m_MiniMap;
	public CMiniMap miniMap {
		get { return this.m_MiniMap; }
		set { this.m_MiniMap = value; }
	}

	[Header("Soccer")]
	[SerializeField]	protected CSoccerPlayerController m_SoccerHaveBall;
	public CSoccerPlayerController soccerHaveBall {
		get { return this.m_SoccerHaveBall; }
		set { this.m_SoccerHaveBall = value; }
	}
	[SerializeField]	protected string m_StrikerPrefabPath;
	[SerializeField]	protected string m_DefenderPrefabPath;
	[SerializeField]	protected string m_GoalKeeperPrefabPath;

	protected FSMManager m_FSMManager;

	#endregion

	#region MonoBehaviour Implementation

	protected override void Awake ()
	{
		base.Awake ();
		this.m_FSMManager = new FSMManager ();

		this.m_FSMManager.RegisterState ("FSMTeamIdleState", 	new FSMTeamIdleState(this));
		this.m_FSMManager.RegisterState ("FSMTeamPlayingState", new FSMTeamPlayingState(this));
		this.m_FSMManager.RegisterState ("FSMTeamAttackState", 	new FSMTeamAttackState(this));
		this.m_FSMManager.RegisterState ("FSMTeamDefendState", 	new FSMTeamDefendState(this));

		this.m_FSMManager.RegisterCondition ("IsPlaying", 		this.IsPlaying);
		this.m_FSMManager.RegisterCondition ("IsTeamHaveBall",	this.IsTeamHaveBall);
		this.m_FSMManager.RegisterCondition ("IsNearAllyGoal",	this.IsNearAllyGoal);
		this.m_FSMManager.RegisterCondition ("IsNearEnemyGoal",	this.IsNearEnemyGoal);

		this.m_FSMManager.LoadFSM (this.m_FSMTextAsset.text);
	}

	protected virtual void Update ()
	{
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
		for (int i = 0; i < this.m_StrikerSoccers.Length; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_StrikerPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_StrikerPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.SetActive (true);
			soccer.Init ();
			this.m_StrikerSoccers [i] = soccer;
			this.AddMapObject (soccer);
		}
		// DEFENDER
		this.m_DefenderSoccers = new CSoccerPlayerController[this.m_DefenderCount];
		for (int i = 0; i < this.m_DefenderSoccers.Length; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_DefenderPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_DefenderPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.SetActive (true);
			soccer.Init ();
			this.m_DefenderSoccers [i] = soccer;
			this.AddMapObject (soccer);
		}
//		// GOALKEEPER
		this.m_GoalKeeperSoccers = new CSoccerPlayerController[this.m_GoalKeeperCount];
		for (int i = 0; i < this.m_GoalKeeperSoccers.Length; i++) {
			var soccer = this.SpawnSoccerPlayer (this.m_GoalKeeperPrefabPath);
			soccer.startPoint = soccer.currentPoint = this.m_GoalPoints [i];
			var startPosition = soccer.startPoint.GetPosition ();
			soccer.SetPosition (startPosition);
			soccer.Team = this;
			soccer.SetActive (true);
			soccer.Init ();
			this.m_GoalKeeperSoccers [i] = soccer;
			this.AddMapObject (soccer);
		}
		// MINI MAP
		this.DisplayMiniMap ();
	}

	public virtual CSoccerPlayerController SpawnSoccerPlayer(string path) {
		var prefab = Resources.Load<CSoccerPlayerController> (path);
		var soccerPlayer = Instantiate (prefab);
//		soccerPlayer.transform.SetParent (this.transform);
		return soccerPlayer;
	}

	public virtual void AddMapObject(IMapObject value) {
		if (this.m_MiniMap == null)
			return;
		this.m_MiniMap.AddMapObject (value);
	}

	public virtual void DisplayMiniMap() {
		if (this.m_MiniMap == null)
			return;
		this.m_MiniMap.DisplayMiniMap ();
	}

	#endregion

	#region ITeamContext implementation

	public virtual bool IsPlaying ()
	{
		return true;
	}

	public virtual bool IsTeamHaveBall ()
	{
		for (int i = 0; i < this.m_StrikerSoccers.Length; i++) {
			if (this.m_StrikerSoccers [i].HaveBall()) {
				return true;
			}
		}
		for (int i = 0; i < this.m_DefenderSoccers.Length; i++) {
			if (this.m_DefenderSoccers [i].HaveBall()) {
				return true;
			}
		}
		for (int i = 0; i < this.m_GoalKeeperSoccers.Length; i++) {
			if (this.m_GoalKeeperSoccers [i].HaveBall()) {
				return true;
			}
		}
		return false;
	}

	public virtual bool IsNearAllyGoal() {
		var direction = this.m_AllyGoal.GetPosition () - this.m_BallController.GetPosition ();
		var radius = this.m_GoalKeeperSoccers [0].interactiveRadius;
		return direction.sqrMagnitude <= radius * radius;
	}

	public virtual bool IsNearEnemyGoal() {
		var direction = this.m_EnemyGoal.GetPosition () - this.m_BallController.GetPosition ();
		var radius = this.m_GoalKeeperSoccers [0].interactiveRadius;
		return direction.sqrMagnitude <= radius * radius;
	}

	#endregion

	#region Getter && Setter 

	public virtual CSoccerPlayerController GetSoccerNearest(CSoccerPlayerController value) {
		var nearestLength = 9999f;
		int i = 0;
		CSoccerPlayerController target = this.m_StrikerSoccers[0];
		Vector3 direction = Vector3.zero;
		for (i = 0; i < this.m_StrikerSoccers.Length; i++) {
			direction = this.m_StrikerSoccers [i].GetPosition () - value.GetPosition (); 
			if (direction.sqrMagnitude <= nearestLength) {
				target = this.m_StrikerSoccers [i];
				nearestLength = direction.sqrMagnitude; 
			}
		}
		for (i = 0; i < this.m_DefenderSoccers.Length; i++) {
			direction = this.m_DefenderSoccers [i].GetPosition () - value.GetPosition (); 
			if (direction.sqrMagnitude <= nearestLength) {
				target = this.m_DefenderSoccers [i];
				nearestLength = direction.sqrMagnitude; 
			}
		}
		for (i = 0; i < this.m_GoalKeeperSoccers.Length; i++) {
			direction = this.m_GoalKeeperSoccers [i].GetPosition () - value.GetPosition (); 
			if (direction.sqrMagnitude <= nearestLength) {
				target = this.m_GoalKeeperSoccers [i];
				nearestLength = direction.sqrMagnitude; 
			}
		}
		return target;
	}

	public virtual CSoccerPlayerController GetSoccerHaveBall() {
		int i = 0;
		for (i = 0; i < this.m_StrikerSoccers.Length; i++) {
			if (this.m_StrikerSoccers [i].HaveBall()) {
				this.m_SoccerHaveBall = this.m_StrikerSoccers [i];
				return this.m_StrikerSoccers [i];
			}
		}
		for (i = 0; i < this.m_DefenderSoccers.Length; i++) {
			if (this.m_DefenderSoccers [i].HaveBall()) {
				this.m_SoccerHaveBall = this.m_DefenderSoccers [i];
				return this.m_DefenderSoccers [i];
			}
		}
		for (i = 0; i < this.m_GoalKeeperSoccers.Length; i++) {
			if (this.m_GoalKeeperSoccers [i].HaveBall()) {
				this.m_SoccerHaveBall = this.m_GoalKeeperSoccers [i];
				return this.m_GoalKeeperSoccers [i];
			}
		}
		return null;
	}

	#endregion

}
