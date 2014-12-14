using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingTester : MonoBehaviour {
	public int parameter = 0;

	[Range(2, 50)]
	public int grid_width = 10;

	[Range(2, 50)]
	public int grid_length = 10;
	
	[Range(2.0f, 100.0f)]
	public float max_base_width = 10.0f;

	[Range(2.0f, 100.0f)]
	public float max_base_length = 10.0f;

	[Range(3.0f, 100.0f)]
	public float average_height = 15.0f;

	[Range(0.0f, 1.0f)]
	public float noise = 0.0f;

	[Range(0.0f, 1.0f)]
	public float style_A = 0.5f;

	[Range(0.0f, 1.0f)]
	public float style_B = 0.5f;

	private Mesh mesh = null;

	private void Create()
	{
		Random.seed = 0;

		mesh = new Mesh();
		CombineInstance[] combine = new CombineInstance[grid_width * grid_length];
		for (var j = 0; j < grid_length; ++j) {
			for (var i = 0; i < grid_width; ++i) {
				int index = i + j * grid_width;
				combine[index].mesh = Building.Generate(average_height, max_base_width, max_base_length, noise, new Vector2(style_A, style_B));
				combine[index].transform = Extensions.TranslationMatrix(max_base_width * i, 0.0f, max_base_length * j);
			}
		}
		mesh.CombineMeshes(combine);
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
