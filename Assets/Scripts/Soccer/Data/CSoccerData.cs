using System;
using UnityEngine;

public class CSoccerData : CObjectData {

	public int starPoint;
	public float moveSpeed;
	public float runSpeed;
	public float potentialPoint;
	public float tackleBallValue;
	public float interactiveRadius;
	public float timePerActive;

	public CSoccerData ()
	{
		this.starPoint 			= 1;
		this.moveSpeed 			= 3.0f;
		this.runSpeed 			= 6.0f;
		this.potentialPoint 	= 2.0f;
		this.tackleBallValue 	= 1.0f;
		this.interactiveRadius 	= 7.0f;
		this.timePerActive 		= 0.5f;
	}

}
