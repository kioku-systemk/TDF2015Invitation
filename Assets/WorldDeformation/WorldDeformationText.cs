using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldDeformationText : MonoBehaviour {
	WorldDeformationParameters parameters;

	private Vector3 position;
// 	private Quaternion rotation;
	private float maxWidth = 518.0f;
	private float maxLength = 1409.0f;

	void Start ()
	{
		parameters = GameObject.Find("GlobalParameters").GetComponent<WorldDeformationParameters>();
		position = transform.localPosition;
	}

	private const float Tau = 6.2831853f;

	// Torus
	Vector3 P_torus(float px, float py, float rate)
	{
		float circumpherence1 = maxWidth * Mathf.Lerp(100.0f, 1.0f, Mathf.Pow(Mathf.Clamp(2.0f * rate, 0.0f, 1.0f), 0.1f));
		float circumpherence2 = maxLength * Mathf.Lerp(100.0f, 1.0f, Mathf.Pow(Mathf.Clamp(2.0f * rate - 1.0f, 0.0f, 1.0f), 0.1f));

		float theta = Tau * px / circumpherence1;
		float phi = Tau * py / circumpherence2;
		float r1 = circumpherence1 / Tau;
		float r2 = circumpherence2 / Tau;

		float x = (r1 * -Mathf.Sin(theta) + r2) * -Mathf.Cos(phi) + r2;
		float y = r1 * -Mathf.Cos(theta) + r1;
		float z = (r1 * -Mathf.Sin(theta) + r2) * Mathf.Sin(phi);

		return new Vector3(x, y, z);
	}

	Matrix4x4 T(float px, float py, float rate) {
		Vector3 p = P_torus(px, py, rate);

		Vector3 ux = (P_torus(px + 1.0f, py, rate) - p).normalized;
		Vector3 uz = (P_torus(px, py + 1.0f, rate) - p).normalized;
		Vector3 uy = Vector3.Cross(uz, ux);

		Matrix4x4 t = new Matrix4x4();
		t.SetColumn(0, new Vector4(ux.x, ux.y, ux.z, 0.0f));
		t.SetColumn(1, new Vector4(uy.x, uy.y, uy.z, 0.0f));
		t.SetColumn(2, new Vector4(uz.x, uz.y, uz.z, 0.0f));
		t.SetColumn(3, new Vector4(p.x, p.y, p.z, 1.0f));
		return t;
	}

	private void Update()
	{
// 		var translation = parameters.vertex_translation;
// 		var lat_translation = parameters.vertex_lat_translation;
// 		var deformation = parameters.vertex_deformation;

// 		var px = position.x + (0.5f * maxWidth + lat_translation) % maxWidth - 0.5f * maxWidth;
// 		var pz = position.z + (0.25f * maxLength + translation) % maxLength;
// 		var t = T(px, pz, deformation);

// 		var newPosition = t.MultiplyPoint3x4(position);
// 		transform.position = newPosition;
	}
}
