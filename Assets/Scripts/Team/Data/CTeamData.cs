using System;
using UnityEngine;

public class CTeamData : CObjectData {

	// STRIKER
	public string strikerFSMPath;
	public CSoccerData[] strikerSlots;

	// DEFENDER
	public string defenderFSMPath;
	public CSoccerData[] defenderSlots;

	// GOAL
	public string goalKeeperFSMPath;
	public CSoccerData[] goalKeeperSlots;

	public CTeamData ()
	{
		// STRIKER
		this.strikerFSMPath = string.Empty;

		// DEFENDER
		this.defenderFSMPath = string.Empty;

		// GOAL KEEPER
		this.goalKeeperFSMPath = string.Empty;
	}

}

