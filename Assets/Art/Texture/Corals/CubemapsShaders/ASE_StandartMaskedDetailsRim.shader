// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartMaskedDetailsRim"
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
		_MaskedDetailNormals("MaskedDetailNormals", 2D) = "bump" {}
		_BVCNormalsScale("BVCNormalsScale", Float) = 1
		_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalMapDepth("NormalMapDepth", Float) = 1
		[Toggle]_SmoothFromMapSwitch("SmoothFromMapSwitch", Float) = 1
		[Toggle]_EmissionSwitch("EmissionSwitch", Float) = 0
		[Toggle]_SmoothRough("Smooth/Rough", Float) = 0
		_SmoothnessMap("SmoothnessMap", 2D) = "white" {}
		_MetallicMap("MetallicMap", 2D) = "white" {}
		_RimMask("RimMask", 2D) = "white" {}
		_Snoothness("Snoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_RimScale("RimScale", Float) = 1
		_RimColor("RimColor", Color) = (0,0,0,0)
		_MetalicBrightnes("MetalicBrightnes", Range( 0 , 1)) = 0.4494838
		_RimPower("RimPower", Float) = 1
		_BlueVCAlbedo("BlueVCAlbedo", 2D) = "white" {}
		_DetailMask("DetailMask", 2D) = "white" {}
		_BlueVCMask("BlueVCMask", 2D) = "white" {}
		_MaskContrast("MaskContrast", Float) = 1
		_BVCTint("BVCTint", Color) = (0,0,0,0)
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
			float3 worldPos;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldRefl;
		};

		uniform float _DetailNormalsScale;
		uniform sampler2D _DetailNormals;
		uniform float4 _DetailNormals_ST;
		uniform float _NormalMapDepth;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _BVCNormalsScale;
		uniform sampler2D _MaskedDetailNormals;
		uniform float4 _MaskedDetailNormals_ST;
		uniform sampler2D _BlueVCMask;
		uniform float4 _BlueVCMask_ST;
		uniform float _MaskContrast;
		uniform float _MaskIntensity;
		uniform sampler2D _DetailMask;
		uniform float4 _DetailMask_ST;
		uniform float4 _RimColor;
		uniform sampler2D _RimMask;
		uniform float4 _RimMask_ST;
		uniform float _RimScale;
		uniform float _RimPower;
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
		uniform float4 _BVCTint;
		uniform sampler2D _BlueVCAlbedo;
		uniform float4 _BlueVCAlbedo_ST;
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
			float2 uv_MaskedDetailNormals = i.uv_texcoord * _MaskedDetailNormals_ST.xy + _MaskedDetailNormals_ST.zw;
			float2 uv_BlueVCMask = i.uv_texcoord * _BlueVCMask_ST.xy + _BlueVCMask_ST.zw;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			float temp_output_174_0 = ( _MaskIntensity * tex2D( _DetailMask, uv_DetailMask ).b );
			float4 temp_cast_0 = (temp_output_174_0).xxxx;
			float temp_output_121_0 = (CalculateContrast(_MaskContrast,temp_cast_0)).r;
			float blendOpSrc113 = tex2D( _BlueVCMask, uv_BlueVCMask ).r;
			float blendOpDest113 = temp_output_121_0;
			float clampResult156 = clamp( ( ( saturate( (( blendOpSrc113 > 0.5 ) ? ( blendOpDest113 / ( ( 1.0 - blendOpSrc113 ) * 2.0 ) ) : ( 1.0 - ( ( ( 1.0 - blendOpDest113 ) * 0.5 ) / blendOpSrc113 ) ) ) )) + temp_output_174_0 ) , 0.0 , 1.0 );
			float3 lerpResult142 = lerp( BlendNormals( UnpackScaleNormal( tex2D( _DetailNormals, uv_DetailNormals ) ,_DetailNormalsScale ) , tex2DNode13 ) , BlendNormals( UnpackScaleNormal( tex2D( _MaskedDetailNormals, uv_MaskedDetailNormals ) ,_BVCNormalsScale ) , tex2DNode13 ) , clampResult156);
			o.Normal = lerpResult142;
			float2 uv_RimMask = i.uv_texcoord * _RimMask_ST.xy + _RimMask_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV177 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode177 = ( 0.0 + _RimScale * pow( 1.0 - fresnelNdotV177, _RimPower ) );
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
			float2 uv_BlueVCAlbedo = i.uv_texcoord * _BlueVCAlbedo_ST.xy + _BlueVCAlbedo_ST.zw;
			float4 lerpResult110 = lerp( ( temp_output_3_0 + lerpResult90 ) , ( _BVCTint * tex2D( _BlueVCAlbedo, uv_BlueVCAlbedo ) ) , clampResult156);
			float2 uv_AlbedoDetail = i.uv_texcoord * _AlbedoDetail_ST.xy + _AlbedoDetail_ST.zw;
			float4 blendOpSrc157 = lerpResult110;
			float4 blendOpDest157 = tex2D( _AlbedoDetail, uv_AlbedoDetail );
			float4 lerpResult159 = lerp( lerpResult110 , ( saturate(  (( blendOpSrc157 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc157 - 0.5 ) ) * ( 1.0 - blendOpDest157 ) ) : ( 2.0 * blendOpSrc157 * blendOpDest157 ) ) )) , _AlbedoDetailStrenth);
			o.Albedo = ( ( ( _RimColor * ( tex2D( _RimMask, uv_RimMask ) * fresnelNode177 ) ) * temp_output_121_0 ) + lerpResult159 ).rgb;
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
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
7;117;1643;1036;228.9081;2350.213;1.850048;True;True
Node;AmplifyShaderEditor.SamplerNode;172;345.7886,-1905.833;Float;True;Property;_DetailMask;DetailMask;28;0;Create;True;0;0;False;0;7937c4e638262954985abb567fcbb3f4;98dbb13063f37334e8d95ad0c3e13b30;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;173;423.624,-2028.275;Float;False;Property;_MaskIntensity;MaskIntensity;32;0;Create;True;0;0;False;0;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;687.0849,-2028.994;Float;False;Property;_MaskContrast;MaskContrast;30;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;842.2515,-1777.135;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;119;1128.58,-1863.665;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;114;949.486,-2168.446;Float;True;Property;_BlueVCMask;BlueVCMask;29;0;Create;True;0;0;False;0;7937c4e638262954985abb567fcbb3f4;b1b2f5bf729be184b8b7e3008c2bbee1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;121;1346.143,-1861.358;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1594.154,-530.6635;Float;False;Property;_NormalMapDepth;NormalMapDepth;14;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1667.916,-1249.94;Float;False;Property;_DetailNormalsScale;DetailNormalsScale;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-1673.941,-875.2289;Float;False;Property;_BVCNormalsScale;BVCNormalsScale;12;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;113;1613.534,-1866.182;Float;True;VividLight;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;1915,-1765.626;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1291.196,248.5266;Float;True;Property;_SmoothnessMap;SmoothnessMap;18;0;Create;True;0;0;False;0;None;5e5d38792c465fd408f0dd183fd6976c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;126;-1404.541,-1007.782;Float;True;Property;_MaskedDetailNormals;MaskedDetailNormals;11;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;128;-1401.475,-1299.794;Float;True;Property;_DetailNormals;DetailNormals;9;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1412.218,-30.17479;Float;True;Property;_MetallicMap;MetallicMap;19;0;Create;True;0;0;False;0;None;ba3907d1e879168479b21f22c51adf0c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1379.019,-727.4025;Float;True;Property;_NormalMap;NormalMap;13;0;Create;True;0;0;False;0;None;dd90c39b09c838b499c907524dacf8ec;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;127;-1030.704,-1172.533;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-896.6956,186.1265;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;15;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;156;1931.604,-1576.31;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;141;-1034.422,-888.8915;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;12;-658.3749,320.7062;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;142;-761.3278,-676.1829;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldReflectionVector;69;-691.8521,-1106.337;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;21;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-515.8754,163.1064;Float;False;Property;_SmoothRough;Smooth/Rough;17;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-406.1573,-598.7033;Float;False;Property;_AlbedoColor;AlbedoColor;4;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;86;-428.6443,-1101.225;Float;True;Property;_CubemapBlured;CubemapBlured;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;70;-80.62634,-1197.825;Float;False;Property;_CubemapColor;CubemapColor;3;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1387.871,-249.1329;Float;False;Property;_MetalicBrightnes;MetalicBrightnes;25;0;Create;True;0;0;False;0;0.4494838;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1389.108,-135.8561;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-403.4567,-799.6359;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;deba979bff989f74eaddb735b4a7feda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;193.4309,-1369.369;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-391.4181,-1758.392;Float;False;Property;_RimScale;RimScale;23;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;22;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-375.4182,-1630.392;Float;False;Property;_RimPower;RimPower;26;0;Create;True;0;0;False;0;1;2.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;0.9208729,-670.8475;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;93;-1014.058,-29.05231;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;89;-4.434421,10.70831;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-293.0748,8.706559;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;202.1891,-764.209;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;380.866,-975.7206;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;177;-103.4181,-1758.392;Float;False;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;179;120.5819,-1726.392;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;178;-243.4181,-2033.392;Float;True;Property;_RimMask;RimMask;20;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;125;431.1373,-1632.608;Float;False;Property;_BVCTint;BVCTint;31;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;111;400.2443,-1423.314;Float;True;Property;_BlueVCAlbedo;BlueVCAlbedo;27;0;Create;True;0;0;False;0;None;98dbb13063f37334e8d95ad0c3e13b30;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;90;555.6287,-822.0814;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;939.9742,-1065.817;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;181;136.582,-1998.392;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;182;104.5819,-2046.392;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;180;-167.4181,-2270.392;Float;False;Property;_RimColor;RimColor;24;0;Create;True;0;0;False;0;0,0,0,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;902.8975,-1462.574;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;161;1702.682,-803.465;Float;False;Property;_AlbedoDetailStrenth;AlbedoDetailStrenth;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;158;1139.649,-910.8867;Float;True;Property;_AlbedoDetail;AlbedoDetail;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;110;1235.805,-1214.922;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;184;232.582,-2238.392;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;136.582,-2174.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;450.3121,-2218.833;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-483.3045,-288.8951;Float;True;Property;_EmissionMap;EmissionMap;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;163;1920.043,-869.4717;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;160;1838.55,-1134.319;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;157;1595.261,-1081.277;Float;True;HardLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;1493.487,-1563.943;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;159;1914.827,-1081.54;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-4.894272,-515.6068;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;32;319.2034,-471.3975;Float;False;Property;_EmissionSwitch;EmissionSwitch;16;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;276.5654,-630.2753;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;186;2097.524,-1306.072;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;36;828.045,-780.9254;Float;False;Property;_EmissionColor;EmissionColor;7;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;143;-512.23,-594.9559;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;709.4679,-581.6841;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;188;2067.562,-645.1808;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;144;-472.5515,-405.8985;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;187;1464.341,-566.6895;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1125.931,-564.4491;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1511.613,-393.9533;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartMaskedDetailsRim;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;174;0;173;0
WireConnection;174;1;172;3
WireConnection;119;1;174;0
WireConnection;119;0;120;0
WireConnection;121;0;119;0
WireConnection;113;0;114;1
WireConnection;113;1;121;0
WireConnection;155;0;113;0
WireConnection;155;1;174;0
WireConnection;126;5;130;0
WireConnection;128;5;154;0
WireConnection;13;5;66;0
WireConnection;127;0;128;0
WireConnection;127;1;13;0
WireConnection;2;0;5;4
WireConnection;2;1;6;1
WireConnection;156;0;155;0
WireConnection;141;0;126;0
WireConnection;141;1;13;0
WireConnection;12;0;2;0
WireConnection;142;0;127;0
WireConnection;142;1;141;0
WireConnection;142;2;156;0
WireConnection;69;0;142;0
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;86;1;69;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;71;0;86;0
WireConnection;71;1;70;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;93;0;5;1
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;89;0;10;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;75;0;71;0
WireConnection;75;1;89;0
WireConnection;92;0;71;0
WireConnection;92;1;3;0
WireConnection;177;0;142;0
WireConnection;177;2;176;0
WireConnection;177;3;175;0
WireConnection;179;0;177;0
WireConnection;90;0;75;0
WireConnection;90;1;92;0
WireConnection;90;2;7;0
WireConnection;74;0;3;0
WireConnection;74;1;90;0
WireConnection;181;0;179;0
WireConnection;182;0;178;0
WireConnection;124;0;125;0
WireConnection;124;1;111;0
WireConnection;110;0;74;0
WireConnection;110;1;124;0
WireConnection;110;2;156;0
WireConnection;184;0;180;0
WireConnection;183;0;182;0
WireConnection;183;1;181;0
WireConnection;185;0;184;0
WireConnection;185;1;183;0
WireConnection;163;0;161;0
WireConnection;160;0;110;0
WireConnection;157;0;110;0
WireConnection;157;1;158;0
WireConnection;189;0;185;0
WireConnection;189;1;121;0
WireConnection;159;0;160;0
WireConnection;159;1;157;0
WireConnection;159;2;163;0
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;186;0;189;0
WireConnection;186;1;159;0
WireConnection;143;0;142;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;188;0;186;0
WireConnection;144;0;143;0
WireConnection;187;0;188;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;0;0;187;0
WireConnection;0;1;144;0
WireConnection;0;2;37;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=08D31D7D423F73B0C28ED68D8ED43456CDC68B12