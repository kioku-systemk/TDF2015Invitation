using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class WorldDeformationParameters : MonoBehaviour {
	public float vertex_translation = 0.0f;

	[Range(0.0f, 1.0f)]
	public float vertex_deformation = 0.0f;
}
