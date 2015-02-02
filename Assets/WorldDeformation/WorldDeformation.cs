using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldDeformation : MonoBehaviour {
	private WorldDeformationParameters parameters = null;

	void Start () {
		parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();
	}

	private void Update(){
		var m = GetComponent<Renderer>().sharedMaterial;
		m.SetFloat("_vertexDeformation", parameters.vertex_deformation);
		m.SetFloat("_waveHeight", parameters.wave_height);
		m.SetFloat("_xFreq", parameters.x_freq);
		m.SetFloat("_yFreq", parameters.y_freq);
		m.SetFloat("_speed", parameters.speed);

		m.SetFloat("_effectEdgeGlow",		parameters.effect_edge_glow);
		m.SetFloat("_effect3Intensity",		parameters.effect3_intensity);
		m.SetFloat("_effect4Intensity",		parameters.effect4_intensity);
	}
}
