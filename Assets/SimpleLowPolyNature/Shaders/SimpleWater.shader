Shader "URP/Custom/SimpleWaterURP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.6, 0.9, 1, 1)
        _WaterNormal("Water Normal", 2D) = "bump" {}
        _NormalScale("Normal Scale", Float) = 1
        _Smoothness("Smoothness", Range(0,1)) = 0.8
        _WavesAmplitude("Waves Amplitude", Float) = 0.05
        _WavesAmount("Waves Amount", Float) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_WaterNormal); SAMPLER(sampler_WaterNormal);
            float4 _BaseColor;
            float _NormalScale;
            float _Smoothness;
            float _WavesAmplitude;
            float _WavesAmount;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float wave = sin(IN.positionOS.z * _WavesAmount + _Time.y) * _WavesAmplitude;
                float3 pos = IN.positionOS.xyz + IN.normalOS * wave;
                OUT.positionHCS = TransformObjectToHClip(pos);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, IN.uv));
                float3 normalWS = normalize(IN.normalWS + normalTS * _NormalScale);

                float3 viewDirWS = normalize(_WorldSpaceCameraPos - TransformObjectToWorld(IN.positionHCS.xyz));
                float3 lightDirWS = _MainLightPosition.xyz;
                float3 lightColor = _MainLightColor.rgb;

                float NdotL = max(0, dot(normalWS, lightDirWS));
                float3 diffuse = _BaseColor.rgb * lightColor * NdotL;

                float3 reflectDir = reflect(-lightDirWS, normalWS);
                float spec = pow(max(dot(viewDirWS, reflectDir), 0.0), 32.0) * _Smoothness;

                return float4(diffuse + spec, _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
