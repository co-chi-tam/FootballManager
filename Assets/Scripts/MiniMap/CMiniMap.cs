using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMiniMap : MonoBehaviour {

	[Header("UI")]
	[SerializeField]	protected GameObject m_MapRoot;
	[SerializeField]	protected GameObject m_MapObjPrefab;
	[SerializeField]	protected Vector2 m_Center = Vector3.zero;
	[SerializeField]	protected Vector2 m_MapScale = new Vector3 (2f, 2f);

	protected List<IMapObject> m_MapObjects;
	protected List<GameObject> m_MiniMapObjects;

	protected virtual void Awake() {
		this.m_MapObjects = new List<IMapObject> ();
		this.m_MiniMapObjects = new List<GameObject> ();
	}

	protected virtual void Update() {
		this.UpdateMiniMap ();
	}

	public virtual void AddMapObject(IMapObject value) {
		if (this.m_MapObjects.Contains (value))
			return;
		this.m_MapObjects.Add (value);
	}

	public virtual void DisplayMiniMap() {
		for (int i = 0; i < this.m_MapObjects.Count; i++) {
			var mapObj = Instantiate <GameObject> (this.m_MapObjPrefab);
			var worldPosition = this.m_MapObjects [i].GetPosition ();
			var mapPosition = new Vector3 (worldPosition.x, worldPosition.z, 0f);
			mapObj.transform.SetParent (this.m_MapRoot.transform);
			mapObj.transform.localPosition = mapPosition;
			mapObj.SetActive (true);
			this.m_MiniMapObjects.Add (mapObj);
		}
		this.m_MapObjPrefab.SetActive (false);
	}

	public virtual void UpdateMiniMap() {
		for (int i = 0; i < this.m_MiniMapObjects.Count; i++) {
			var mapObj = this.m_MiniMapObjects[i];
			var worldPosition = this.m_MapObjects [i].GetPosition ();
			var mapPosition = new Vector3 (
				(worldPosition.x + this.m_Center.x) * this.m_MapScale.x, 
				(worldPosition.z + this.m_Center.y) * this.m_MapScale.y, 
				0f);
			mapObj.transform.localPosition = mapPosition;
		}
	}

}
