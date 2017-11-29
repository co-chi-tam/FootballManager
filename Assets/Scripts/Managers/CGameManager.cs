using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSingleton;

public class CGameManager : CMonoSingleton<CGameManager> {

	protected override void Awake ()
	{
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);
	}

}
