using System;
using UnityEngine;

public interface IBallControlObject {

	Vector3 GetBallWorldPosition();
	Vector3 GetPosition();

	float GetBallValue();

	void SetBall(CBallController ball);

	CTeamController GetTeam();

}
