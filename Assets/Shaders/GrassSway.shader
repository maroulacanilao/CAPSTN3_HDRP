Shader "Custom/GrassSwayShaderF"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {} // Make sure you have a texture assigned to this property
        _WindDirection ("Wind Direction", Vector) = (0, 0, 1, 0)
        _WindSpeed ("Wind Speed", Range(0.0, 10.0)) = 1.0
        _SwayAmplitude ("Sway Amplitude", Range(0.0, 1.0)) = 0.1
        _SwayFrequency ("Sway Frequency", Range(0.0, 10.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderPipeline" = "HDRP" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Custom properties
        sampler2D _MainTex;
        float4 _WindDirection;
        float _WindSpeed;
        float _SwayAmplitude;
        float _SwayFrequency;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate time-based wind offset
            float timeOffset = _Time.y * _WindSpeed;
            float2 windOffset = _WindDirection.xy * timeOffset;

            // Calculate vertex sway based on wind direction and frequency
            float sway = sin(dot(IN.worldPos.xz, _WindDirection.xy) * _SwayFrequency + timeOffset) * _SwayAmplitude;

            // Apply vertex displacement
            IN.worldPos += float3(0, sway, 0);

            // Sample the main texture
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

