Shader "Custom/SeeThroughShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 0.5)
        _Transparency ("Transparency", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };
            
            float4 _Color;
            float _Transparency;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            half4 frag(v2f i) : SV_Target
            {
                half4 finalColor = _Color;
                finalColor.a *= _Transparency;
                
                return finalColor;
            }
            
            ENDCG
        }
    }
}
