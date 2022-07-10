
Shader "BG/Trippy"
{
	Properties{

	}
	SubShader
	{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
	Pass
	{
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"




		// A single iteration of Bob Jenkins' One-At-A-Time hashing algorithm.
	uint hash(uint x) {
		x += (x << 10u);
		x ^= (x >> 6u);
		x += (x << 3u);
		x ^= (x >> 11u);
		x += (x << 15u);
		return x;
	}



	// Compound versions of the hashing algorithm I whipped together.
	uint hash(uint2 v) { return hash(v.x ^ hash(v.y)); }
	uint hash(uint3 v) { return hash(v.x ^ hash(v.y) ^ hash(v.z)); }
	uint hash(uint4 v) { return hash(v.x ^ hash(v.y) ^ hash(v.z) ^ hash(v.w)); }


	// Construct a float with half-open range [0:1] using low 23 bits.
	// All zeroes yields 0.0, all ones yields the next smallest representable value below 1.0.
	float floatConstruct(uint m) {
		const uint ieeeMantissa = 0x007FFFFFu; // binary32 mantissa bitmask
		const uint ieeeOne = 0x3F800000u; // 1.0 in IEEE binary32

		m &= ieeeMantissa;                     // Keep only mantissa bits (fractional part)
		m |= ieeeOne;                          // Add fractional part to 1.0

		float  f = asfloat(m);       // Range [1:2]
		return f - 1.0;                        // Range [0:1]
	}



	// Pseudo-random value in half-open range [0:1].
	float random(float x) { return floatConstruct(hash(asuint(x))); }
	float random(float2  v) { return floatConstruct(hash(asuint(v))); }
	float random(float3  v) { return floatConstruct(hash(asuint(v))); }
	float random(float4  v) { return floatConstruct(hash(asuint(v))); }

	float4 vec4(float x,float y,float z,float w) { return float4(x,y,z,w); }
	float4 vec4(float x) { return float4(x,x,x,x); }
	float4 vec4(float2 x,float2 y) { return float4(float2(x.x,x.y),float2(y.x,y.y)); }
	float4 vec4(float3 x,float y) { return float4(float3(x.x,x.y,x.z),y); }

	float3 vec3(float x,float y,float z) { return float3(x,y,z); }
	float3 vec3(float x) { return float3(x,x,x); }
	float3 vec3(float2 x,float y) { return float3(float2(x.x,x.y),y); }

	float2 vec2(float x,float y) { return float2(x,y); }
	float2 vec2(float x) { return float2(x,x); }

	float vec(float x) { return float(x); }



	struct VertexInput {
	float4 vertex : POSITION;
	float2 uv:TEXCOORD0;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	//VertexInput
	};
	struct VertexOutput {
	float4 pos : SV_POSITION;
	float2 uv:TEXCOORD0;
	//VertexOutput
	};


	VertexOutput vert(VertexInput v)
	{
	VertexOutput o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	//VertexFactory
	return o;
	}

#define l 120


	fixed4 frag(VertexOutput vertex_output) : SV_Target
	{


	float2 v = (vertex_output.uv - 1 / 2.) / min(1,1) * 30.;
	float2 vv = v;// float2 vvv = v;
	float ft = _Time.x + 360.1;
	float tm = ft * 0.1;
	float tm2 = ft * 0.3;
	float2 mspt = (vec2(
			sin(tm) + cos(tm*0.2) + sin(tm*0.5) + cos(tm*-0.4) + sin(tm*1.3),
			cos(tm) + sin(tm*0.1) + cos(tm*0.8) + sin(tm*-1.1) + cos(tm*1.5)
			) + 1.0)*0.35; //5x harmonics, scale back to [0,1]
	float R = 0.0;
	float RR = 0.0;
	float RRR = 0.0;
	float a = (1. - mspt.x)*0.5;
	float C = cos(tm2*0.03 + a * 0.01)*1.1;
	float S = sin(tm2*0.033 + a * 0.23)*1.1;
	float C2 = cos(tm2*0.024 + a * 0.23)*3.1;
	float S2 = sin(tm2*0.03 + a * 0.01)*3.3;
	float2 xa = vec2(C, -S);
	float2 ya = vec2(S, C);
	float2 xa2 = vec2(C2, -S2);
	float2 ya2 = vec2(S2, C2);
	float2 shift = vec2(0.033, 0.14);
	float2 shift2 = vec2(-0.023, -0.22);
	float Z = 0.4 + mspt.y*0.3;
	float m = 0.99 + (sin(_Time.w) > 0 ? 1 : -1) * 0.00025 / 1000;
	for (int i = 0; i < l; i++) {
		float r = dot(v,v);
		float r2 = dot(vv,vv);
		if (r > 1.0)
		{
			r = (1.0) / r;
			v.x = v.x * r;
			v.y = v.y * r;
		}
		if (r2 > 1.0)
		{
			r2 = (1.0) / r2;
			vv.x = vv.x * r2;
			vv.y = vv.y * r2;
		}
		R *= m;
		R += r;
		R *= m;
		R += r2;
		if (i < l - 1) {
			RR *= m;
			RR += r;
			RR *= m;
			RR += r2;
			if (i < l - 2) {
				RRR *= m;
				RRR += r;
				RRR *= m;
				RRR += r2;
			}
		}

		v = vec2(dot(v, xa), dot(v, ya)) * Z + shift;
		vv = vec2(dot(vv, xa2), dot(vv, ya2)) * Z + shift2;
	}

	float c = ((fmod(R,2.0) > 1.0) ? 1.0 - frac(R) : frac(R));
	float cc = ((fmod(RR,2.0) > 1.0) ? 1.0 - frac(RR) : frac(RR));
	float ccc = ((fmod(RRR,2.0) > 1.0) ? 1.0 - frac(RRR) : frac(RRR));

	float2 uv = (vertex_output.uv - 0.5 * 1) / 1;

	uv *= 3.0;
	float2 gv = frac(uv) - 0.5;
	float3 col = vec3(gv.xy, 1.);

	//float m = 0.;
	float t = _Time.y / 3.9;


	float dist = length(uv * 4.);

	[unroll(100)]
	for (int x = -2; x <= 2; x++) {
		[unroll(100)]
		for (int y = -2; y <= 2; y++) {
			float2 offset = vec2(x, y);
			float d = length(gv - offset * 0.7)*1.75;
			float tNorm = sin(dist - t) * .25 + 0.5;
			float r = lerp(0.35, 1.8, tNorm);

			m += smoothstep(r*1.0001, r, d);
		}
	}

	float rand = random(vec3(vertex_output.uv, _Time.w / 20));

	float inp = 1.1;

	if (_Time.y % 997 < 500) {
		inp = 3.2;
	}

	col.x = fmod(m / 1., c);
	col.y = fmod(m / 1.1, cc);
	col.z = fmod(m / 1.3, ccc);

	return vec4(col,1.0);

	}
	ENDCG
	}
	}
}
