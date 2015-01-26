Shader "Custom/WorldDeformation" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		//_MainTex ("Texture", 2D) = "white" {}
		_waveHeight ("wave height", Range(0, 20)) = 0.0
		_xFreq ("X frequency", Range(0.0, 0.1)) = 0.02
		_yFreq ("Y frequency", Range(0.0, 0.1)) = 0.02
		_speed ("Speed", Range(0.0, 10.0)) = 1.0
		_maxWidth ("Max width", Float) = 1.0
		_maxLength ("Max length", Float) = 1.0
		_a ("A", Range(0.0, 1000.0)) = 1.0
		_c ("C", Range(0.0, 1000.0)) = 1.0
		_d ("D", Float) = 1.0
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
		uniform float _speed;
		uniform float _maxWidth;
		uniform float _maxLength;
		uniform float _a;
		uniform float _c;
		uniform float _d;

		//
		// Parametric equation: [x,y] --> [x,y,z]
		//
		#define TAU 6.2831853
		
		//float X(float2 p) { return (_maxLength/TAU + _maxWidth/TAU * sin(TAU * p.x/_maxWidth)) * sin(TAU * p.y/_maxLength); }
		//float Y(float2 p) { return _maxWidth/TAU * cos(TAU * p.x/_maxWidth); }
		//float Z(float2 p) { return (_maxLength/TAU + _maxWidth/TAU * sin(TAU * p.x/_maxWidth)) * cos(TAU * p.y/_maxLength); }
		
		//float X(float2 p) { return 1 * sin((TAU / 2) * p.x / _maxWidth) * cos(TAU * p.y / _maxLength); }
		//float Y(float2 p) { return 1 * sin((TAU / 2) * p.x / _maxWidth) * sin(TAU * p.y / _maxLength); }
		//float Z(float2 p) { return 1 * cos((TAU / 2) * p.x / _maxWidth); }
		
		//float X(float2 p) { return p.x; }
		//float Y(float2 p) { return _waveHeight * (sin(_speed * _Time.y + _xFreq * p.x) + sin(_speed * _Time.y + _yFreq * p.y)); }
		//float Z(float2 p) { return p.y; }
		
		float sX(float2 p) { return (_maxLength/TAU + _maxWidth/TAU * sin(TAU * p.x/_maxWidth)) * sin(TAU * p.y/_maxLength); }
		float sY(float2 p) { return _maxWidth/TAU * cos(TAU * p.x/_maxWidth); }
		float sZ(float2 p) { return (_maxLength/TAU + _maxWidth/TAU * sin(TAU * p.x/_maxWidth)) * cos(TAU * p.y/_maxLength); }
		
		//float X(float2 p) { return 1 * sin((TAU / 2) * p.x / _maxWidth) * cos(TAU * p.y / _maxLength); }
		//float Y(float2 p) { return 1 * sin((TAU / 2) * p.x / _maxWidth) * sin(TAU * p.y / _maxLength); }
		//float Z(float2 p) { return 1 * cos((TAU / 2) * p.x / _maxWidth); }
		
		float pX(float2 p) { return p.x; }
		float pY(float2 p) { return 0; }
		float pZ(float2 p) { return p.y; }
		
		float X(float2 p) { return sX(p); }
		float Y(float2 p) { return sY(p); }
		float Z(float2 p) { return sZ(p); }
		
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
			
			//if (p.z < 200.0)
			{
			float4x4 transform = T(p.xz);
			v.vertex = mul(p - float4(p.x, 0.0, p.z, 0.0), transform);
			v.normal = mul(float4(v.normal, 0.0), transform).xyz;
			}
		}

		float4 _Color;
		//sampler2D _MainTex;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;//tex2D (_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	} 
	Fallback "Diffuse"
}
