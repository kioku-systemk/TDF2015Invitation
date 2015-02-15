using UnityEngine;
using System.Collections;

public class WorldDeformationText : MonoBehaviour {
	private GameObject parameters = null;

// 	private Vector3 position;
// 	private Quaternion rotation;

	private GUIStyle debugFont;


	void Start () {
		parameters = GameObject.Find("GlobalParameters");

// 		position = transform.position;
// 		rotation = transform.localRotation;

		debugFont = new GUIStyle();
		debugFont.fontSize = 24;
		debugFont.normal.textColor = Color.cyan;
	}

	private void LateUpdate () {
		var m = GetComponent<Renderer>().sharedMaterial;
		var translation = m.GetFloat("_vertexTranslation");
		var lat_translation = m.GetFloat("_vertexLatTranslation");

		var newPosition = new Vector3(translation, 0.0f, lat_translation);
		transform.position = newPosition;
	}

	void OnGUI() {
		GUI.Label(new Rect(100f, 150f, 200f, 100f), "Position: " + transform.position.ToString(), debugFont);
	}
}
