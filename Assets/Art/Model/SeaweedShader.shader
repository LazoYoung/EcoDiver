// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X
Shader "ASE/SeaweedShader"
{
    Properties
    {
        _Cutoff("Mask Clip Value", Float) = 0.5
        _Albedo("Albedo", 2D) = "white" {}
        _AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
        _EmissionMap("EmissionMap", 2D) = "white" {}
        _EmissionColor("EmissionColor", Color) = (0,0,0,0)
        _EmissionMultiplayer("EmissionMultiplayer", Float) = 0
        _NormalMap("NormalMap", 2D) = "bump" {}
        [Toggle]_SmoothFromMapSwitch("SmoothFromMapSwitch", Float) = 1
        [Toggle]_EmissionSwitch("EmissionSwitch", Float) = 1
        [Toggle]_SmoothRough("Smooth/Rough", Float) = 0
        _SmoothnessMap("SmoothnessMap", 2D) = "white" {}
        _MetallicMap("MetallicMap", 2D) = "white" {}
        _Smoothness("Smoothness", Float) = 1
        _Metallic("Metallic", Float) = 1
        _NormalMapDepth("NormalMapDepth", Float) = 1
        _TransmisionValue("TransmisionValue", Float) = 1
        _TransmissionColor("TransmissionColor", Color) = (1,1,1,0)
        _SwaySpeed("Sway Speed", Float) = 1.0
        _SwayAmplitude("Sway Amplitude", Float) = 0.1
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true" }
        Cull Off
        CGPROGRAM
        #include "UnityStandardUtils.cginc"
        #include "UnityPBSLighting.cginc"
        #pragma target 3.0
        #pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred

        struct Input
        {
            float2 uv_texcoord;
            float3 worldPos; // Added to get the world position of the vertex
        };

        struct SurfaceOutputStandardCustom
        {
            fixed3 Albedo;
            fixed3 Normal;
            half3 Emission;
            half Metallic;
            half Smoothness;
            half Occlusion;
            fixed Alpha;
            fixed3 Transmission;
        };

        uniform float _NormalMapDepth;
        uniform sampler2D _NormalMap;
        uniform float4 _NormalMap_ST;
        uniform sampler2D _Albedo;
        uniform float4 _Albedo_ST;
        uniform float4 _AlbedoColor;
        uniform float4 _EmissionColor;
        uniform float _EmissionMultiplayer;
        uniform float _EmissionSwitch;
        uniform sampler2D _EmissionMap;
        uniform float4 _EmissionMap_ST;
        uniform float _Metallic;
        uniform sampler2D _MetallicMap;
        uniform float4 _MetallicMap_ST;
        uniform float _SmoothRough;
        uniform float _SmoothFromMapSwitch;
        uniform sampler2D _SmoothnessMap;
        uniform float4 _SmoothnessMap_ST;
        uniform float _Smoothness;
        uniform float _TransmisionValue;
        uniform float4 _TransmissionColor;
        uniform float _Cutoff = 0.5;
        uniform float _SwaySpeed;
        uniform float _SwayAmplitude;

        inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi)
        {
            half3 transmission = max(0, -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
            half4 d = half4(s.Albedo * transmission, 0);

            SurfaceOutputStandard r;
            r.Albedo = s.Albedo;
            r.Normal = s.Normal;
            r.Emission = s.Emission;
            r.Metallic = s.Metallic;
            r.Smoothness = s.Smoothness;
            r.Occlusion = s.Occlusion;
            r.Alpha = s.Alpha;
            return LightingStandard(r, viewDir, gi) + d;
        }

        inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi)
        {
            #if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
                gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
            #else
                UNITY_GLOSSY_ENV_FROM_SURFACE(g, s, data);
                gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);
            #endif
        }

        void surf(Input i, inout SurfaceOutputStandardCustom o)
        {
            float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
            o.Normal = UnpackScaleNormal(tex2D(_NormalMap, uv_NormalMap), _NormalMapDepth);

            // Calculate sway offset
            float sway = sin(_Time.y * _SwaySpeed + i.worldPos.y) * _SwayAmplitude;
            float3 swayOffset = float3(sway, 0, sway);

            // Apply sway offset to vertex position
            float2 uv_Albedo = (i.uv_texcoord + swayOffset.xy) * _Albedo_ST.xy + _Albedo_ST.zw;
            float4 tex2DNode1 = tex2D(_Albedo, uv_Albedo);
            float4 temp_output_3_0 = (tex2DNode1 * _AlbedoColor);
            o.Albedo = temp_output_3_0.rgb;

            float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
            float4 tex2DNode33 = tex2D(_EmissionMap, uv_EmissionMap);
            o.Emission = (_EmissionColor * (_EmissionMultiplayer * lerp(tex2DNode33, (temp_output_3_0 * tex2DNode33.a), _EmissionSwitch))).rgb;

            float2 uv_MetallicMap = i.uv_texcoord * _MetallicMap_ST.xy + _MetallicMap_ST.zw;
            float4 tex2DNode5 = tex2D(_MetallicMap, uv_MetallicMap);
            o.Metallic = (_Metallic * tex2DNode5.r);

            float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
            o.Smoothness = (lerp(lerp(tex2DNode5.a, tex2D(_SmoothnessMap, uv_SmoothnessMap).r, _SmoothFromMapSwitch), (1.0 - lerp(tex2DNode5.a, tex2D(_SmoothnessMap, uv_SmoothnessMap).r, _SmoothFromMapSwitch)), _SmoothRough) * _Smoothness);

            o.Transmission = (_TransmisionValue * _TransmissionColor).rgb;
            o.Alpha = 1;
            clip(tex2DNode1.a - _Cutoff);
        }

        ENDCG
    }
    Fallback "Diffuse"
    CustomEditor "ASEMaterialInspector"
}
