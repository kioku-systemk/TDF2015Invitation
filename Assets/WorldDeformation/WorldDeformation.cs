using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldDeformation : MonoBehaviour {
	private WorldDeformationParameters parameters = null;

	void Start () {
		parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();
	}

	private void Update() {
		var m = GetComponent<Renderer>().sharedMaterial;
		m.SetFloat("_vertexTranslation",	parameters.vertex_translation);
		m.SetFloat("_vertexDeformation",	parameters.vertex_deformation);

		//Debug.Log("_vertexTranslation " + parameters.vertex_translation);
		//Debug.Log("_vertexDeformation " + parameters.vertex_deformation);
	}
}
