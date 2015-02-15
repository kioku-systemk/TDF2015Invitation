Shader "Custom/WorldDeformation" {
	Properties {
		_vertexTranslation ("vertex translation", Float) = 0.0
		_vertexLatTranslation ("vertex lateral translation", Float) = 0.0
		_vertexDeformation ("vertex deformation", Range(0, 1.0)) = 0.0
		_maxWidth  ("Max width",  Float) = 1.0
		_maxLength ("Max length", Float) = 1.0

		_fstTex ("1st texture", 2D) = "white" {}
		_sndTex ("2nd texture", 2D) = "white" {}

		_headLightColor ("Head light color", Color) = (1,1,1,1)
		_stopLightColor ("Stop light color", Color) = (1,0,0,1)

		_effectCars ("Car lights", Range(0.0, 1.0)) = 0.0

		_move ("light streak move", Float) = 0.0

	}
	SubShader {
		Tags { "RenderType" = "Transparent" }

		CGPROGRAM

		#ifdef SHADER_API_OPENGL
			#pragma glsl
		#endif

		// From documentation:
		// http://docs.unity3d.com/Manual/SL-SurfaceShaders.html
		//
		// vertex:Func - Custom vertex modification function. See Tree
		//               Bark shader for example.
		//
		// addshadow   - Add shadow caster & collector passes. Commonly
		//               used with custom vertex modification, so that
		//               shadow casting also gets any procedural
		//               vertex animation.
		#pragma surface surf NoLighting vertex:vert alpha


		// Surface Shader input structure
		//
		// The input structure Input generally has any texture
		// coordinates needed by the shader. Texture coordinates must
		// be named "uv" followed by texture name (or start it with
		// "uv2" to use second texture coordinate set).
		struct Input {
			fixed4 color		: COLOR;
			float2 uv_fstTex	: TEXCOORD0; // We use uv to store barycentric coordinates :)
			float2 uv2_sndTex	: TEXCOORD1;
			float3 worldPos;
		};
		// Additional values that can be put into Input structure:
		//
		// float3 viewDir     - will contain view direction, for computing
		//                      Parallax effects, rim lighting etc.
		//
		// float4 with COLOR semantic - will contain interpolated
		//                      per-vertex color.
		//
		// float4 screenPos   - will contain screen space position for
		//                      reflection effects. Used by WetStreet
		//                      shader in Dark Unity for example.
		//
		// float3 worldPos    - will contain world space position.
		//
		// float3 worldRefl   - will contain world reflection vector if
		//                      surface shader does not write to
		//                      o.Normal. See Reflect-Diffuse shader
		//                      for example.
		//
		// float3 worldNormal - will contain world normal vector if
		//                      surface shader does not write to
		//                      o.Normal.
		//
		// float3 worldRefl; INTERNAL_DATA - will contain world
		//                      reflection vector if surface shader
		//                      writes to o.Normal. To get the
		//                      reflection vector based on per-pixel
		//                      normal map, use WorldReflectionVector
		//                      (IN, o.Normal). See Reflect-Bumped
		//                      shader for example.
		//
		// float3 worldNormal; INTERNAL_DATA - will contain world
		//                      normal vector if surface shader writes
		//                      to o.Normal. To get the normal vector
		//                      based on per-pixel normal map, use
		//                      WorldNormalVector (IN, o.Normal).

		uniform float4 _headLightColor;
		uniform float4 _stopLightColor;

		uniform float _vertexTranslation;
		uniform float _vertexLatTranslation;
		uniform float _vertexDeformation;
		uniform float _maxWidth;
		uniform float _maxLength;
		uniform float _move;

		uniform float _effectCars;

		float4 _spectrum;

		// ---8<--------------------------------------------------------------
		// Vertex shading

		//
		// Parametric equations: [x,y] --> [x,y,z]
		//
		#define TAU 6.2831853

		// Torus
		float3 P_torus(float2 p, float rate)
		{
			float circumpherence1 = _maxWidth * lerp(100.0, 1.0, pow(clamp(2.0 * rate, 0.0, 1.0), 0.1));
			float circumpherence2 = _maxLength * lerp(100.0, 1.0, pow(clamp(2.0 * rate - 1.0, 0.0, 1.0), 0.1));

			float theta = TAU * p.x / circumpherence1;
			float phi = TAU * p.y / circumpherence2;
			float r1 = circumpherence1 / TAU;
			float r2 = circumpherence2 / TAU;

			float x = (r1 * -sin(theta) + r2) * -cos(phi) + r2;
			float y = r1 * -cos(theta) + r1;
			float z = (r1 * -sin(theta) + r2) * sin(phi);

			return float3(x, y, z);
		}

		/*
		// Torus knot --- TODO
		float3 P_pq_torus(float2 p)
		{
			// TODO
			float P = 1.0;
			float Q = 0.0;

			float theta = TAU * p.x/_maxWidth;
			float phi = TAU * p.y/_maxLength;
			float r1 = _maxWidth/TAU;
			float r2 = _maxLength/TAU;

			float r = 0.5 * sin(Q * phi) + 1.0;
			float x = r * sin(P * phi);
			float y = r * cos(Q * phi);
			float z = r * cos(P * phi);

			return float3(x, y, z);
		}
		*/

		// Combined final equation
		float3 P(float2 p)
		{
			return P_torus(p, _vertexDeformation);
		}

		//
		// Deduce transformation from parametric equation.
		//
		float4x4 T(float2 p2d) {
			float3 p = P(p2d);

			float3 ux = normalize(P(p2d + float2(1.0, 0.0)) - p);
			float3 uz = normalize(P(p2d + float2(0.0, 1.0)) - p);
			float3 uy = cross(uz, ux);

			return float4x4(float4(ux, 0.0),
							float4(uy, 0.0),
							float4(uz, 0.0),
							float4(p,  1.0));
		}

		void vert (inout appdata_full v) {
			float4 p = v.vertex;
			p.x += fmod(0.5 * _maxWidth + _vertexLatTranslation, _maxWidth) - 0.5 * _maxWidth;
			p.z += fmod(0.25 * _maxLength + _vertexTranslation, _maxLength);

			float4x4 transform = T(p.xz);
			v.vertex = mul(p - float4(p.x, 0.0, p.z, 0.0), transform);
			v.normal = mul(float4(v.normal, 0.0), transform).xyz;
			v.tangent = mul(v.tangent, transform);
		}

		// ---8<--------------------------------------------------------------
		// Fragment shading

		sampler2D _fstTex;
		sampler2D _sndTex;

		// No albedo at all
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			return float4(0.0, 0.0, 0.0, 0.0);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float4 color = lerp(_headLightColor, _stopLightColor, float(IN.uv_fstTex.y > 0.0));

			float anim = 1.0;//fmod(abs((IN.uv_fstTex.y + _move * IN.uv_fstTex.x) * 0.1f), 1.0); // light move animation
			o.Emission = color.rgb;//0.0*float3(anim,anim,anim) * color.rgb;
			o.Alpha = color.a * _effectCars * frac(IN.uv_fstTex.y - _move); // Fade out the tail
		}

		// ---8<--------------------------------------------------------------

		ENDCG
	}
	Fallback "Diffuse"
}
