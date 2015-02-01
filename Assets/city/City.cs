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

				Building.BillboardDesc billboard = Building.BillboardDesc.None;
				if (i == 0) { billboard = Building.BillboardDesc.Left; }
				else if (j == 0) { billboard = Building.BillboardDesc.Front; }
				else if (i == blockWidthInCells - 1) { billboard = Building.BillboardDesc.Right; }
				else if (j == blockLengthInCells - 1) { billboard = Building.BillboardDesc.Back; }

				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB), billboard);
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
			var j1 = j + j0;
			var yStreets = j1 < 0 ? (j1 + 1) / blockLengthInCells - 1 : j1 / blockLengthInCells;
			var yLargeStreets = j1 < 0 ? (j1 + 1) / largeBlockLengthInCells - 1 : j1 / largeBlockLengthInCells;
			var yStreetOffset = yStreets * streetWidth + yLargeStreets * largeStreetWidth;

			for (var i = 0; i < patchWidthInCells; ++i) {
				var i1 = i + i0;
				var xStreets = i1 < 0 ? (i1 + 1) / blockWidthInCells - 1 : i1 / blockWidthInCells;
				var xLargeStreets = i1 < 0 ? (i1 + 1) / largeBlockWidthInCells - 1 : i1 / largeBlockWidthInCells;
				var xStreetOffset = xStreets * streetWidth + xLargeStreets * largeStreetWidth;

				var x = cellWidth * i1 + xStreetOffset;
				var y = cellLength * j1 + yStreetOffset;

				Building.BillboardDesc billboard = Building.BillboardDesc.None;
				if (((i + i0) % largeBlockWidthInCells) == 0) { billboard = Building.BillboardDesc.Left; }
				else if (((i + i0 + 1) % largeBlockWidthInCells) == 0) { billboard = Building.BillboardDesc.Right; }
				else if (((j + j0) % largeBlockLengthInCells) == 0) { billboard = Building.BillboardDesc.Front; }
				else if (((j + j0 + 1) % largeBlockLengthInCells) == 0) { billboard = Building.BillboardDesc.Back; }

				Random.seed = Hash.Get(Hash.Get(i) + j);
				int index = i + j * patchWidthInCells;
				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB), billboard);
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

				GeneratePatch(patch, i - cityWidthInCells / 2, j - cityLengthInCells / 2, patchWidth, patchLength,
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
}
