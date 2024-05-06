// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartMaskedDetails"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoDetail("AlbedoDetail", 2D) = "white" {}
		_AlbedoDetailStrenth("AlbedoDetailStrenth", Float) = 0
		_CubemapColor("CubemapColor", Color) = (0,0,0,1)
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		_CubemapBlured("CubemapBlured", CUBE) = "white" {}
		_EmissionMap("EmissionMap", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMultiplayer("EmissionMultiplayer", Float) = 0
		_DetailNormals("DetailNormals", 2D) = "bump" {}
		_DetailNormalsScale("DetailNormalsScale", Float) = 1
		_BVCNormals("BVCNormals", 2D) = "bump" {}
		_BVCNormalsScale("BVCNormalsScale", Float) = 1
		_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalMapDepth("NormalMapDepth", Float) = 1
		[Toggle]_SmoothFromMapSwitch("SmoothFromMapSwitch", Float) = 1
		[Toggle]_EmissionSwitch("EmissionSwitch", Float) = 0
		[Toggle]_SmoothRough("Smooth/Rough", Float) = 0
		_SmoothnessMap("SmoothnessMap", 2D) = "white" {}
		_MetallicMap("MetallicMap", 2D) = "white" {}
		_Snoothness("Snoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_MetalicBrightnes("MetalicBrightnes", Range( 0 , 1)) = 0.4494838
		_DetailMask("DetailMask", 2D) = "white" {}
		_MaskContrast("MaskContrast", Float) = 1
		_MaskIntensity("MaskIntensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
		};

		uniform float _DetailNormalsScale;
		uniform sampler2D _DetailNormals;
		uniform float4 _DetailNormals_ST;
		uniform float _NormalMapDepth;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _BVCNormalsScale;
		uniform sampler2D _BVCNormals;
		uniform float4 _BVCNormals_ST;
		uniform float _MaskContrast;
		uniform float _MaskIntensity;
		uniform sampler2D _DetailMask;
		uniform float4 _DetailMask_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AlbedoColor;
		uniform samplerCUBE _CubemapBlured;
		uniform float4 _CubemapColor;
		uniform float _SmoothRough;
		uniform float _SmoothFromMapSwitch;
		uniform sampler2D _MetallicMap;
		uniform float4 _MetallicMap_ST;
		uniform sampler2D _SmoothnessMap;
		uniform float4 _SmoothnessMap_ST;
		uniform float _Snoothness;
		uniform float _Metallic;
		uniform float _MetalicBrightnes;
		uniform sampler2D _AlbedoDetail;
		uniform float4 _AlbedoDetail_ST;
		uniform float _AlbedoDetailStrenth;
		uniform float4 _EmissionColor;
		uniform float _EmissionMultiplayer;
		uniform float _EmissionSwitch;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_DetailNormals = i.uv_texcoord * _DetailNormals_ST.xy + _DetailNormals_ST.zw;
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode13 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ) ,_NormalMapDepth );
			float2 uv_BVCNormals = i.uv_texcoord * _BVCNormals_ST.xy + _BVCNormals_ST.zw;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			float4 temp_cast_0 = (( _MaskIntensity * tex2D( _DetailMask, uv_DetailMask ).b )).xxxx;
			float4 clampResult156 = clamp( CalculateContrast(_MaskContrast,temp_cast_0) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float3 lerpResult142 = lerp( BlendNormals( UnpackScaleNormal( tex2D( _DetailNormals, uv_DetailNormals ) ,_DetailNormalsScale ) , tex2DNode13 ) , BlendNormals( UnpackScaleNormal( tex2D( _BVCNormals, uv_BVCNormals ) ,_BVCNormalsScale ) , tex2DNode13 ) , clampResult156.r);
			o.Normal = lerpResult142;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _Albedo, uv_Albedo ) * _AlbedoColor );
			float4 temp_output_71_0 = ( texCUBE( _CubemapBlured, WorldReflectionVector( i , lerpResult142 ) ) * _CubemapColor );
			float2 uv_MetallicMap = i.uv_texcoord * _MetallicMap_ST.xy + _MetallicMap_ST.zw;
			float4 tex2DNode5 = tex2D( _MetallicMap, uv_MetallicMap );
			float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
			float temp_output_10_0 = ( lerp(lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch),( 1.0 - lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch) ),_SmoothRough) * _Snoothness );
			float lerpResult93 = lerp( tex2DNode5.r , 1.0 , _MetalicBrightnes);
			float temp_output_7_0 = ( _Metallic * lerpResult93 );
			float4 lerpResult90 = lerp( ( temp_output_71_0 * saturate( temp_output_10_0 ) ) , ( temp_output_71_0 * temp_output_3_0 ) , temp_output_7_0);
			float4 temp_output_74_0 = ( temp_output_3_0 + lerpResult90 );
			float2 uv_AlbedoDetail = i.uv_texcoord * _AlbedoDetail_ST.xy + _AlbedoDetail_ST.zw;
			float4 blendOpSrc157 = temp_output_74_0;
			float4 blendOpDest157 = tex2D( _AlbedoDetail, uv_AlbedoDetail );
			float4 lerpResult159 = lerp( temp_output_74_0 , ( saturate(  (( blendOpSrc157 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc157 - 0.5 ) ) * ( 1.0 - blendOpDest157 ) ) : ( 2.0 * blendOpSrc157 * blendOpDest157 ) ) )) , ( clampResult156.r * _AlbedoDetailStrenth ));
			o.Albedo = lerpResult159.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 tex2DNode33 = tex2D( _EmissionMap, uv_EmissionMap );
			o.Emission = ( _EmissionColor * ( _EmissionMultiplayer * lerp(tex2DNode33,( temp_output_3_0 * tex2DNode33.a ),_EmissionSwitch) ) ).rgb;
			o.Metallic = temp_output_7_0;
			o.Smoothness = temp_output_10_0;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldRefl = -worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
