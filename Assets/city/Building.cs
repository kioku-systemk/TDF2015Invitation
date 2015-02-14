using UnityEngine;

public class Building {

	public class Meshes
	{
		public Mesh Building = new Mesh();
		public Mesh BillboardAd = new Mesh();
		public Mesh LightStreak = new Mesh();
	};

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
								Mesh billboardAd,
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
		CombineInstance[] combine = new CombineInstance[2];

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
			CombineInstance[] combineBillboardAd = new CombineInstance[1];
			combineBillboardAd[0].mesh = Cuboid.CreateWithTag(new Vector3(billboardWidth, billboardHeight, billboardLength), tag, Cuboid.Face.all);
			combineBillboardAd[0].transform = Extensions.TranslationMatrix(0.5f * (trunkWidth + billboardWidth) * x, 0.5f * trunkHeight, 0.5f * z * (trunkLength + billboardLength));
			billboardAd.CombineMeshes(combineBillboardAd);
		}

		building.CombineMeshes(combine);
	}
	public static void GenerateLightStreak(Mesh lightStreak,
								  float averageHeight,		/// Average height of the building
								  float maxWidth,			/// Max width of the building
								  float maxLength			/// Max length of the building
								  )
	{
		Mesh mesh = lightStreak;
		// TODO: Generate polygons
		mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0),  //front
                                        new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1),  //back
                                        new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1),  //right
                                        new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0),  //left
                                        new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0),  //top
                                        new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0)}; //bottom
         mesh.uv = new Vector2[] {new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0)};
         mesh.triangles = new int[] {0,1,2,0,2,3,4,5,6,4,6,7,8,9,10,8,10,11,12,13,14,12,14,15,16,17,18,16,18,19,20,21,22,20,22,23};
         mesh.normals = new Vector3[] {new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),  //front
                                       new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),  //back
                                       new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),  //right
                                       new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),  //left
                                       new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),  //top
	                                   new Vector3( 0,-1, 0),new Vector3( 0,-1, 0),new Vector3( 0,-1, 0),new Vector3( 0,-1, 0)}; //bottom
	}
	public static Meshes Generate(float averageHeight,		/// Average height of the building
								  float maxWidth,			/// Max width of the building
								  float maxLength,			/// Max length of the building
								  float noise,				/// Noise: when 0, the building is exactly averageHeight x maxWidth x maxLength
								  Vector2 style,			/// Style of the building (magic fudge parameter)
								  Vector3 tag,				/// Information to identify in the shader
								  BillboardDesc billboard	/// Where to put the billboard if any
								  )
	{
		Meshes meshes = new Meshes();
		Generate(meshes.Building, meshes.BillboardAd, averageHeight, maxWidth, maxLength, noise, style, tag, billboard);
		GenerateLightStreak(meshes.LightStreak, averageHeight, maxWidth, maxLength);
		return meshes;
	}
}
