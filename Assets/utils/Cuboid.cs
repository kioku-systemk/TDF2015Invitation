using UnityEngine;
using System.Linq;

public class Cuboid {
	private struct Vertex
	{
		public Vector3 p;
		public Vector3 n;
		public Vector3 t;
		public Vector2 uv;
		public Vector4 w;

		public Vertex(Vector3 position, Vector3 normal, Vector3 tangeant, Vector2 textureCoord, Vector4 barycentricCoord)
		{
			p = position;
			n = normal;
			t = tangeant;
			uv = textureCoord;
			w = barycentricCoord;
		}
	};

	private static Vertex[] vertices =
	{
		new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),    Vector3.left, Vector3.forward, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3(-0.5f, -0.5f,  0.5f),    Vector3.left, Vector3.forward, new Vector2(1.0f, 0.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f,  0.5f),    Vector3.left, Vector3.forward, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f, -0.5f),    Vector3.left, Vector3.forward, new Vector2(0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f, -0.5f),   Vector3.right,    Vector3.back, new Vector2(1.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f, -0.5f),   Vector3.right,    Vector3.back, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f,  0.5f),   Vector3.right,    Vector3.back, new Vector2(0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f,  0.5f),   Vector3.right,    Vector3.back, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),    Vector3.down,   Vector3.right, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f, -0.5f),    Vector3.down,   Vector3.right, new Vector2(1.0f, 0.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f,  0.5f),    Vector3.down,   Vector3.right, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f, -0.5f,  0.5f),    Vector3.down,   Vector3.right, new Vector2(0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f, -0.5f),      Vector3.up,    Vector3.left, new Vector2(1.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f,  0.5f),      Vector3.up,    Vector3.left, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f,  0.5f),      Vector3.up,    Vector3.left, new Vector2(0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f, -0.5f),      Vector3.up,    Vector3.left, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),    Vector3.back,    Vector3.left, new Vector2(1.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f, -0.5f),    Vector3.back,    Vector3.left, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f, -0.5f),    Vector3.back,    Vector3.left, new Vector2(0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f, -0.5f),    Vector3.back,    Vector3.left, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.forward,   Vector3.right, new Vector2(0.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.forward,   Vector3.right, new Vector2(1.0f, 0.0f), new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
		new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.forward,   Vector3.right, new Vector2(1.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
		new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.forward,   Vector3.right, new Vector2(0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
	};

	private static int[] triangles =
	{
		0,   1,  2,    2,  3,  0,
		4,   5,  6,    6,  7,  4,
		8,   9, 10,   10, 11,  8,
		12, 13, 14,   14, 15, 12,
		16, 17, 18,   18, 19, 16,
		20, 21, 22,   22, 23, 20,
	};

	public enum Face
	{
		none	= 0,

		left	= 1,
		right	= 2,
		bottom	= 4,
		top		= 8,
		back	= 16,
		front	= 32,

		all		= 63,
	};

	private static bool IsFaceIncluded(int i, Face face)
	{
		int test = 1 << (i/6);
		return ((int)face & test) != 0;
	}

	public static Mesh Create(Vector3 size, Face face)
	{
		Mesh cuboid = new Mesh();
		cuboid.name = "Cuboid";
		cuboid.vertices	= vertices.Select(x => new Vector3(x.p.x * size.x, x.p.y * size.y, x.p.z * size.z)).ToArray();
		cuboid.normals	= vertices.Select(x => x.n).ToArray();
		cuboid.tangents	= vertices.Select(x => new Vector4(x.t.x, x.t.y, x.t.z, 0.0f)).ToArray();
		cuboid.uv		= vertices.Select(x => x.uv).ToArray();
		cuboid.uv2		= vertices.Select(x => new Vector2(1.0f, 1.0f) - x.uv).ToArray();
		cuboid.colors	= vertices.Select(x => Color.white).ToArray();
		cuboid.triangles = triangles.Where((v, i) => IsFaceIncluded(i, face)).ToArray();
		return cuboid;
	}

	public static Mesh CreateWithTag(Vector3 size, Vector3 tag, Face face)
	{
		Mesh cuboid = new Mesh();
		cuboid.name = "Cuboid";
		cuboid.vertices	= vertices.Select(x => new Vector3(x.p.x * size.x, x.p.y * size.y, x.p.z * size.z)).ToArray();
		cuboid.normals	= vertices.Select(x => x.n).ToArray();
		cuboid.tangents	= vertices.Select(x => new Vector4(x.t.x, x.t.y, x.t.z, 0.0f)).ToArray();
		cuboid.uv		= vertices.Select(x => x.uv).ToArray();
		cuboid.uv2		= vertices.Select(x => new Vector2(1.0f, 1.0f) - x.uv).ToArray();

		Color color = new Color(tag.x, tag.y, tag.z);
		cuboid.colors	= vertices.Select((x, i) => (i/4 == 3 || i/4 == 5) ? Color.white : color).ToArray();
		cuboid.triangles = triangles.Where((v, i) => IsFaceIncluded(i, face)).ToArray();
		return cuboid;
	}

	public static Mesh CreateWithUVGrid(Vector3 size, Vector3 unit, Vector3 tag, Face face)
	{
		Mesh cuboid = new Mesh();
		cuboid.name = "CuboidWithUVGrid";
		cuboid.vertices	= vertices.Select(x => new Vector3(x.p.x * size.x, x.p.y * size.y, x.p.z * size.z)).ToArray();
		cuboid.normals	= vertices.Select(x => x.n).ToArray();
		cuboid.tangents	= vertices.Select(x => new Vector4(x.t.x, x.t.y, x.t.z, 0.0f)).ToArray();

		Vector3 scale = new Vector3(Mathf.Ceil(size.x / unit.x),
									Mathf.Ceil(size.y / unit.y),
									Mathf.Ceil(size.z / unit.z));

		cuboid.uv		= vertices.Select((x, i) => (i/4 == 3 || i/4 == 5) ? x.uv : new Vector2(x.uv.x * scale.x, x.uv.y * scale.y)).ToArray();
		cuboid.uv2		= vertices.Select(x => new Vector2((1.0f - x.uv.x) * scale.x, (1.0f - x.uv.y) * scale.y)).ToArray();

		Color color = new Color(tag.x, tag.y, tag.z);
		cuboid.colors	= vertices.Select((x, i) => (i/4 == 3 || i/4 == 5) ? Color.white : color).ToArray();
		cuboid.triangles = triangles.Where((v, i) => IsFaceIncluded(i, face)).ToArray();
		return cuboid;
	}
}
