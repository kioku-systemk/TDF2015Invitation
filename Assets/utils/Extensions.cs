using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
	public static float RandomAverage(float average, float spread)
	{
		return UnityEngine.Random.Range(average - spread, average + spread);
	}

	public static T[] ConcatArrays<T>(params T[][] list)
	{
		var result = new T[list.Sum(a => a.Length)];
		int offset = 0;
		for (int x = 0; x < list.Length; x++)
		{
			list[x].CopyTo(result, offset);
			offset += list[x].Length;
		}
		return result;
	}
	/*
	// FIXME: rewrite using CombineChildren
	public static void Add(this Mesh self, Mesh other, Matrix4x4 transform)
	{
		var triangleOffset = self.triangles.Length;
		var vertexIndexOffset = self.vertices.Length;
		
		self.vertices = ConcatArrays(self.vertices, other.vertices.Select(p => transform.MultiplyPoint(p)).ToArray());
		self.normals = ConcatArrays(self.normals, other.normals.Select(n => transform.MultiplyVector(n)).ToArray());
		self.tangents = ConcatArrays(self.tangents, other.tangents.Select(t => {
			var t2 = transform.MultiplyVector(new Vector3(t.x, t.y, t.z));
			return new Vector4(t2.x, t2.y, t2.z, t.w); }).ToArray());
		self.uv = ConcatArrays(self.uv, other.uv);
		self.colors = ConcatArrays(self.colors, other.colors);
		
		var triangles = ConcatArrays(self.triangles, other.triangles);
		for (var i = triangleOffset; i < triangles.Length; ++i)
		{
			triangles[i] += vertexIndexOffset;
		}
		self.triangles = triangles;
	}
	*/

	public static void Transform(this Mesh mesh, Matrix4x4 mat)
	{
		mesh.vertices = mesh.vertices.Select(p => mat.MultiplyPoint(p)).ToArray();
		mesh.normals = mesh.normals.Select(n => mat.MultiplyVector(n)).ToArray();
		mesh.tangents = mesh.tangents.Select(t => {
			var t2 = mat.MultiplyVector(new Vector3(t.x, t.y, t.z));
			return new Vector4(t2.x, t2.y, t2.z, t.w); }).ToArray();
	}

	public static Matrix4x4 TranslationMatrix(float x, float y, float z)
	{
		return Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, Vector3.one);
	}

	public static Matrix4x4 RotationMatrix(float angle, float x, float y, float z)
	{
		return Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, new Vector3(x, y, z)), Vector3.one);
	}

	public static Matrix4x4 ScaleMatrix(float x, float y, float z)
	{
		return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(x, y, z));
	}

	public static void Translate(this Mesh mesh, float x, float y, float z) { mesh.Transform(TranslationMatrix(x, y, z)); }	
	public static void Rotate(this Mesh mesh, float angle, float x, float y, float z) { mesh.Transform(RotationMatrix(angle, x, y, z)); }
	public static void Scale(this Mesh mesh, float x, float y, float z) { mesh.Transform(ScaleMatrix(x, y, z)); }
}
