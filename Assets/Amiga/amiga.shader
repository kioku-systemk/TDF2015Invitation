Shader "Custom/amiga" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#ifdef SHADER_API_OPENGL
            #pragma glsl
        #endif
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float div = 8;
			float v = fmod(IN.uv_MainTex.x, 1.0/div)*div;
			float h = fmod(IN.uv_MainTex.y, 2.0/div)*div*0.5;
			v = smoothstep(0.5, 0.5001, v);
			h = smoothstep(0.5, 0.5001, h);
			float a1 = v * h;
			float a2 = (1.0-v) * (1.0-h);
			o.Albedo = lerp(float3(1,1,1), float3(1,0,0), a2+a1);
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
