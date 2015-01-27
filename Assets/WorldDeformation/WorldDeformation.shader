﻿Shader "Custom/WorldDeformation" {
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
		_a ("A", Range(0.0, 1000.0)) = 1.0
		_c ("C", Range(0.0, 1000.0)) = 1.0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert addshadow

		struct Input {
			float2 uv_MainTex;
		};

		uniform float _vertexDeformation;
		uniform float _waveHeight;
		uniform float _xFreq;
		uniform float _yFreq;
		uniform float _speed;
		uniform float _maxWidth;
		uniform float _maxLength;
		uniform float _a;
		uniform float _c;

		//
		// Parametric equation: [x,y] --> [x,y,z]
		//
		#define TAU 6.2831853

		// Torus
		float3 P_identity(float2 p)
		{
			return float3(p.x, 0.0, p.y);
		}

		// Torus
		float3 P_torus(float2 p)
		{
			float u = TAU * p.y/_maxLength;
			float v = TAU * p.x/_maxWidth;
			float a = _maxWidth/TAU;
			float c = _maxLength/TAU;

			float x = (c + a * sin(TAU * p.x/_maxWidth)) * sin(u);
			float y = a * cos(v);
			float z = (c + a * sin(TAU * p.x/_maxWidth)) * cos(u);

			return float3(x, y, z);
		}

		// Torus knot
		float3 P_pq_torus(float2 p)
		{
			// TODO
			float P = 0.0;
			float Q = 0.0;

			float theta = 0.0;
			float phi = 0.0;
			float r = cos(Q * theta) + 2.0;

			float x = r * cos(P * phi);
			float y = -sin(Q * phi);
			float z = r * sin(P * phi);

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
			return (_vertexDeformation < 0.5 ?
					lerp(P_identity(p),	P_sinus(p), clamp(2.0 * _vertexDeformation, 0.0, 1.0)) :
					lerp(P_sinus(p),	P_torus(p), clamp(2.0 * _vertexDeformation - 1.0, 0.0, 1.0)));
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
