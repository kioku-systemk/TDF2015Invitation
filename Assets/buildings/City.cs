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
	                                 float cellWidth,
	                                 float cellLength,
	                                 float buildingAverageHeight,
	                                 float noise,
	                                 float styleA,
	                                 float styleB) {
		CombineInstance[] combine = new CombineInstance[patchWidthInCells * patchLengthInCells];

		for (var j = 0; j < patchLengthInCells; ++j) {
			for (var i = 0; i < patchWidthInCells; ++i) {
				Random.seed = Hash.Get(Hash.Get(i) + j);
				int index = i + j * patchWidthInCells;
				var x = cellWidth * i + (i / blockWidthInCells) * streetWidth;
				var y = cellLength * j + (j / blockLengthInCells) * streetWidth;

				combine[index].mesh = Building.Generate(buildingAverageHeight, cellWidth, cellLength, noise, new Vector2(styleA, styleB));
				combine[index].transform = Extensions.TranslationMatrix(x, 0.0f, y);
			}
		}
		patch.CombineMeshes(combine);
	}
}
