using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingTester : MonoBehaviour {
	[Range(1, 50)]
	public int patch_width = 10;

	[Range(1, 50)]
	public int patch_length = 10;

	[Range(1, 50)]
	public int block_width = 2;

	[Range(1, 50)]
	public int block_length = 4;

	[Range(2.0f, 50.0f)]
	public float street_width = 8.0f;

	[Range(2.0f, 100.0f)]
	public float cell_width = 10.0f;

	[Range(2.0f, 100.0f)]
	public float cell_length = 10.0f;

	[Range(3.0f, 100.0f)]
	public float average_height = 15.0f;

	[Range(0.0f, 1.0f)]
	public float noise = 0.0f;

	[Range(0.0f, 1.0f)]
	public float style_A = 0.5f;

	[Range(0.0f, 1.0f)]
	public float style_B = 0.5f;

	private Mesh mesh = null;

	private void Create() {
		mesh = new Mesh();
		City.GeneratePatch(mesh,
		                   patch_width, patch_length,
		                   block_width, block_length,
		                   street_width, cell_width, cell_length, average_height,
		                   noise, style_A, style_B);
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void Awake () {
		Create ();
	}

	private void OnEnable() {
		Create ();
	}

	private void Update () {
		Create ();
	}
}
