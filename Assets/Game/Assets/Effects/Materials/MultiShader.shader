Shader "Common/TwoUvsLayeredTexture"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        ZWrite Off Lighting Off Cull Off Fog{ Mode Off } Blend SrcAlpha OneMinusSrcAlpha
        LOD 110

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_vct
            #pragma fragment frag_mult 
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            sampler2D _Texture;

            struct vin_vct
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct v2f_vct
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            v2f_vct vert_vct(vin_vct v)
            {
                v2f_vct o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord0 = v.texcoord0;
                o.texcoord1 = v.texcoord1;
                return o;
            }

            fixed4 frag_mult(v2f_vct i) : SV_Target
            {
                fixed4 col1 = tex2D(_Texture, i.texcoord0) * i.color;
                fixed4 col2 = tex2D(_Texture, i.texcoord1) * i.color;

                fixed4 output;
                output.rgb = (col1.rgb * (1.0f - col2.a)) + (col2.rgb * col2.a);
                output.a = min(col1.a + col2.a, 1.0f);
                return output;
            }

            ENDCG
        }
    }
}