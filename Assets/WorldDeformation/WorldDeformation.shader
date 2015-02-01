Shader "Custom/WorldDeformation" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		//_MainTex ("Texture", 2D) = "white" {}
		_vertexDeformation ("vertex deformation", Range(0, 1.0)) = 0.0
		_waveHeight ("wave height", Range(0, 20)) = 0.0
		_xFreq ("X frequency", Range(0.0, 0.1)) = 0.02
		_yFreq ("Y frequency", Range(0.0, 0.1)) = 0.02
		_speed ("Speed", Range(0.0, 10.0)) = 1.0
		_maxWidth ("Max width", Float) = 1.0
		_maxLength ("Max length", Float) = 1.0

		_effect1Intensity ("Effect 1 intensity", Range(0.0, 1.0)) = 0.0
		_effect2Intensity ("Effect 2 intensity", Range(0.0, 1.0)) = 0.0
		_effect3Intensity ("Effect 3 intensity", Range(0.0, 1.0)) = 0.0
		_effect4Intensity ("Effect 4 intensity", Range(0.0, 1.0)) = 0.0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM

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
		#pragma surface surf Lambert vertex:vert addshadow


		// Surface Shader input structure
		//
		// The input structure Input generally has any texture
		// coordinates needed by the shader. Texture coordinates must
		// be named "uv" followed by texture name (or start it with
		// "uv2" to use second texture coordinate set).
		struct Input {
			float2 uv_MainTex;
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

		uniform float _vertexDeformation;
		uniform float _waveHeight;
		uniform float _xFreq;
		uniform float _yFreq;
		uniform float _speed;
		uniform float _maxWidth;
		uniform float _maxLength;
		uniform float _effect1Intensity;
		uniform float _effect2Intensity;
		uniform float _effect3Intensity;
		uniform float _effect4Intensity;

		// ---8<--------------------------------------------------------------
		// Vertex shading

		//
		// Parametric equations: [x,y] --> [x,y,z]
		//
		#define TAU 6.2831853

		// No transformation
		float3 P_identity(float2 p)
		{
			return float3(p.x, 0.0, p.y);
		}

		// Cylinder
		float3 P_halfcylinder(float2 p)
		{
			float theta = -0.7 + 0.5 * TAU * p.x/_maxWidth;
			float r = _maxLength/TAU;

			float x = -r * cos(theta) + r;
			float y = -r * sin(theta) + r;
			float z = p.y;

			return float3(x, y, z);
		}

		// Cylinder
		float3 P_cylinder(float2 p, float rate)
		{
			float circumpherence = _maxWidth * lerp(100.0, 1.0, pow(rate, 0.1));

			float theta = TAU * p.x / circumpherence;
			float r = circumpherence / TAU;

			float x = r * sin(theta);
			float y = r * -cos(theta) + r;
			float z = p.y;

			return float3(x, y, z);
		}

		// Torus
		float3 P_torus(float2 p, float rate)
		{
			float circumpherence1 = _maxWidth * lerp(100.0, 1.0, pow(clamp(2.0 * rate, 0.0, 1.0), 0.1));
			float circumpherence2 = _maxLength * lerp(100.0, 1.0, pow(clamp(2.0 * rate - 1.0, 0.0, 1.0), 0.1));

			float theta = TAU * p.x / circumpherence1;
			float phi = TAU * p.y/circumpherence2;
			float r1 = circumpherence1 / TAU;
			float r2 = circumpherence2 / TAU;

			float x = (r1 * -sin(theta) + r2) * -cos(phi) + r2;
			float y = r1 * -cos(theta) + r1;
			float z = (r1 * -sin(theta) + r2) * sin(phi);

			return float3(x, y, z);
		}

		// Torus knot
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

		// Sinus
		float3 P_sinus(float2 p)
		{
			float x = p.x;
			float y = _waveHeight * (sin(_speed * _Time.y + _xFreq * p.x) + sin(_speed * _Time.y + _yFreq * p.y));
			float z = p.y;
			return float3(x, y, z);
		}

		// Combined final equation
		float3 P(float2 p)
		{
			return P_torus(p, _vertexDeformation);
			// TODO: more optimization?
// 			return (_vertexDeformation < 0.25 ?
// 					lerp(P_identity(p),	P_halfcylinder(p), clamp(4.0 * _vertexDeformation, 0.0, 1.0)) :
// 					(_vertexDeformation < 0.5 ?
// 						lerp(P_halfcylinder(p),	P_cylinder(p), clamp(4.0 * _vertexDeformation - 1.0, 0.0, 1.0)) :
// 						lerp(P_cylinder(p),	P_torus(p), clamp(2.0 * _vertexDeformation - 1.0, 0.0, 1.0))
// 					 )
// 					);
		}

		//
		// Deduce transformation from parametric equation.
		//
		float4x4 T(float2 p2d) {
			float3 p = P(p2d);

			float3 ux = normalize(P(p2d + float2(0.01, 0.0)) - p);
			float3 uz = normalize(P(p2d + float2(0.0, 0.01)) - p);
			float3 uy = cross(uz, ux);

			return float4x4(float4(ux, 0.0),
							float4(uy, 0.0),
							float4(uz, 0.0),
							float4(p,  1.0));
		}

		void vert (inout appdata_full v) {
			float4 p = v.vertex;

			if (p.x > 20.0 && p.z > 20.0)
			{
			float4x4 transform = T(p.xz);
			v.vertex = mul(p - float4(p.x, 0.0, p.z, 0.0), transform);
			v.normal = mul(float4(v.normal, 0.0), transform).xyz;
			}
		}

		// ---8<--------------------------------------------------------------
		// Fragment shading

		//
		// TODO
		//
		float3 awesomeShaderEffect(Input IN)
		{
			float3 superEffect1 = float3(0.8, 0.0, 0.3) * frac(2.0 * _Time.y);

			float3 superEffect2 = float3(0.3, 0.8, 0.0) *
				((floor(0.5 * IN.worldPos.x) - 2.0 * floor(0.25 * IN.worldPos.x)) *
				 (floor(0.5 * IN.worldPos.y) - 2.0 * floor(0.25 * IN.worldPos.y)) *
				 (floor(0.5 * IN.worldPos.z) - 2.0 * floor(0.25 * IN.worldPos.z)));

			float3 superEffect3 = float3(0.0, 0.3, 0.8) * IN.uv_MainTex.x;
			float3 superEffect4 = float3(0.6, 0.6, 0.6);

			return (_effect1Intensity * superEffect1 +
					_effect2Intensity * superEffect2 +
					_effect3Intensity * superEffect3 +
					_effect4Intensity * superEffect4);
		}

		float4 _Color;
		//sampler2D _MainTex;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;// * float4(awesomeShaderEffect(IN), 1.0); // * tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		// ---8<--------------------------------------------------------------

		ENDCG
	}
	Fallback "Diffuse"
}
