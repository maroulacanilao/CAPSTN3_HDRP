Shader "Custom/SelectiveBlurTextShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurStart ("Blur Start", Range(0, 1)) = 0.3
        _BlurEnd ("Blur End", Range(0, 1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float _BlurStart;
            float _BlurEnd;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                float blurFactor = smoothstep(_BlurStart, _BlurEnd, i.uv.x);
                // Apply the blur effect by adjusting the alpha component based on the blurFactor
                fixed4 blurredColor = color;
                blurredColor.a *= blurFactor;
                return blurredColor;
            }
            ENDCG
        }
    }
}

