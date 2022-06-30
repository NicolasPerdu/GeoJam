
Shader "ShaderMan/Beginning"
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
	float4 vec4a2(float2 x,float2 y) { return float4(float2(x.x,x.y),float2(y.x,y.y)); }
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

	float3 hsl2rgb(in float3 c)
{
	float3 rgb = clamp(abs(fmod(c.x*6.0 + vec3(0.0,4.0,2.0),6.0) - 3.0) - 1.0, 0.0, 1.0);

	return c.z + c.y * (rgb - 0.5)*(1.0 - abs(2.0*c.z - 1.0));
}

float3 HueShift(in float3 Color, in float Shift)
{
	float3 P = vec3(0.55735)*dot(vec3(0.55735),Color);

	float3 U = Color - P;

	float3 V = cross(vec3(0.55735),U);

	Color = U * cos(Shift*6.2832) + V * sin(Shift*6.2832) + P;

	return vec3(Color);
}

float3 rgb2hsl(in float3 c) {
  float h = 0.0;
	float s = 0.0;
	float l = 0.0;
	float r = c.r;
	float g = c.g;
	float b = c.b;
	float cMin = min(r, min(g, b));
	float cMax = max(r, max(g, b));

	l = (cMax + cMin) / 2.0;
	if (cMax > cMin) {
		float cDelta = cMax - cMin;

		//s = l < .05 ? cDelta / ( cMax + cMin ) : cDelta / ( 2.0 - ( cMax + cMin ) ); Original
		s = l < .0 ? cDelta / (cMax + cMin) : cDelta / (2.0 - (cMax + cMin));

		if (r == cMax) {
			h = (g - b) / cDelta;
		}
 else if (g == cMax) {
  h = 2.0 + (b - r) / cDelta;
}
else {
 h = 4.0 + (r - g) / cDelta;
}

if (h < 0.0) {
	h += 6.0;
}
h = h / 6.0;
}
return vec3(h, s, l);
}

float3 rgb2hsv(float3 c)
{
	float4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = lerp(vec4a2(c.bg, K.wz), vec4a2(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(vec4a2(p.xyw, c.r), vec4a2(c.r, p.yzx), step(p.x, c.r));

	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 c)
{
	float4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}




	fixed4 frag(VertexOutput vertex_output) : SV_Target
	{

	float time = _Time.y;
	float2 uv = vertex_output.uv / 1;
	//float sintime = (sin(time)/2.)+0.5;
	float3 rainbow = hsl2rgb(vec3(uv.x + time/7.0,1,uv.y));
	//rainbow = rgb2hsl(rainbow);
	//rainbow = hsl2rgb(rainbow);
	return vec4(rainbow,1.0);

	}
	ENDCG
	}
	}
}
