using UnityEngine;

public class Building {
	public static void Generate(Mesh building,
	                            float averageHeight,	/// Average height of the building
	                            float maxWidth,			/// Max width of the building
	                            float maxLength,		/// Max length of the building
	                            float noise,			/// Noise: when 0, the building is exactly averageHeight x maxWidth x maxLength
	                            Vector2 style			/// Style of the building (magic fudge parameter)
	                            )
	{
		float height = Extensions.RandomAverage(averageHeight, averageHeight * noise);
		float reduction = 0.3f + 0.7f * style.x;
		float asymmetry = 0.6f * style.y;// * style.x;

		float floorHeight = 3.0f;
		float baseHeight = height * Random.Range(0.1f, 0.2f);
		baseHeight = Mathf.Max(floorHeight, baseHeight - (baseHeight % floorHeight));
		float topHeight =  height * Random.Range(0.05f, 0.1f);
		float trunkHeight = height - baseHeight - topHeight;

		float baseWidth =  Random.Range(maxWidth * (1.0f - noise), maxWidth);
		float baseLength = Random.Range(maxLength * (1.0f - noise), maxLength);
		float trunkWidth = baseWidth * Extensions.RandomAverage(reduction, reduction * noise);
		float trunkLength = baseLength * Extensions.RandomAverage(reduction, reduction * noise);
		float topWidth =  Random.Range(trunkWidth * reduction * (1.0f - noise), trunkWidth);
		float topLength = Random.Range(trunkLength * reduction * (1.0f - noise), trunkLength);

		float baseX =  Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (maxWidth - baseWidth));
		float baseY =  Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (maxLength - baseLength));
		float trunkX = Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (baseWidth - trunkWidth));
		float trunkY = Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (baseLength - trunkLength));
		float topX =   Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (trunkWidth - topWidth));
		float topY =   Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (trunkLength - topLength));

		CombineInstance[] combine = new CombineInstance[3];

		// Top
		combine[0].mesh = Cuboid.Create(new Vector3(topWidth, topHeight, topLength)); // FIXME: (pave_all & ~pave_bottom)
		combine[0].transform = Extensions.TranslationMatrix(topX + trunkX + baseX, 0.5f * topHeight + trunkHeight + baseHeight, topY + trunkY + baseY);

		// Main (trunk) part
		combine[1].mesh = Cuboid.Create(new Vector3(trunkWidth, trunkHeight, trunkLength)); // FIXME: (pave_all & ~pave_bottom)
		combine[1].transform = Extensions.TranslationMatrix(trunkX, 0.5f * trunkHeight + baseHeight, trunkY);

		// Base
		combine[2].mesh = Cuboid.Create(new Vector3(baseWidth, baseHeight, baseLength)); // FIXME: (pave_all & ~pave_bottom)
		combine[2].transform = Extensions.TranslationMatrix(baseX, 0.5f * baseHeight, baseY);

		building.CombineMeshes(combine);
	}
	
	public static Mesh Generate(float averageHeight,	/// Average height of the building
	                            float maxWidth,			/// Max width of the building
	                            float maxLength,		/// Max length of the building
	                            float noise,			/// Noise: when 0, the building is exactly averageHeight x maxWidth x maxLength
	                            Vector2 style			/// Style of the building (magic fudge parameter)
	                            )
	{
		Mesh building = new Mesh();
		Generate(building, averageHeight, maxWidth, maxLength, noise, style);
		return building;
	}
}
