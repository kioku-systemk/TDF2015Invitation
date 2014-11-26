using UnityEngine;
using System.Collections;

public class Scene2code : MonoBehaviour {

	private float m_starttime = 0;
	private float m_time = 0;

	// Use this for initialization
	void Start () {
		Debug.Log("scene2 start:" + Time.time);
		m_starttime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		m_time = Time.time;
	}

	void OnGUI() {
		float sceneTime = m_time - m_starttime;
		GUI.Label (new Rect (0, 0, 200, 20), "Scene2 : " + sceneTime.ToString("F3") + "[sec]");
	}
}
