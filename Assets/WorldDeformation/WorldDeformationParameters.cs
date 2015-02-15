using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class WorldDeformationParameters : MonoBehaviour {
	public float vertex_translation = 0.0f;
	public float vertex_lat_translation = 0.0f;

	[Range(0.0f, 1.0f)]
	public float vertex_deformation = 0.0f;

	// ---8<------------------------------------------------------------------

	private GUIStyle debugFont;

	void Start () {
		debugFont = new GUIStyle();

		debugFont.fontSize = 24;
		debugFont.normal.textColor = Color.yellow;
	}

	void OnGUI() {
		GUI.Label(new Rect(100f, 100f, 200f, 100f), Time.time.ToString("F2"), debugFont);
	}
}
