using UnityEngine;

[ExecuteInEditMode]
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

	[Range(1, 50)]
	public int large_block_width = 6;

	[Range(1, 50)]
	public int large_block_length = 12;

	[Range(2.0f, 50.0f)]
	public float large_street_width = 8.0f;

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

	private int hash = 0;

	public override int GetHashCode() {
		var hash = 0;
		hash = (patch_width.GetHashCode() + hash).GetHashCode();
		hash = (patch_length.GetHashCode() + hash).GetHashCode();
		hash = (block_width.GetHashCode() + hash).GetHashCode();
		hash = (block_length.GetHashCode() + hash).GetHashCode();
		hash = (street_width.GetHashCode() + hash).GetHashCode();
		hash = (large_block_width.GetHashCode() + hash).GetHashCode();
		hash = (large_block_length.GetHashCode() + hash).GetHashCode();
		hash = (large_street_width.GetHashCode() + hash).GetHashCode();
		hash = (cell_width.GetHashCode() + hash).GetHashCode();
		hash = (cell_length.GetHashCode() + hash).GetHashCode();
		hash = (average_height.GetHashCode() + hash).GetHashCode();
		hash = (noise.GetHashCode() + hash).GetHashCode();
		hash = (style_A.GetHashCode() + hash).GetHashCode();
		hash = (style_B.GetHashCode() + hash).GetHashCode();
		return hash;
	}


	private void Create() {
		var newHash = GetHashCode();
		if (hash == newHash) {
			return;
 		}
		hash = newHash;

		mesh = new Mesh();
		City.GeneratePatch(mesh,
		                   patch_width, patch_length,
		                   block_width, block_length, street_width,
		                   large_block_width, large_block_length, large_street_width,
		                   cell_width, cell_length, average_height,
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
