using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Environment : MonoBehaviour {
	public Color ambientLight = Color.black;
	public Color skyColor = Color.white;
	public Color fogColor = Color.white;

	public GameObject sky = null;

	void Start () {
	}

	void Update () {
		RenderSettings.ambientLight = ambientLight;
		RenderSettings.fog = true;
		RenderSettings.fogColor = fogColor;

		var m = sky.GetComponent<MeshRenderer>().sharedMaterial;
		m.SetColor("_fogColor", fogColor);
		m.SetColor("_topColor", skyColor);
	}
}
