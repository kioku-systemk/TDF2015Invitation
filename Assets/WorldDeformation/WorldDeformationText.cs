using UnityEngine;
using System.Collections;

public class WorldDeformationText : MonoBehaviour {
	WorldDeformationParameters parameters;

// 	private Vector3 position;
// 	private Quaternion rotation;

	private GUIStyle debugFont;


	void Start ()
	{
		parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();

		debugFont = new GUIStyle();
		debugFont.fontSize = 24;
		debugFont.normal.textColor = Color.cyan;
	}

	private void LateUpdate ()
	{
		var translation = parameters.vertex_translation;
		var lat_translation = parameters.vertex_lat_translation;

		var newPosition = new Vector3(translation, 0.0f, lat_translation);
		transform.position = newPosition;
	}

	void OnGUI() {
		GUI.Label(new Rect(100f, 150f, 200f, 100f), "Position: " + transform.position.ToString(), debugFont);
	}
}
