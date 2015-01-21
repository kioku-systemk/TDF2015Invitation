Shader "Custom/WorldDeformation" {
	Properties {
		_waveHeight ("wave height", Float) = 0.0
		_xFreq ("X frequency", Float) = 0.02
		_yFreq ("Y frequency", Float) = 0.02
		_time ("Time", Float) = 0.0
	}
    SubShader {
        Pass {
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            uniform float _waveHeight;
            uniform float _xFreq;
            uniform float _yFreq;
            uniform float _time;

			//
			// Parametric equation: [x,y] --> [x,y,z]
			//
			float X(float2 p) { return p.x; }
			float Y(float2 p) { return _waveHeight * (sin(2.0 * _time + _xFreq * p.x) + sin(_time + _yFreq * p.y)); }
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
			
			float4 vert(float4 v:POSITION) : SV_POSITION {
				return mul(UNITY_MATRIX_MVP, mul(v - float4(v.x, 0.0, v.z, 0.0), T(v.xz)));
            }
            
            
            fixed4 frag() : COLOR {
                return fixed4(0.4,0.4,0.4,1.0);
            }
            
            ENDCG
        }
    }
}
