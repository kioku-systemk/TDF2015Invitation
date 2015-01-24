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
		m.SetFloat("_waveHeight", parameters.wave_height);
		m.SetFloat("_xFreq", parameters.x_freq);
		m.SetFloat("_yFreq", parameters.y_freq);
		m.SetFloat("_speed", parameters.speed);
	}
}
