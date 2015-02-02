using UnityEngine;

public class Building {
	public enum BillboardDesc {
		None,
		Left,
		Right,
		Front,
		Back,
	};

	private static Vector3 unitSize = new Vector3(3.0f, 3.0f, 3.0f);
	private static Cuboid.Face allButBottom = (Cuboid.Face.all & ~Cuboid.Face.bottom);

	public static void Generate(Mesh building,
								float averageHeight,	/// Average height of the building
								float maxWidth,			/// Max width of the building
								float maxLength,		/// Max length of the building
								float noise,			/// Noise: when 0, the building is exactly averageHeight x maxWidth x maxLength
								Vector2 style,			/// Style of the building (magic fudge parameter)
								Vector3 tag,			/// Information to identify in the shader
								BillboardDesc billboard	/// Where to put the billboard if any
								)
	{
		float height = Extensions.RandomAverage(averageHeight, averageHeight * noise);
		float reduction = 0.3f + 0.7f * style.x;
		float asymmetry = 0.6f * style.y;// * style.x;

		float floorHeight = 3.0f;
		float topHeight =  Mathf.Max(floorHeight, height * Random.Range(0.05f, 0.1f));
		float trunkHeight = height - topHeight;

		float trunkWidth =  Mathf.Max(3.0f, Random.Range(maxWidth * (1.0f - noise), maxWidth));
		float trunkLength = Mathf.Max(3.0f, Random.Range(maxLength * (1.0f - noise), maxLength));
		float topWidth =  Random.Range(trunkWidth * reduction * (1.0f - noise), trunkWidth);
		float topLength = Random.Range(trunkLength * reduction * (1.0f - noise), trunkLength);

		float trunkX = Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (maxWidth - trunkWidth));
		float trunkY = Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (maxLength - trunkLength));
		float topX =   Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (trunkWidth - topWidth));
		float topY =   Extensions.RandomAverage(0.0f, asymmetry * 0.5f * (trunkLength - topLength));

		//billboard = BillboardDesc.None;
		CombineInstance[] combine = new CombineInstance[(billboard == BillboardDesc.None ? 2 : 3)];

		// Top
		combine[0].mesh = Cuboid.Create(new Vector3(topWidth, topHeight, topLength), allButBottom);
		combine[0].transform = Extensions.TranslationMatrix(topX + trunkX, 0.5f * topHeight + trunkHeight, topY + trunkY);

		// Main (trunk) part
		combine[1].mesh = Cuboid.CreateWithUVGrid(new Vector3(trunkWidth, trunkHeight, trunkLength), unitSize, tag, allButBottom);
		combine[1].transform = Extensions.TranslationMatrix(trunkX, 0.5f * trunkHeight, trunkY);

		if (billboard != BillboardDesc.None)
		{
			float billboardHeight = Random.Range(1.0f, Mathf.Max(1.0f, trunkHeight - floorHeight));
			float billboardWidth =  (billboard == BillboardDesc.Left || billboard == BillboardDesc.Right ? 1.2f : 0.4f);
			float billboardLength = (billboard == BillboardDesc.Front || billboard == BillboardDesc.Back ? 1.2f : 0.4f);

			float x = (billboard == BillboardDesc.Left ? -1.0f : billboard == BillboardDesc.Right ? 1.0f : 0.8f);
			float z = (billboard == BillboardDesc.Front ? -1.0f : billboard == BillboardDesc.Back ? 1.0f : 0.8f);

			// Billboard sign
			combine[2].mesh = Cuboid.Create(new Vector3(billboardWidth, billboardHeight, billboardLength), Cuboid.Face.all);
			combine[2].transform = Extensions.TranslationMatrix(0.5f * (trunkWidth + billboardWidth) * x, 0.5f * trunkHeight, 0.5f * z * (trunkLength + billboardLength));
		}

		building.CombineMeshes(combine);
	}

	public static Mesh Generate(float averageHeight,	/// Average height of the building
								float maxWidth,			/// Max width of the building
								float maxLength,		/// Max length of the building
								float noise,			/// Noise: when 0, the building is exactly averageHeight x maxWidth x maxLength
								Vector2 style,			/// Style of the building (magic fudge parameter)
								Vector3 tag,			/// Information to identify in the shader
								BillboardDesc billboard	/// Where to put the billboard if any
								)
	{
		Mesh building = new Mesh();
		Generate(building, averageHeight, maxWidth, maxLength, noise, style, tag, billboard);
		return building;
	}
}
