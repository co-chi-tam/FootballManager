using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSingleton;

public class CGameManager : CMonoSingleton<CGameManager> {

	#region Fields

	[Header("Ball")]
	[SerializeField]	protected Transform m_BallPosition;
	[SerializeField]	protected CBallController[] m_BallPrefabs;

	[Header ("Teams")]
	[SerializeField]	protected Transform m_Team1;
	[SerializeField]	protected Transform m_Team2;
	[SerializeField]	protected CTeamController m_Team5x5x1_Prefab;
	[SerializeField]	protected CTeamController m_Team2x3x5x1_Prefab;

	#endregion

	protected override void Awake ()
	{
		base.Awake ();
//		DontDestroyOnLoad (this.gameObject);
	}



}
