using UnityEngine;

public class City {

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
		CombineInstance[] combine = new CombineInstance[patchWidthInCells * patchLengthInCells];

		for (var j = 0; j < patchLengthInCells; ++j) {
			var yStreets = j / blockLengthInCells;
			var yLargeStreets = j / largeBlockLengthInCells;
			var yStreetOffset = yStreets * streetWidth + yLargeStreets * largeStreetWidth;

			for (var i = 0; i < patchWidthInCells; ++i) {
				var xStreets = i / blockWidthInCells;
				var xLargeStreets = i / largeBlockWidthInCells;
				var xStreetOffset = xStreets * streetWidth + xLargeStreets * largeStreetWidth;

				var x = cellWidth * i + xStreetOffset;
				var y = cellLength * j + yStreetOffset;

				Random.seed = Hash.Get(Hash.Get(i) + j);
				int index = i + j * patchWidthInCells;
				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB));
				combine[index].transform = Extensions.TranslationMatrix(x, 0.0f, y);
			}
		}
		patch.CombineMeshes(combine);
	}
}
