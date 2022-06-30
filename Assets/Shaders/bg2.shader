
Shader "ShaderMan/MyShader"
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




	fixed4 frag(VertexOutput vertex_output) : SV_Target
	{

	float2 uv = (vertex_output.uv - 0.5 * 1) / 1;

	uv *= 3.0;
	float2 gv = frac(uv) - 0.5;
	float3 col = vec3(gv.xy, 1.);

	float m = 0.;
	float t = _Time.y / 2.;


	float dist = length(uv * 4.);
	[unroll(100)]
for (int x = -2; x <= 2; x++) {
		[unroll(100)]
for (int y = -2; y <= 2; y++) {
			float2 offset = vec2(x, y);
			float d = length(gv - offset * 0.7)*1.75;
			float tNorm = sin(dist - t) * .5 + 0.5;
			float r = lerp(0.35, 1.8, tNorm);

			m += smoothstep(r*1.0001, r, d);
		}
	}

	col.x = fmod(m / 1., 1.1);
	col.y = fmod(m / 1.1, 1.3);
	col.z = fmod(m / 1.3, 1.4);

	return vec4(col,1.0);

	}
	ENDCG
	}
	}
}
