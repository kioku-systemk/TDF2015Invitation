Shader "Custom/background" {
	Properties {
		_topColor ("Top color", Color) = (1,0,0,1)
		_fogColor ("Fog color (horizon)", Color) = (0,1,0,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Front
		LOD 200

		CGPROGRAM
		#ifdef SHADER_API_OPENGL
            #pragma glsl
        #endif
		#pragma surface surf NoLighting

		sampler2D _MainTex;

		uniform float4 _topColor;
		uniform float4 _fogColor;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			return fixed4(s.Albedo, s.Alpha);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float altitude = clamp(2.0 * IN.uv_MainTex.y - 1.0, 0.0, 1.0);

			float3 color = lerp(_fogColor, _topColor, pow(altitude, 0.8));

			o.Albedo = float3(0.0, 0.0, 0.0); // Don't use the light function
			o.Alpha = 1.0;
			o.Emission = color;
		}
		ENDCG

	}
	FallBack "Diffuse"
}
