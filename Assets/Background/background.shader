Shader "Custom/background" {
	Properties {
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
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			float f = pow(IN.uv_MainTex.y, 0.2);

			// TODO: sunset shader!!
			float3 col = float3(f,f,f);

			o.Albedo = col;
			o.Alpha = 1.0;
		}
		ENDCG
		
	} 
	FallBack "Diffuse"
}
