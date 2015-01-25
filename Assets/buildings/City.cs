using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class City {

	public struct Size
	{
		public float width;
		public float length;

		public Size(float w, float l)
		{
			width = w;
			length = l;
		}
	};
	
	public static Size GetCitySize(int widthInCells,
	                               int lengthInCells,
	                               int blockWidthInCells,
	                               int blockLengthInCells,
	                               float streetWidth,
	                               int largeBlockWidthInCells,
	                               int largeBlockLengthInCells,
	                               float largeStreetWidth,
	                               float cellWidth,
	                               float cellLength)
	{
		var yStreets = lengthInCells / blockLengthInCells;
		var yLargeStreets = lengthInCells / largeBlockLengthInCells;
		var yStreetOffset = yStreets * streetWidth + yLargeStreets * largeStreetWidth;
		
		var xStreets = widthInCells / blockWidthInCells;
		var xLargeStreets = widthInCells / largeBlockWidthInCells;
		var xStreetOffset = xStreets * streetWidth + xLargeStreets * largeStreetWidth;
		
		var width = cellWidth * widthInCells + xStreetOffset;
		var length = cellLength * lengthInCells + yStreetOffset;
		
		return new Size(width, length);
	}

	public static void GenerateBlock(Mesh block,
	                                 int blockWidthInCells,
	                                 int blockLengthInCells,
	                                 float cellWidth,
	                                 float cellLength,
	                                 float buildingAverageHeight,
	                                 float noise,
	                                 float styleA,
	                                 float styleB) {
		CombineInstance[] combine = new CombineInstance[blockWidthInCells * blockLengthInCells];

		for (var j = 0; j < blockLengthInCells; ++j) {
			for (var i = 0; i < blockWidthInCells; ++i) {
				Random.seed = Hash.Get(Hash.Get(i) + j);
				int index = i + j * blockWidthInCells;
				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB));
				combine[index].transform = Extensions.TranslationMatrix(cellWidth * i, 0.0f, cellLength * j);
			}
		}
		block.CombineMeshes(combine);
	}

	public static Mesh GenerateBlock(int blockWidthInCells,
	                                 int blockLengthInCells,
	                                 float cellWidth,
	                                 float cellLength,
	                                 float buildingAverageHeight,
	                                 float noise,
	                                 float styleA,
	                                 float styleB) {
		Mesh block = new Mesh();
		GenerateBlock(block, blockWidthInCells, blockLengthInCells, cellWidth, cellLength, buildingAverageHeight, noise, styleA, styleB);
		return block;
	}

	public static void GeneratePatch(Mesh patch,
	                                 int i0,
	                                 int j0,
	                                 int patchWidthInCells,
	                                 int patchLengthInCells,
	                                 int blockWidthInCells,
	                                 int blockLengthInCells,
	                                 float streetWidth,
	                                 int largeBlockWidthInCells,
	                                 int largeBlockLengthInCells,
	                                 float largeStreetWidth,
	                                 float cellWidth,
	                                 float cellLength,
	                                 float buildingAverageHeight,
	                                 float noise,
	                                 float styleA,
	                                 float styleB) {
		Debug.Log("Generate patch [" + patchWidthInCells + " x " + patchLengthInCells +"]");
		CombineInstance[] combine = new CombineInstance[patchWidthInCells * patchLengthInCells];

		for (var j = 0; j < patchLengthInCells; ++j) {
			var yStreets = (j + j0) / blockLengthInCells;
			var yLargeStreets = (j + j0) / largeBlockLengthInCells;
			var yStreetOffset = yStreets * streetWidth + yLargeStreets * largeStreetWidth;

			for (var i = 0; i < patchWidthInCells; ++i) {
				var xStreets = (i + i0) / blockWidthInCells;
				var xLargeStreets = (i + i0) / largeBlockWidthInCells;
				var xStreetOffset = xStreets * streetWidth + xLargeStreets * largeStreetWidth;

				var x = cellWidth * (i + i0) + xStreetOffset;
				var y = cellLength * (j + j0) + yStreetOffset;

				Random.seed = Hash.Get(Hash.Get(i) + j);
				int index = i + j * patchWidthInCells;
				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB));
				combine[index].transform = Extensions.TranslationMatrix(x, 0.0f, y); // ParametricFunction.T(x, y, maxX, maxY);
			}
		}
		patch.CombineMeshes(combine);
	}

	public static Size GenerateCity(List<Mesh> meshes,
									int cityWidthInCells,
	                                int cityLengthInCells,
	                                int idealPatchWidthInCells,
	                                int idealPatchLengthInCells,
	                                int blockWidthInCells,
	                                int blockLengthInCells,
	                                float streetWidth,
	                                int largeBlockWidthInCells,
	                                int largeBlockLengthInCells,
	                                float largeStreetWidth,
	                                float cellWidth,
	                                float cellLength,
	                                float buildingAverageHeight,
	                                float noise,
	                                float styleA,
	                                float styleB)
	{
		Debug.Log("Generate city [" + cityWidthInCells + " x " + cityLengthInCells +"]");

		for (var j = 0; j < cityLengthInCells; j += idealPatchLengthInCells) {
			for (var i = 0; i < cityWidthInCells; i += idealPatchWidthInCells) {
				var patch = new Mesh();
				var patchWidth = Mathf.Min(idealPatchWidthInCells, cityWidthInCells - i);
				var patchLength = Mathf.Min(idealPatchLengthInCells, cityLengthInCells - j);

				GeneratePatch(patch, i, j, patchWidth, patchLength,
				              blockWidthInCells, blockLengthInCells, streetWidth,
				              largeBlockWidthInCells, largeBlockLengthInCells, largeStreetWidth,
				              cellWidth, cellLength,
				              buildingAverageHeight, noise, styleA, styleB);
				meshes.Add(patch);
			}
		}

		Debug.Log("Generated " + meshes.Count + (meshes.Count > 1 ? " patches" : "patch"));

		
		return GetCitySize(cityWidthInCells, cityLengthInCells,
		                   blockWidthInCells, blockLengthInCells, streetWidth,
		                   largeBlockWidthInCells, largeBlockLengthInCells, largeStreetWidth,
		                   cellWidth, cellLength);
	}

	private static class ParametricFunction {
		public static float X(float u, float v) { return u; }
		public static float Y(float u, float v) { return 20.0f * Mathf.Sin(0.02f* u) * Mathf.Sin(0.04f * v); }
		public static float Z(float u, float v) { return v; }

		public static Vector4 P(float u, float v) { return new Vector4(X(u, v), Y(u, v), Z(u, v), 1.0f); }

		public static Matrix4x4 T(float u, float v, float maxX, float maxY) {
			var p = P(u, v);
			var ux = (P(u + 0.001f, v) - p).normalized;
			var uz = (P(u, v + 0.001f) - p).normalized;
			var uy = Vector3.Cross(uz, ux);
			var m = new Matrix4x4();
			m.SetColumn(0, ux);
			m.SetColumn(1, uy);
			m.SetColumn(2, uz);
			m.SetColumn(3, p);
			return m;
		}
	}
}
