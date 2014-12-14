using UnityEngine;
using System.Collections;

public static class Hash {
	private static int[] hash = {
		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
	};
	public const int Mask = 255;

	public static int Get(int i)
	{
		return hash[i & Mask];
	}

	public static void GetFor1DSpace(int i0, int i1,
	                                 out int h0, out int h1)
	{
		h0 = Hash.Get(i0);
		h1 = Hash.Get(i1);
	}

	public static void GetFor2DSpace(int i0, int i1, int j0, int j1,
	                                 out int h00, out int h01, out int h10, out int h11)
	{
		int h0, h1; GetFor1DSpace(i0, i1, out h0, out h1);
		h00 = Hash.Get(h0 + j0);
		h01 = Hash.Get(h1 + j0);
		h10 = Hash.Get(h0 + j1);
		h11 = Hash.Get(h1 + j1);
	}

	public static void GetFor3DSpace(int i0, int i1, int j0, int j1, int k0, int k1,
	                                 out int h000, out int h001, out int h010, out int h011,
	                                 out int h100, out int h101, out int h110, out int h111)
	{
		int h00, h01, h10, h11; GetFor2DSpace(i0, i1, j0, j1,
		                                      out h00, out h01, out h10, out h11);

		h000 = Hash.Get(h00 + k0);
		h001 = Hash.Get(h01 + k0);
		h010 = Hash.Get(h10 + k0);
		h011 = Hash.Get(h11 + k0);
		h100 = Hash.Get(h00 + k1);
		h101 = Hash.Get(h01 + k1);
		h110 = Hash.Get(h10 + k1);
		h111 = Hash.Get(h11 + k1);
	}
}
