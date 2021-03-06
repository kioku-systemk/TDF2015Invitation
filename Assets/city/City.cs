﻿using UnityEngine;
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
	                               float cellLength) {
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

	public static float GridToPosition(int i, int i0,
									   int blockWidthInCells, float streetWidth,
									   int largeBlockWidthInCells, float largeStreetWidth,
									   float cellWidth) {
		var i1 = i + i0;
		var streets = i1 < 0 ? (i1 + 1) / blockWidthInCells - 1 : i1 / blockWidthInCells;
		var largeStreets = i1 < 0 ? (i1 + 1) / largeBlockWidthInCells - 1 : i1 / largeBlockWidthInCells;
		var streetOffset = streets * streetWidth + largeStreets * largeStreetWidth;

		return cellWidth * i1 + streetOffset;
	}

	public static void GeneratePatch(Mesh patchShader1,
									 Mesh patchShader2,
									 Mesh patchShader3,
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
		CombineInstance[] combine1 = new CombineInstance[2 * patchWidthInCells * patchLengthInCells];
		CombineInstance[] combine2 = new CombineInstance[patchWidthInCells * patchLengthInCells];
		CombineInstance[] combine3 = new CombineInstance[patchWidthInCells * patchLengthInCells];

		for (var j = 0; j < patchLengthInCells; ++j) {
			for (var i = 0; i < patchWidthInCells; ++i) {
				Building.BillboardDesc billboard = Building.BillboardDesc.None;
				if (((i + i0) % largeBlockWidthInCells) == 0) { billboard = Building.BillboardDesc.Left; }
				else if (((i + i0 + 1) % largeBlockWidthInCells) == 0) { billboard = Building.BillboardDesc.Right; }
				else if (((j + j0) % largeBlockLengthInCells) == 0) { billboard = Building.BillboardDesc.Front; }
				else if (((j + j0 + 1) % largeBlockLengthInCells) == 0) { billboard = Building.BillboardDesc.Back; }

				var seed = Hash.Get(Hash.Get(i) + j);
				Random.seed = seed;
				Vector3 tag = new Vector3(Mathf.Abs((float)(i + i0)) / 256.0f,
										  Mathf.Abs((float)(j + j0)) / 256.0f,
										  (float)seed / 256.0f);
				var x = GridToPosition(i, i0,
									   blockWidthInCells, streetWidth,
									   largeBlockWidthInCells, largeStreetWidth,
									   cellWidth);
				var y = GridToPosition(j, j0,
									   blockLengthInCells, streetWidth,
									   largeBlockLengthInCells, largeStreetWidth,
									   cellLength);

				int index = i + j * patchWidthInCells;

				var meshes = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB), tag, billboard);
				combine1[index * 2].mesh = meshes.Building;
				combine1[index * 2].transform = Extensions.TranslationMatrix(x, 0.0f, y);
				combine2[index].mesh = meshes.BillboardAd;
				combine2[index].transform = Extensions.TranslationMatrix(x, 0.0f, y);
				combine3[index].mesh = meshes.LightStreak;
				combine3[index].transform = Extensions.TranslationMatrix(x, 0.0f, y);
				var x1 = GridToPosition(i + 1, i0,
										blockWidthInCells, streetWidth,
										largeBlockWidthInCells, largeStreetWidth,
										cellWidth);
				var y1 = GridToPosition(j + 1, j0,
										blockLengthInCells, streetWidth,
										largeBlockLengthInCells, largeStreetWidth,
										cellLength);
				combine1[index * 2 + 1].mesh = Cuboid.Create(new Vector3(x1 - x, 0.0f, y1 - y), Cuboid.Face.top);
				combine1[index * 2 + 1].transform = Extensions.TranslationMatrix(0.5f * (x + x1), 0.0f, 0.5f * (y + y1));
			}
		}
		patchShader1.CombineMeshes(combine1);
		patchShader2.CombineMeshes(combine2);
		patchShader3.CombineMeshes(combine3);
	}

	public static Size GenerateCity(List<Mesh> meshesShader1,
									List<Mesh> meshesShader2,
									List<Mesh> meshesShader3,
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
	                                float styleB) {
		Debug.Log("Generate city [" + cityWidthInCells + " x " + cityLengthInCells +"]");

		for (var j = 0; j < cityLengthInCells; j += idealPatchLengthInCells) {
			for (var i = 0; i < cityWidthInCells; i += idealPatchWidthInCells) {
				var patchShader1 = new Mesh();
				var patchShader2 = new Mesh();
				var patchShader3 = new Mesh();
				var patchWidth = Mathf.Min(idealPatchWidthInCells, cityWidthInCells - i);
				var patchLength = Mathf.Min(idealPatchLengthInCells, cityLengthInCells - j);

				GeneratePatch(patchShader1, patchShader2, patchShader3, i - cityWidthInCells / 2, j - cityLengthInCells / 2, patchWidth, patchLength,
				              blockWidthInCells, blockLengthInCells, streetWidth,
				              largeBlockWidthInCells, largeBlockLengthInCells, largeStreetWidth,
				              cellWidth, cellLength,
				              buildingAverageHeight, noise, styleA, styleB);
				meshesShader1.Add(patchShader1);
				meshesShader2.Add(patchShader2);
				meshesShader3.Add(patchShader3);
			}
		}

		Size size = GetCitySize(cityWidthInCells, cityLengthInCells,
								blockWidthInCells, blockLengthInCells, streetWidth,
								largeBlockWidthInCells, largeBlockLengthInCells, largeStreetWidth,
								cellWidth, cellLength);

		Debug.Log("Generated " + meshesShader1.Count + (meshesShader1.Count > 1 ? " patches" : "patch"));
		Debug.Log("Size: " + size.width + " x " + size.length);

		return size;
	}
}
