Shader "Custom/WorldDeformation" {
	Properties {
		//_MainTex ("Texture", 2D) = "white" {}
		_waveHeight ("wave height", Range(0, 20)) = 0.0
		_xFreq ("X frequency", Range(0.0, 0.1)) = 0.02
		_yFreq ("Y frequency", Range(0.0, 0.1)) = 0.02
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert addshadow

		struct Input {
			float2 uv_MainTex;
		};

		uniform float _waveHeight;
		uniform float _xFreq;
		uniform float _yFreq;

		//
		// Parametric equation: [x,y] --> [x,y,z]
		//
		float X(float2 p) { return p.x; }
		float Y(float2 p) { return _waveHeight * (sin(2.0 * _Time.y + _xFreq * p.x) + sin(_Time.y + _yFreq * p.y)); }
		float Z(float2 p) { return p.y; }
		float3 P(float2 p) { return float3(X(p), Y(p), Z(p)); }

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
			
			float4x4 transform = T(p.xz);
			v.vertex = mul(p - float4(p.x, 0.0, p.z, 0.0), transform);
			v.normal = mul(v.normal, transform);
		}

		//sampler2D _MainTex;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = 1;//tex2D (_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	} 
	Fallback "Diffuse"
}
