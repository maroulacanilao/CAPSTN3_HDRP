Shader "Custom/GrassSwayShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SwaySpeed ("Sway Speed", Range(0, 10)) = 1.0
        _SwayAmplitude ("Sway Amplitude", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _SwaySpeed;
            float _SwayAmplitude;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate sway displacement using sine wave
                float swayOffset = sin(i.uv.x * _SwaySpeed + _Time.y) * _SwayAmplitude;

                // Apply sway offset to the UV coordinate
                float2 uvOffset = float2(0, swayOffset);
                float2 uv = i.uv + uvOffset;

                // Sample the main texture
                fixed4 color = tex2D(_MainTex, uv);

                // Apply alpha cutoff to create transparent cutout effect
                clip(color.a - 0.5);

                return color;
            }
            ENDCG
        }
    }
}
