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
		float rnd1 = 6.0f + Random.Range(0.1f, 10.0f);
		float rnd2 = 5.0f + Random.Range(0.1f, 10.0f);
		float wid1 = 0.1f + Random.Range(0.1f, 0.5f);
		float wid2 = 0.1f + Random.Range(0.1f, 0.5f);
		float pos1 = Random.Range(0.0f, 1.0f);
		float pos2 = Random.Range(0.0f, 1.0f);
		float colr = Random.Range(0.0f, 255.0f);
		float colg = Random.Range(0.0f, 255.0f);
		float colb = Random.Range(0.0f, 255.0f);
		float cola = Random.Range(0.0f, 255.0f);
		Color32 col = new Color(colr, colg, colb, cola);
		mesh.vertices  = new Vector3[] {
			new Vector3(0-rnd1, 1, 0), new Vector3(0-rnd1, 1, 100), new Vector3(wid1-rnd1, 1, 100), new Vector3(wid1-rnd1, 1, 0),
			new Vector3(0+rnd2, 1, 0), new Vector3(0+rnd2, 1, 100), new Vector3(wid2+rnd2, 1, 100), new Vector3(wid2+rnd2, 1, 0)
		};
        mesh.uv        = new Vector2[] {
        	new Vector2(pos1, 0), new Vector2(pos1, 1), new Vector2(pos1, 1), new Vector2 (pos1, 0),
         	new Vector2(pos2, 0), new Vector2(pos2, 1), new Vector2(pos2, 1), new Vector2 (pos2, 0)
        };
        mesh.normals   = new Vector3[] {
        	new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),
      		new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0)
        };
        mesh.colors32   = new Color32[] {
        	col, col, col, col,
      		col, col, col, col
        };
        mesh.triangles = new int[] {
        	0  ,1  ,2  ,0  ,2  ,3  ,
			0+4,1+4,2+4,0+4,2+4,3+4
        };
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
