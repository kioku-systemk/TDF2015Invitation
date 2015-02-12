using UnityEngine;
using System.Collections;

public class WorldDeformationText : MonoBehaviour {
	private WorldDeformationParameters parameters = null;

	private Vector3 position;
	private Quaternion rotation;

	void Start () {
		parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();

		position = transform.localPosition;
		rotation = transform.localRotation;

		Debug.Log("Text original position : {" + position.x + ", " + position.y + ", " + position.z + "}");
	}

	private void Update () {
		transform.localPosition.Set(position.x,
									position.y + parameters.vertex_translation,
									position.z);

		Debug.Log("Translation: " + parameters.vertex_translation.ToString() + "\n" +
				  //"Deformation: " + parameters.vertex_deformation.ToString() + "\n" +
				  "Position: " + transform.localPosition.ToString());
	}
}
