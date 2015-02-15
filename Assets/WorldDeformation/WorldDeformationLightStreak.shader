Shader "Custom/WorldDeformationLightStreak" {
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
	Pass {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Lighting Off
		ZWrite Off

CGPROGRAM

#ifdef SHADER_API_OPENGL
	#pragma glsl
#endif

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"



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

struct v2f
{
	fixed4 color : COLOR;
	float4 vertex : SV_POSITION;
	float4 texcoord : TEXCOORD0;
};

v2f vert(appdata_full v)
{
	float4 p = v.vertex;
	p.x += fmod(0.5 * _maxWidth + _vertexLatTranslation, _maxWidth) - 0.5 * _maxWidth;
	p.z += fmod(0.25 * _maxLength + _vertexTranslation, _maxLength);

	float4x4 transform = T(p.xz);
	v.vertex = mul(p - float4(p.x, 0.0, p.z, 0.0), transform);

	v2f ret;
	ret.color = v.color;
	ret.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	ret.texcoord = v.texcoord;
	return ret;
}

// ---8<--------------------------------------------------------------
// Fragment shading

float4 frag(v2f IN) : SV_Target
{
	float4 color = lerp(_headLightColor, _stopLightColor, float(IN.texcoord.y > 0.0));

	//float anim = fmod(abs((IN.texcoord.y + _move * IN.texcoord.x) * 0.1f), 1.0); // light move animation
	float4 ret;
	ret.rgb = color.rgb;//0.0*float3(anim,anim,anim) * color.rgb;

	float visible = smoothstep(IN.color.b, IN.color.b + 0.1, 1.1 * _effectCars);
	float trail = frac(IN.texcoord.y - _move);
	ret.a = color.a * visible * trail;

	return ret;
}

// ---8<--------------------------------------------------------------

ENDCG
	}
}
}
