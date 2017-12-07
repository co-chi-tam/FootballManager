using UnityEngine;
using UnityEditor;

public class CSoccerCreateAsset
{
	[MenuItem("Assets/Create/Soccer Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<CSoccerData> ();
	}
}