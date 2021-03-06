using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldDeformation : MonoBehaviour {
    private WorldDeformationParameters parameters = null;

    void Start () {
        parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();
    }

    private void Update() {
        Shader.SetGlobalFloat("_vertexTranslation", parameters.vertex_translation);
        Shader.SetGlobalFloat("_vertexLatTranslation", parameters.vertex_lat_translation);
        Shader.SetGlobalFloat("_vertexDeformation", parameters.vertex_deformation);
    }
}
