using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class WorldDeformationParameters : MonoBehaviour {
	[Range(0.0f, 1.0f)]
	public float vertex_deformation = 0.0f;

	[Range(0.0f, 20.0f)]
	public float wave_height = 0.0f;

	[Range(0.0f, 0.1f)]
	public float x_freq = 0.02f;

	[Range(0.0f, 0.1f)]
	public float y_freq = 0.02f;

	[Range(0.0f, 10.0f)]
	public float speed = 1.0f;

	// ---8<------------------------------------------------------------------

	[Range(0.0f, 1.0f)]
	public float effect_edge_glow = 0.0f;

	[Range(0.0f, 1.0f)]
	public float effect3_intensity = 0.0f;

	[Range(0.0f, 1.0f)]
	public float effect4_intensity = 0.0f;
}
