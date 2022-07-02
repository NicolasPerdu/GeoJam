Shader "Custom/4DTimeo"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SegmentCount("Segment Count", Float) = 4
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		float _SegmentCount;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        float4 frag (v2f i) : SV_Target
        {
            // Convert to polar coordinates.
            float2 shiftUV = i.uv - 0.5 ;
            float radius = sqrt(dot(shiftUV, shiftUV));
            float angle = atan2(shiftUV.y, shiftUV.x) + _Time.w;

            // Calculate segment angle amount.
            float segmentAngle = UNITY_TWO_PI / _SegmentCount - sin(_Time.x) * 100;

            // Calculate which segment this angle is in.
            angle -= segmentAngle * floor(angle / segmentAngle);

            // Each segment contains one reflection.
            angle = min(angle, segmentAngle - angle);

            // Convert back to UV coordinates.
            float2 uv = float2(cos(angle), sin(angle)) * radius + 0.5f;

            // Reflect outside the inner circle boundary.
            uv = max(min(uv, 2.0 - uv), -uv);

            return tex2D(_MainTex, uv);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }


        
        ENDCG
    }
    FallBack "Diffuse"
}
