using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CResetGame : MonoBehaviour {

	protected virtual void OnTriggerEnter(Collider coll) {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

}