401.3333;121.3333;1755;1029;5889.704;3031.027;5.148986;True;True
Node;AmplifyShaderEditor.RangedFloatNode;173;423.624,-2028.275;Float;False;Property;_MaskIntensity;MaskIntensity;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;172;345.7886,-1905.833;Float;True;Property;_DetailMask;DetailMask;23;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;120;687.0849,-2028.994;Float;False;Property;_MaskContrast;MaskContrast;24;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;830.0134,-1668.521;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;119;1128.58,-1863.665;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-1673.941,-875.2289;Float;False;Property;_BVCNormalsScale;BVCNormalsScale;12;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1594.154,-530.6635;Float;False;Property;_NormalMapDepth;NormalMapDepth;14;0;Create;True;0;0;False;0;1;0.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1667.916,-1249.94;Float;False;Property;_DetailNormalsScale;DetailNormalsScale;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;128;-1401.475,-1299.794;Float;True;Property;_DetailNormals;DetailNormals;9;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;156;1341.945,-1576.31;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-1379.019,-727.4025;Float;True;Property;_NormalMap;NormalMap;13;0;Create;True;0;0;False;0;None;7e98918081b969a47a7c0f4ffca8aad7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1291.196,248.5266;Float;True;Property;_SmoothnessMap;SmoothnessMap;18;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1412.218,-30.17479;Float;True;Property;_MetallicMap;MetallicMap;19;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;126;-1404.541,-1007.782;Float;True;Property;_BVCNormals;BVCNormals;11;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-896.6956,186.1265;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;15;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;175;1556.699,-1525.966;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BlendNormalsNode;127;-1030.704,-1172.533;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;141;-1034.422,-888.8915;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;12;-658.3749,320.7062;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;142;-761.3278,-676.1829;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldReflectionVector;69;-691.8521,-1106.337;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-515.8754,163.1064;Float;False;Property;_SmoothRough;Smooth/Rough;17;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;20;0;Create;True;0;0;False;0;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-406.1573,-598.7033;Float;False;Property;_AlbedoColor;AlbedoColor;4;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;70;-80.62634,-1021.878;Float;False;Property;_CubemapColor;CubemapColor;3;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;97;-1389.108,-135.8561;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-403.4567,-799.6359;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;16ed5cbe27133bc438937b9ce71815dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;95;-1387.871,-249.1329;Float;False;Property;_MetalicBrightnes;MetalicBrightnes;22;0;Create;True;0;0;False;0;0.4494838;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;86;-428.6443,-1101.225;Float;True;Property;_CubemapBlured;CubemapBlured;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;209.2819,-1120.51;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;89;-4.434421,10.70831;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;93;-1014.058,-29.05231;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;0.9208729,-670.8475;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;200.604,-860.8999;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-293.0748,8.706559;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;380.866,-975.7206;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;90;630.1284,-834.7624;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;910.0557,-1155.571;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;161;1620.005,-1258.188;Float;False;Property;_AlbedoDetailStrenth;AlbedoDetailStrenth;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;158;1139.649,-910.8867;Float;True;Property;_AlbedoDetail;AlbedoDetail;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-483.3045,-288.8951;Float;True;Property;_EmissionMap;EmissionMap;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;1968.498,-1355.245;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-4.894272,-515.6068;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;157;1528.264,-1008.578;Float;True;HardLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;159;2017.152,-1115.798;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;32;319.2034,-471.3975;Float;False;Property;_EmissionSwitch;EmissionSwitch;16;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;276.5654,-630.2753;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;709.4679,-581.6841;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;143;-512.23,-594.9559;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;36;828.045,-780.9254;Float;False;Property;_EmissionColor;EmissionColor;7;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;164;2040.428,-663.8912;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1125.931,-564.4491;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;162;1499.621,-613.8837;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;144;-472.5515,-405.8985;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1511.613,-393.9533;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartMaskedDetails;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;174;0;173;0
WireConnection;174;1;172;3
WireConnection;119;1;174;0
WireConnection;119;0;120;0
WireConnection;128;5;154;0
WireConnection;156;0;119;0
WireConnection;13;5;66;0
WireConnection;126;5;130;0
WireConnection;2;0;5;4
WireConnection;2;1;6;1
WireConnection;175;0;156;0
WireConnection;127;0;128;0
WireConnection;127;1;13;0
WireConnection;141;0;126;0
WireConnection;141;1;13;0
WireConnection;12;0;2;0
WireConnection;142;0;127;0
WireConnection;142;1;141;0
WireConnection;142;2;175;0
WireConnection;69;0;142;0
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;86;1;69;0
WireConnection;71;0;86;0
WireConnection;71;1;70;0
WireConnection;89;0;10;0
WireConnection;93;0;5;1
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;75;0;71;0
WireConnection;75;1;89;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;92;0;71;0
WireConnection;92;1;3;0
WireConnection;90;0;75;0
WireConnection;90;1;92;0
WireConnection;90;2;7;0
WireConnection;74;0;3;0
WireConnection;74;1;90;0
WireConnection;176;0;175;0
WireConnection;176;1;161;0
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;157;0;74;0
WireConnection;157;1;158;0
WireConnection;159;0;74;0
WireConnection;159;1;157;0
WireConnection;159;2;176;0
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;143;0;142;0
WireConnection;164;0;159;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;162;0;164;0
WireConnection;144;0;143;0
WireConnection;0;0;162;0
WireConnection;0;1;144;0
WireConnection;0;2;37;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=757BF0F24A3B1449A5A66F42BAAF8006B882C02B