Shader "Custom/Foil"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _FrameTex;
            float _ShinePosition;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            float4 Blend(float4 a, float4 b, float t)
            {
                float4 r = a;
                r.a = b.a + a.a * (1 - b.a);
                r.rgb = (b.rgb * b.a + a.rgb * a.a * (1 - b.a)) * (r.a + 0.0000001);
                r.a = saturate(r.a);
                return lerp(a, r, t);
            }

            float4 Parallax(float2 uv, sampler2D source, float intensity)
            {
                float x = (-unity_ObjectToWorld[0][2] * 0.1);
                float4 rgb = tex2D(source, uv + float2(x, 0));
                float r = tex2D(source, uv + float2(x * (1 - intensity), 0)).r;
                float b = tex2D(source, uv + float2(x * (1 + intensity), 0)).b;
                return float4(r, rgb.g, b, rgb.a);
            }

            float4 Foil(float4 source, float2 uv, float speed, float t)
            {
                float a = 2.0 * uv.x;
                float b = sin((_Time.y * speed * 2.2) + 1.1 + a) + sin((_Time.y * speed * 1.8) + 0.5 - a) + sin((_Time.y * speed * 1.5) + 8.2 + 2.0 * uv.y) + sin((_Time.y * speed * 2.0) + 595 + 5.0 * uv.y);
                float c = ((5.0 + b) / 5.0) - floor(((5.0 + b) / 5.0));
                float d = c + source.r * 0.2 + source.g * 0.4 + source.b * 0.2;
                d = (d - floor(d)) * 8;
                return lerp(source, float4(clamp(d - 4.0, 0.0, 1.0) + clamp(2.0 - d, 0.0, 1.0), d < 2.0 ? clamp(d, 0.0, 1.0) : clamp(4.0 - d, 0.0, 1.0), d < 4.0 ? clamp(d - 2.0, 0.0, 1.0) : clamp(6.0 - d, 0.0, 1.0), source.a), t);
            }

            float4 Shine(float2 uv, float position, float size, float smoothing, float intensity)
            {
                uv = uv - float2(position + 0.5, 0.5);
                float a = atan2(uv.x, uv.y) + 1.4;
                float r = 3.1415;
                float c = cos(floor(0.5 + a / r) * r - a) * length(uv);
                float d = 1.0 - smoothstep(size, size + smoothing, c);
                return d * intensity;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                //return color;

                float4 art = Blend(Parallax(IN.texcoord, _MainTex, 0.2), Foil(float4(1, 1, 1, 1), IN.texcoord, 0.6, 0.5), 0.2);
                art = Blend(art, Shine(IN.texcoord, _ShinePosition, -0.1, 0.3, 0.6), 1.0);
                float4 r = Blend(art, tex2D(_FrameTex, IN.texcoord), 1.0);
                r.rgb *= IN.color.rgb;
                r.a = r.a * IN.color.a;

                r.a = color.a;

                return r;
                
            }
        ENDCG
        }
    }
}