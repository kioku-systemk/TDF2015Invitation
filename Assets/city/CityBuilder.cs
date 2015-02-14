using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class CityBuilder : MonoBehaviour {

	public GameObject BuildingPatch = null;

	[Range(1, 100)]
	public int city_width = 10;

	[Range(1, 200)]
	public int city_length = 10;

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

	private float width = 1.0f;
	private float length = 1.0f;

	private int hash = 0;

	public override int GetHashCode() {
		var hash = 0;
		hash = (city_width.GetHashCode() + hash).GetHashCode();
		hash = (city_length.GetHashCode() + hash).GetHashCode();
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

#if UNITY_EDITOR
    [MenuItem("City/Build Mesh")]
    public static void BuildMesh()
    {
        var obj = GameObject.Find("City Test");
        if (obj != null)
        {
            var builder = obj.GetComponent<CityBuilder>();
            if (builder != null)
            {
                builder.Create();
            }
        }
    }

	private void Create()
    {
        hash = GetHashCode();

		List<Mesh> buildingsMeshes = new List<Mesh>();
		List<Mesh> billboardsMeshes = new List<Mesh>();
		List<Mesh> lightStreakMeshes = new List<Mesh>();
		City.Size size = City.GenerateCity(buildingsMeshes, billboardsMeshes, lightStreakMeshes,
		                                   city_width, city_length,
		                                   20, 20,
		                                   block_width, block_length, street_width,
		                                   large_block_width, large_block_length, large_street_width,
		                                   cell_width, cell_length, average_height,
		                                   noise, style_A, style_B);
		width = size.width;
		length = size.length;

 		ApplyMeshes(BuildingPatch, buildingsMeshes, billboardsMeshes, lightStreakMeshes);
	}

	private void ApplyMeshes(GameObject patchObject,
							 List<Mesh> buildingsMeshes,
							 List<Mesh> billboadsMeshes,
							 List<Mesh> lightStreakMeshes) {
		var numberOfMeshes = buildingsMeshes.Count;

		// Destroy over numbered children
		for (var i = transform.childCount - 1; i >= numberOfMeshes; --i) {
			DestroyImmediate(transform.GetChild(i).gameObject);
		}

		// Add children if necessary
		for (var i = transform.childCount; i < numberOfMeshes; ++i) {
			var patch = Instantiate(patchObject) as GameObject;
			patch.transform.parent = this.transform;
		}

		// At this point, we have the right number of children
		for (var i = 0; i < numberOfMeshes; ++i) {
			var child = transform.GetChild(i);
			var meshFilter = child.GetComponent<MeshFilter>();

			// Hack to avoid culling of shaded vertices;
			// FIXME: deduce bounding box from deformation.
            buildingsMeshes[i].bounds = new Bounds(Vector3.zero, 2000.0f * Vector3.one);
            billboadsMeshes[i].bounds = new Bounds(Vector3.zero, 2000.0f * Vector3.one);
            lightStreakMeshes[i].bounds = new Bounds(Vector3.zero, 2000.0f * Vector3.one);

            string path_building = "Assets/Meshes/City/Building[" + i + "].asset";
            string path_billboard = "Assets/Meshes/City/Billboard[" + i + "].asset";
            string path_lightstreak = "Assets/Meshes/City/LightStreak[" + i + "].asset";
            AssetDatabase.CreateAsset(buildingsMeshes[i], path_building);
            AssetDatabase.CreateAsset(billboadsMeshes[i], path_billboard);
            AssetDatabase.CreateAsset(lightStreakMeshes[i], path_lightstreak);
            meshFilter.sharedMesh = AssetDatabase.LoadAssetAtPath(path_building, typeof(Mesh)) as Mesh;

			var m1 = child.GetComponent<Renderer>().sharedMaterial;
			m1.SetFloat("_maxWidth",	width);
			m1.SetFloat("_maxLength",	length);


			var subChild = child.GetChild(0);
			var subMeshFilter = subChild.GetComponent<MeshFilter>();
            subMeshFilter.sharedMesh = AssetDatabase.LoadAssetAtPath(path_billboard, typeof(Mesh)) as Mesh;

			var m2 = subChild.GetComponent<Renderer>().sharedMaterial;
			m2.SetFloat("_maxWidth",	width);
			m2.SetFloat("_maxLength",	length);
		}
	}
#endif // UNITY_EDITOR

    private void Awake()
    {
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        // 		var material = GetComponent<Renderer>().sharedMaterial;
        // 		material.SetFloat("_time", Time.time);
        // 		material.SetFloat("_maxWidth", width);
        // 		material.SetFloat("_maxLength", length);
    }
}
