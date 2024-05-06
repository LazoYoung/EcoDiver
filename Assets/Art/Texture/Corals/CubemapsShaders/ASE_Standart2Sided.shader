// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartCutout2Sided"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
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
		_Snoothness("Snoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_NormalMapDepth("NormalMapDepth", Float) = 1
		_TransmisionValue("TransmisionValue", Float) = 1
		_TransmissionColor("TransmissionColor", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
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
		uniform float _Snoothness;
		uniform float _TransmisionValue;
		uniform float4 _TransmissionColor;
		uniform float _Cutoff = 0.5;

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ) ,_NormalMapDepth );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo );
			float4 temp_output_3_0 = ( tex2DNode1 * _AlbedoColor );
			o.Albedo = temp_output_3_0.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 tex2DNode33 = tex2D( _EmissionMap, uv_EmissionMap );
			o.Emission = ( _EmissionColor * ( _EmissionMultiplayer * lerp(tex2DNode33,( temp_output_3_0 * tex2DNode33.a ),_EmissionSwitch) ) ).rgb;
			float2 uv_MetallicMap = i.uv_texcoord * _MetallicMap_ST.xy + _MetallicMap_ST.zw;
			float4 tex2DNode5 = tex2D( _MetallicMap, uv_MetallicMap );
			o.Metallic = ( _Metallic * tex2DNode5.r );
			float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
			o.Smoothness = ( lerp(lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch),( 1.0 - lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch) ),_SmoothRough) * _Snoothness );
			o.Transmission = ( _TransmisionValue * _TransmissionColor ).rgb;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14301
37;578;1622;797;1835.664;879.8256;2.174937;True;True
Node;AmplifyShaderEditor.ColorNode;4;-407.9034,-673.7851;Float;False;Property;_AlbedoColor;AlbedoColor;2;0;Create;True;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-331.8671,-927.1003;Float;True;Property;_Albedo;Albedo;1;0;Create;True;None;2d34ca855e5f6cf4991f393e1a354fe7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1291.196,248.5266;Float;True;Property;_SmoothnessMap;SmoothnessMap;10;0;Create;True;None;3351c56a873b04b4a88622baabbe1dec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1285.996,5.426482;Float;True;Property;_MetallicMap;MetallicMap;11;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;0.9208739,-662.1165;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-352.3483,-472.2335;Float;True;Property;_EmissionMap;EmissionMap;3;0;Create;True;None;18395a2d46776b8479f778c2f1e0f7fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;191.1603,-705.4803;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-896.6956,186.1265;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;7;0;Create;True;1;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;420.8224,-589.8196;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;5;0;Create;True;0;12.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;32;280.0221,-432.5562;Float;False;Property;_EmissionSwitch;EmissionSwitch;8;0;Create;True;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;12;-658.3749,320.7062;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;588.9876,-480.4108;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1428.504,-467.3266;Float;False;Property;_NormalMapDepth;NormalMapDepth;14;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;684.2137,-982.8802;Float;False;Property;_EmissionColor;EmissionColor;4;0;Create;True;0,0,0,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;13;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-318.9572,521.3286;Float;False;Property;_TransmisionValue;TransmisionValue;15;0;Create;True;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-326.7983,623.955;Float;False;Property;_TransmissionColor;TransmissionColor;16;0;Create;True;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;12;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-508.8754,175.1064;Float;False;Property;_SmoothRough;Smooth/Rough;9;0;Create;True;0;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-293.0748,8.706559;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-778.8251,-414.4076;Float;True;Property;_NormalMap;NormalMap;6;0;Create;True;None;66b3acac97fc66e44a8c584f4661ccb1;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;903.0311,-401.3935;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-80.79828,611.955;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-24.70001,49.40001;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartCutout2Sided;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;2;0;5;4
WireConnection;2;1;6;1
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;12;0;2;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;7;0;8;0
WireConnection;7;1;5;1
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;13;5;66;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;0;0;3;0
WireConnection;0;1;13;0
WireConnection;0;2;37;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
WireConnection;0;6;70;0
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=C0741E84B6FD658C97F32D00EAC97E0754A7CE41