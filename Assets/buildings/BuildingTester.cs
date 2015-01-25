using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class BuildingTester : MonoBehaviour {

	public GameObject patchObject = null;

	[Range(1, 100)]
	public int city_width = 10;

	[Range(1, 100)]
	public int city_length = 10;

	/*
	[Range(1, 20)]
	public int ideal_patch_width = 10;
	
	[Range(1, 20)]
	public int ideal_patch_length = 10;
	*/

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
		//hash = (ideal_patch_width.GetHashCode() + hash).GetHashCode();
		//hash = (ideal_patch_length.GetHashCode() + hash).GetHashCode();
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


		List<Mesh> meshes = new List<Mesh>();
		City.Size size = City.GenerateCity(meshes,
		                                   city_width, city_length,
		                                   20, 20, //ideal_patch_width, ideal_patch_length,
		                                   block_width, block_length, street_width,
		                                   large_block_width, large_block_length, large_street_width,
		                                   cell_width, cell_length, average_height,
		                                   noise, style_A, style_B);
		width = size.width;
		length = size.length;
		GetComponent<MeshFilter>().mesh = null;//meshes[0];

		var numberOfMeshes = meshes.Count;

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
		for (var i = 0; i < numberOfMeshes; ++i)
		{
			var meshFilter = transform.GetChild(i).GetComponent<MeshFilter>();

			// Hack to avoid culling of shaded vertices
			meshFilter.sharedMesh = meshes[i];
			meshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, float.PositiveInfinity * Vector3.one);
		}
	}

	private void Awake () {
		Create ();
	}

	private void OnEnable() {
		Create ();
	}

	private void Update () {
		Create ();
		var material = GetComponent<Renderer>().sharedMaterial;
		material.SetFloat("_time", Time.time);
		material.SetFloat("_maxWidth", width);
		material.SetFloat("_maxLength", length);
	}
}
