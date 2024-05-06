// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartBlend"
{
	Properties
	{
		_BlendContrast("BlendContrast", Float) = 1
		_AlbedoMapArray("AlbedoMapArray", 2DArray ) = "" {}
		_MetallicMapArray("MetallicMapArray", 2DArray ) = "" {}
		_HeightMapBase("HeightMapBase", 2D) = "white" {}
		_ParalaxOffsetBase("ParalaxOffsetBase", Float) = 0.001
		_PrlxRefPlaneBase("PrlxRefPlaneBase", Range( 0 , 1)) = 0.001
		_NormalMapBase("NormalMapBase", 2D) = "bump" {}
		_NormalMapDepthBase("NormalMapDepthBase", Float) = 1
		_TillingBase("TillingBase", Float) = 1
		_AlbedoColorBase("AlbedoColorBase", Color) = (1,1,1,1)
		_SmoothnessBase("SmoothnessBase", Float) = 1
		_MetallicBase("MetallicBase", Float) = 1
		_MetalicBrightnesBase("MetalicBrightnesBase", Range( 0 , 1)) = 0
		_HeightMap1("HeightMap1", 2D) = "white" {}
		_ParalaxOffset1("ParalaxOffset1", Float) = 0.001
		_PrlxRefPlane1("PrlxRefPlane1", Range( 0 , 1)) = 0.001
		_Height1("Height1", Float) = 2
		_NormalMap1("NormalMap1", 2D) = "bump" {}
		_NormalMapDepth1("NormalMapDepth1", Float) = 1
		_Tilling1("Tilling1", Float) = 1
		_AlbedoColor1("AlbedoColor1", Color) = (1,1,1,1)
		_Smoothness1("Smoothness1", Float) = 1
		_Metallic1("Metallic1", Float) = 1
		_MetalicBrightnes1("MetalicBrightnes1", Range( 0 , 1)) = 0
		_HeightMap2("HeightMap2", 2D) = "white" {}
		_ParalaxOffset2("ParalaxOffset2", Float) = 0.001
		_PrlxRefPlane2("PrlxRefPlane2", Range( 0 , 1)) = 0.001
		_Height2("Height2", Float) = 2
		_NormalMap2("NormalMap2", 2D) = "bump" {}
		_NormalMapDepth2("NormalMapDepth2", Float) = 1
		_Tilling2("Tilling2", Float) = 1
		_AlbedoColor2("AlbedoColor2", Color) = (1,1,1,1)
		_Smoothness2("Smoothness2", Float) = 1
		_Metallic2("Metallic2", Float) = 1
		_MetalicBrightnes2("MetalicBrightnes2", Range( 0 , 1)) = 0
		_HeightMap3("HeightMap3", 2D) = "white" {}
		_ParalaxOffset3("ParalaxOffset3", Float) = 0.001
		_PrlxRefPlane3("PrlxRefPlane3", Range( 0 , 1)) = 0.001
		_Height3("Height3", Float) = 2
		_NormalMap3("NormalMap3", 2D) = "bump" {}
		_NormalMapDepth3("NormalMapDepth3", Float) = 1
		_Tilling3("Tilling3", Float) = 1
		_AlbedoColor3("AlbedoColor3", Color) = (1,1,1,1)
		_Smoothness3("Smoothness3", Float) = 1
		_Metallic3("Metallic3", Float) = 1
		_MetalicBrightnes3("MetalicBrightnes3", Range( 0 , 1)) = 0
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMultiplayer("EmissionMultiplayer", Float) = 0
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
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float3 viewDir;
		};

		uniform float _BlendContrast;
		uniform float _Height1;
		uniform sampler2D _HeightMap1;
		uniform float _Tilling1;
		uniform float _ParalaxOffset1;
		uniform float _PrlxRefPlane1;
		uniform float4 _HeightMap1_ST;
		uniform float _Height2;
		uniform sampler2D _HeightMap2;
		uniform float _Tilling2;
		uniform float _ParalaxOffset2;
		uniform float _PrlxRefPlane2;
		uniform float4 _HeightMap2_ST;
		uniform float _Height3;
		uniform sampler2D _HeightMap3;
		uniform float _Tilling3;
		uniform float _ParalaxOffset3;
		uniform float _PrlxRefPlane3;
		uniform float4 _HeightMap3_ST;
		uniform float _NormalMapDepthBase;
		uniform sampler2D _NormalMapBase;
		uniform float _TillingBase;
		uniform sampler2D _HeightMapBase;
		uniform float _ParalaxOffsetBase;
		uniform float _PrlxRefPlaneBase;
		uniform float4 _HeightMapBase_ST;
		uniform float _NormalMapDepth1;
		uniform sampler2D _NormalMap1;
		uniform float _NormalMapDepth2;
		uniform sampler2D _NormalMap2;
		uniform float _NormalMapDepth3;
		uniform sampler2D _NormalMap3;
		uniform UNITY_DECLARE_TEX2DARRAY( _AlbedoMapArray );
		uniform float4 _AlbedoColorBase;
		uniform float4 _AlbedoColor1;
		uniform float4 _AlbedoColor2;
		uniform float4 _AlbedoColor3;
		uniform float4 _EmissionColor;
		uniform float _EmissionMultiplayer;
		uniform float _MetallicBase;
		uniform UNITY_DECLARE_TEX2DARRAY( _MetallicMapArray );
		uniform float _MetalicBrightnesBase;
		uniform float _Metallic1;
		uniform float _MetalicBrightnes1;
		uniform float _Metallic2;
		uniform float _MetalicBrightnes2;
		uniform float _Metallic3;
		uniform float _MetalicBrightnes3;
		uniform float _SmoothnessBase;
		uniform float _Smoothness1;
		uniform float _Smoothness2;
		uniform float _Smoothness3;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, (float)dot( normalWorld, viewWorld ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
				currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
				if ( currHeight > currRayZ )
				{
					stepIndex = numSteps + 1;
				}
				else
				{
					stepIndex++;
					prevTexOffset = currTexOffset;
					prevRayZ = currRayZ;
					prevHeight = currHeight;
					currTexOffset += deltaTex;
					currRayZ -= layerHeight;
				}
			}
			int sectionSteps = 2;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
				intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
				finalTexOffset = prevTexOffset + intersection * deltaTex;
				newZ = prevRayZ - intersection * layerHeight;
				newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
				if ( newHeight > newZ )
				{
					currTexOffset = finalTexOffset;
					currHeight = newHeight;
					currRayZ = newZ;
					deltaTex = intersection * deltaTex;
					layerHeight = intersection * layerHeight;
				}
				else
				{
					prevTexOffset = finalTexOffset;
					prevHeight = newHeight;
					prevRayZ = newZ;
					deltaTex = ( 1 - intersection ) * deltaTex;
					layerHeight = ( 1 - intersection ) * layerHeight;
				}
				sectionIndex++;
			}
			return uvs + finalTexOffset;
		}


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 clampResult178 = clamp( i.vertexColor , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 break170 = clampResult178;
			float3 appendResult169 = (float3(break170.r , break170.g , break170.b));
			float4 break166 = ( 2.0 * ( ( 1.0 - i.vertexColor ) * i.vertexColor ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV288 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode288 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV288, 10.17 ) );
			float clampResult296 = clamp( fresnelNode288 , 0.0 , 1.0 );
			float temp_output_291_0 = ( 1.0 - clampResult296 );
			float2 OffsetPOM98 = POM( _HeightMap1, ( i.uv_texcoord * _Tilling1 ), ddx(( i.uv_texcoord * _Tilling1 )), ddy(( i.uv_texcoord * _Tilling1 )), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 16, ( ( 0.001 * _ParalaxOffset1 ) * temp_output_291_0 ), _PrlxRefPlane1, _HeightMap1_ST.xy, float2(0,0), 0 );
			float lerpResult134 = lerp( 0.0 , break166.r , CalculateContrast(_BlendContrast,( _Height1 + tex2D( _HeightMap1, OffsetPOM98 ) )).r);
			float2 OffsetPOM115 = POM( _HeightMap2, ( i.uv_texcoord * _Tilling2 ), ddx(( i.uv_texcoord * _Tilling2 )), ddy(( i.uv_texcoord * _Tilling2 )), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 16, ( ( 0.001 * _ParalaxOffset2 ) * temp_output_291_0 ), _PrlxRefPlane2, _HeightMap2_ST.xy, float2(0,0), 0 );
			float lerpResult135 = lerp( 0.0 , break166.g , CalculateContrast(_BlendContrast,( _Height2 + tex2D( _HeightMap2, OffsetPOM115 ) )).r);
			float2 OffsetPOM257 = POM( _HeightMap3, ( i.uv_texcoord * _Tilling3 ), ddx(( i.uv_texcoord * _Tilling3 )), ddy(( i.uv_texcoord * _Tilling3 )), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 16, ( ( 0.001 * _ParalaxOffset3 ) * temp_output_291_0 ), _PrlxRefPlane3, _HeightMap3_ST.xy, float2(0,0), 0 );
			float lerpResult151 = lerp( 0.0 , break166.b , CalculateContrast(_BlendContrast,( _Height3 + tex2D( _HeightMap3, OffsetPOM257 ) )).r);
			float3 appendResult129 = (float3(lerpResult134 , lerpResult135 , lerpResult151));
			float3 clampResult182 = clamp( appendResult129 , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 temp_output_167_0 = ( appendResult169 + clampResult182 );
			float2 OffsetPOM261 = POM( _HeightMapBase, ( i.uv_texcoord * _TillingBase ), ddx(( i.uv_texcoord * _TillingBase )), ddy(( i.uv_texcoord * _TillingBase )), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 16, ( ( 0.001 * _ParalaxOffsetBase ) * temp_output_291_0 ), _PrlxRefPlaneBase, _HeightMapBase_ST.xy, float2(0,0), 0 );
			float2 temp_output_262_0 = ( OffsetPOM261 * 1.0 );
			float2 temp_output_239_0 = ( OffsetPOM98 * 1.0 );
			float2 temp_output_243_0 = ( OffsetPOM115 * 1.0 );
			float2 temp_output_246_0 = ( OffsetPOM257 * 1.0 );
			float3 layeredBlendVar173 = temp_output_167_0;
			float3 layeredBlend173 = ( lerp( lerp( lerp( UnpackScaleNormal( tex2D( _NormalMapBase, temp_output_262_0 ) ,_NormalMapDepthBase ) , UnpackScaleNormal( tex2D( _NormalMap1, temp_output_239_0 ) ,_NormalMapDepth1 ) , layeredBlendVar173.x ) , UnpackScaleNormal( tex2D( _NormalMap2, temp_output_243_0 ) ,_NormalMapDepth2 ) , layeredBlendVar173.y ) , UnpackScaleNormal( tex2D( _NormalMap3, temp_output_246_0 ) ,_NormalMapDepth3 ) , layeredBlendVar173.z ) );
			o.Normal = layeredBlend173;
			float3 clampResult190 = clamp( temp_output_167_0 , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float4 texArray230 = UNITY_SAMPLE_TEX2DARRAY(_AlbedoMapArray, float3(temp_output_262_0, 0.0)  );
			float4 texArray232 = UNITY_SAMPLE_TEX2DARRAY(_AlbedoMapArray, float3(temp_output_239_0, 1.0)  );
			float4 texArray234 = UNITY_SAMPLE_TEX2DARRAY(_AlbedoMapArray, float3(temp_output_243_0, 2.0)  );
			float4 texArray236 = UNITY_SAMPLE_TEX2DARRAY(_AlbedoMapArray, float3(temp_output_246_0, 3.0)  );
			float3 layeredBlendVar132 = clampResult190;
			float4 layeredBlend132 = ( lerp( lerp( lerp( ( texArray230 * _AlbedoColorBase ) , ( texArray232 * _AlbedoColor1 ) , layeredBlendVar132.x ) , ( texArray234 * _AlbedoColor2 ) , layeredBlendVar132.y ) , ( texArray236 * _AlbedoColor3 ) , layeredBlendVar132.z ) );
			o.Albedo = layeredBlend132.rgb;
			o.Emission = ( _EmissionColor * ( _EmissionMultiplayer * layeredBlend132 ) ).rgb;
			float4 texArray253 = UNITY_SAMPLE_TEX2DARRAY(_MetallicMapArray, float3(temp_output_262_0, 0.0)  );
			float lerpResult223 = lerp( texArray253.r , 1.0 , _MetalicBrightnesBase);
			float4 texArray254 = UNITY_SAMPLE_TEX2DARRAY(_MetallicMapArray, float3(temp_output_239_0, 0.0)  );
			float lerpResult93 = lerp( texArray254.r , 1.0 , _MetalicBrightnes1);
			float4 texArray255 = UNITY_SAMPLE_TEX2DARRAY(_MetallicMapArray, float3(temp_output_243_0, 0.0)  );
			float lerpResult206 = lerp( texArray255.r , 1.0 , _MetalicBrightnes2);
			float4 texArray256 = UNITY_SAMPLE_TEX2DARRAY(_MetallicMapArray, float3(temp_output_246_0, 0.0)  );
			float lerpResult214 = lerp( texArray256.r , 1.0 , _MetalicBrightnes3);
			float3 layeredBlendVar220 = temp_output_167_0;
			float layeredBlend220 = ( lerp( lerp( lerp( ( _MetallicBase * lerpResult223 ) , ( _Metallic1 * lerpResult93 ) , layeredBlendVar220.x ) , ( _Metallic2 * lerpResult206 ) , layeredBlendVar220.y ) , ( _Metallic3 * lerpResult214 ) , layeredBlendVar220.z ) );
			o.Metallic = layeredBlend220;
			float3 layeredBlendVar229 = temp_output_167_0;
			float layeredBlend229 = ( lerp( lerp( lerp( ( texArray253.a * _SmoothnessBase ) , ( texArray254.a * _Smoothness1 ) , layeredBlendVar229.x ) , ( texArray255.a * _Smoothness2 ) , layeredBlendVar229.y ) , ( texArray256.a * _Smoothness3 ) , layeredBlendVar229.z ) );
			float clampResult251 = clamp( ( layeredBlend229 + 0.0 ) , 0.0 , 1.0 );
			o.Smoothness = clampResult251;
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
				half4 color : COLOR0;
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
				o.color = v.color;
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
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
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
169;911;1643;966;4828.314;826.6902;2.676252;True;True
Node;AmplifyShaderEditor.RangedFloatNode;290;-3883.459,290.1167;Float;False;Constant;_Float4;Float 4;48;0;Create;True;0;0;False;0;10.17;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;288;-3547.828,221.1874;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;282;-3670.607,90.60851;Float;False;Property;_ParalaxOffset3;ParalaxOffset3;36;0;Create;True;0;0;False;0;0.001;5.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;296;-3303.202,250.5947;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;279;-3656.308,-97.89147;Float;False;Property;_ParalaxOffset2;ParalaxOffset2;25;0;Create;True;0;0;False;0;0.001;58.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-3714.573,-586.684;Float;False;Constant;_ParalaxDepthCorrection;ParalaxDepthCorrection;20;0;Create;True;0;0;False;0;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-3642.007,-301.9914;Float;False;Property;_ParalaxOffset1;ParalaxOffset1;14;0;Create;True;0;0;False;0;0.001;38.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;277;-3398.801,-393.7146;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;280;-3413.102,-189.6146;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;-3427.401,-1.114647;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;291;-3271.738,151.3241;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-2587.025,-227.9156;Float;False;Property;_Tilling2;Tilling2;30;0;Create;True;0;0;False;0;1;11.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;-2609.539,8.671144;Float;False;Property;_Tilling3;Tilling3;41;0;Create;True;0;0;False;0;1;26.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2904.818,-463.0872;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;241;-2586.618,-465.257;Float;False;Property;_Tilling1;Tilling1;19;0;Create;True;0;0;False;0;1;14.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;287;-2658.822,-110.0751;Float;False;Property;_PrlxRefPlane3;PrlxRefPlane3;37;0;Create;True;0;0;False;0;0.001;0.156;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;286;-2648.422,-333.6752;Float;False;Property;_PrlxRefPlane2;PrlxRefPlane2;26;0;Create;True;0;0;False;0;0.001;0.58;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;293;-3216.742,-213.7761;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;285;-2653.619,-574.1754;Float;False;Property;_PrlxRefPlane1;PrlxRefPlane1;15;0;Create;True;0;0;False;0;0.001;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;-3202.441,-417.8761;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;19;-2872.867,-294.6031;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-2290.67,-249.8378;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;267;-2289.794,-458.0731;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;-2271.866,-10.85054;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;292;-3231.041,-25.27615;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;116;-3820.921,-1107.884;Float;True;Property;_HeightMap2;HeightMap2;24;0;Create;True;0;0;False;0;None;cca98e34997a3dc419fb4697f59531d0;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;143;-3771.311,-904.1208;Float;True;Property;_HeightMap3;HeightMap3;35;0;Create;True;0;0;False;0;None;40ce7e29b25f9614cb917d5731b3b653;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;101;-3821.083,-1306.292;Float;True;Property;_HeightMap1;HeightMap1;13;0;Create;True;0;0;False;0;None;81817898314db2541b665c98ee71a28f;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;98;-1910.503,-586.5798;Float;False;0;8;16;2;0.02;0.5;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;115;-1911.256,-364.6845;Float;False;0;8;16;2;0.02;0.5;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;257;-1907.035,-160.4962;Float;False;0;8;16;2;0.02;0.5;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;119;-3686.579,-2139.288;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;148;-3400.677,-903.2667;Float;True;Property;_TextureSample5;Texture Sample 5;24;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;188;-3238.771,-1820.16;Float;False;Property;_Height2;Height2;27;0;Create;True;0;0;False;0;2;0.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-3248.421,-1900.571;Float;False;Property;_Height1;Height1;16;0;Create;True;0;0;False;0;2;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;189;-3252.416,-1727.857;Float;False;Property;_Height3;Height3;38;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;125;-3424.734,-1322.12;Float;True;Property;_TextureSample0;Texture Sample 0;24;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;126;-3403.787,-1101.359;Float;True;Property;_TextureSample2;Texture Sample 2;24;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;162;-3539.181,-1975.778;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;193;-2809.38,-1240.837;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;191;-2811.471,-1410.145;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;137;-2896.634,-1857.936;Float;False;Property;_BlendContrast;BlendContrast;0;0;Create;True;0;0;False;0;1;16.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;192;-2826.104,-1623.347;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-3243.217,-2160.498;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-3346.034,-2329.627;Float;False;Constant;_Float1;Float 1;28;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;159;-2496.419,-1600.617;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-3011.556,-2219.248;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3674.667,-506.7083;Float;False;Property;_ParalaxOffsetBase;ParalaxOffsetBase;4;0;Create;True;0;0;False;0;0.001;5.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;157;-2510.499,-1443.035;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;154;-2492.806,-1243.64;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;156;-2241.454,-1551.634;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;158;-2243.235,-1245.422;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;166;-2822.625,-2092.846;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-3395.061,-598.4315;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;155;-2243.345,-1389.625;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;240;-2606.882,-681.7051;Float;False;Property;_TillingBase;TillingBase;8;0;Create;True;0;0;False;0;1;7.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-1953.633,-1605.26;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;266;-2409.11,-742.2209;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;134;-1957.491,-1770.829;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;295;-3198.701,-622.593;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;-1934.993,-1458.566;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;284;-2687.422,-802.975;Float;False;Property;_PrlxRefPlaneBase;PrlxRefPlaneBase;5;0;Create;True;0;0;False;0;0.001;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;178;-2453.984,-2526.425;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;259;-3801.562,-1540.83;Float;True;Property;_HeightMapBase;HeightMapBase;3;0;Create;True;0;0;False;0;None;9d90cfebda4ff7b46bdec1e283c0c084;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BreakToComponentsNode;170;-2274.408,-2397.194;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;129;-1687.052,-1625.119;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;271;-1754.847,-1135.221;Float;False;Constant;_Float3;Float 3;38;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;261;-1917.706,-806.939;Float;False;0;8;16;2;0.02;0.5;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;237;-907.7741,-1502.912;Float;False;Constant;_AlbedoIndex3;AlbedoIndex3;37;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-924.3703,-2348.929;Float;False;Constant;_AlbedoIndex1;AlbedoIndex1;37;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;243;-1488.848,-361.5107;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-967.8638,-2759.419;Float;False;Constant;_AlbedoIndex;AlbedoIndex;37;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;182;-1526.307,-1881.479;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;-1470.045,-122.5234;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;235;-909.506,-1873.41;Float;False;Constant;_AlbedoIndex2;AlbedoIndex2;37;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;169;-1987.758,-2365.499;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;262;-1495.174,-790.105;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;239;-1487.972,-569.7458;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;106;-650.8668,-2173.823;Float;False;Property;_AlbedoColor1;AlbedoColor1;20;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;145;-678.6395,-1315.097;Float;False;Property;_AlbedoColor3;AlbedoColor3;42;0;Create;True;0;0;False;0;1,1,1,1;0.6981132,0.6981132,0.6981132,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-669.6706,-2605.136;Float;False;Property;_AlbedoColorBase;AlbedoColorBase;9;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;253;-1037.801,148.1044;Float;True;Property;_MetallicMapArray;MetallicMapArray;2;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;232;-746.82,-2395.449;Float;True;Property;_TextureArray0;Texture Array 0;37;0;Create;True;0;0;False;0;None;0;Instance;230;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;167;-1368.372,-2197.729;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureArrayNode;236;-719.7493,-1549.432;Float;True;Property;_TextureArray2;Texture Array 2;39;0;Create;True;0;0;False;0;None;0;Instance;230;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;256;-1015.624,1277.151;Float;True;Property;_TextureArray5;Texture Array 5;36;0;Create;True;0;0;False;0;None;0;Instance;253;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;230;-740.726,-2825.902;Float;True;Property;_AlbedoMapArray;AlbedoMapArray;1;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;254;-1042.824,536.3504;Float;True;Property;_TextureArray3;Texture Array 3;36;0;Create;True;0;0;False;0;None;0;Instance;253;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-625.6056,684.0491;Float;False;Property;_Smoothness1;Smoothness1;21;0;Create;True;0;0;False;0;1;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;215;-600.5758,1434.315;Float;False;Property;_Smoothness3;Smoothness3;43;0;Create;True;0;0;False;0;1;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-636.3124,304.4656;Float;False;Property;_SmoothnessBase;SmoothnessBase;10;0;Create;True;0;0;False;0;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;255;-1028.425,904.3506;Float;True;Property;_TextureArray4;Texture Array 4;36;0;Create;True;0;0;False;0;None;0;Instance;253;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;207;-614.1426,1056.536;Float;False;Property;_Smoothness2;Smoothness2;32;0;Create;True;0;0;False;0;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;234;-721.6922,-1952.12;Float;True;Property;_TextureArray1;Texture Array 1;38;0;Create;True;0;0;False;0;None;0;Instance;230;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;141;-645.0353,-1742.706;Float;False;Property;_AlbedoColor2;AlbedoColor2;31;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;225;-894.4614,-17.24541;Float;False;Constant;_Float2;Float 2;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-373.828,-1417.128;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;190;-379.5413,-2045.497;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-372.7155,-1838.764;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-379.1516,-2585.914;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-998.0695,820.6123;Float;False;Property;_MetalicBrightnes2;MetalicBrightnes2;34;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-378.5469,-2269.882;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1015.933,452.9251;Float;False;Property;_MetalicBrightnes1;MetalicBrightnes1;23;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-391.1477,995.8464;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-895.3912,740.3919;Float;False;Constant;_Float5;Float 5;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-888.2244,1112.971;Float;False;Constant;_Float9;Float 9;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-891.7546,365.5381;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-409.0113,628.1591;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-1018.639,70.14155;Float;False;Property;_MetalicBrightnesBase;MetalicBrightnesBase;12;0;Create;True;0;0;False;0;0;0.64;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;-383.9808,1368.425;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-990.9027,1193.192;Float;False;Property;_MetalicBrightnes3;MetalicBrightnes3;45;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;-411.7178,245.3755;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;274;-1242.925,-529.4929;Float;False;Property;_NormalMapDepth3;NormalMapDepth3;40;0;Create;True;0;0;False;0;1;1.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1259.126,-871.1492;Float;False;Property;_NormalMapDepthBase;NormalMapDepthBase;7;0;Create;True;0;0;False;0;1;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;229;-83.37372,172.9181;Float;False;6;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;214;-593.7981,1239.483;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;132;-66.76407,-1565.721;Float;False;6;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-8.835178,-1128.632;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;47;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;226;-630.488,29.36113;Float;False;Property;_MetallicBase;MetallicBase;11;0;Create;True;0;0;False;0;1;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-627.7811,412.1447;Float;False;Property;_Metallic1;Metallic1;22;0;Create;True;0;0;False;0;1;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-1264.925,-767.4929;Float;False;Property;_NormalMapDepth1;NormalMapDepth1;18;0;Create;True;0;0;False;0;1;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;93;-618.8279,499.2162;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;219.8878,198.5585;Float;False;Constant;_SmothnessAdd;SmothnessAdd;41;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;273;-1242.925,-635.4929;Float;False;Property;_NormalMapDepth2;NormalMapDepth2;29;0;Create;True;0;0;False;0;1;0.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;223;-619.2131,124.5581;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;206;-600.965,866.9034;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-602.7513,1152.411;Float;False;Property;_Metallic3;Metallic3;44;0;Create;True;0;0;False;0;1;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;208;-609.9181,779.8319;Float;False;Property;_Metallic2;Metallic2;33;0;Create;True;0;0;False;0;1;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;111;-984.6247,-700.4472;Float;True;Property;_NormalMap1;NormalMap1;17;0;Create;True;0;0;False;0;None;69d9cdf7f0f274f4393e666185930e5a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-410.0048,508.3086;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-983.2156,-911.5417;Float;True;Property;_NormalMapBase;NormalMapBase;6;0;Create;True;0;0;False;0;None;f9891a47dc86e124fbc677f3134b81f7;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;303.1523,-1129.245;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;250;487.9043,19.18243;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;354.2496,-1332.35;Float;False;Property;_EmissionColor;EmissionColor;46;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-390.0013,867.4355;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;171;-981.0021,-492.7853;Float;True;Property;_NormalMap2;NormalMap2;28;0;Create;True;0;0;False;0;None;705b8d9f7ba3edd429da72454b0a0a30;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;221;-422.3225,124.3643;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-384.9745,1248.575;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;172;-981.0021,-272.2191;Float;True;Property;_NormalMap3;NormalMap3;39;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;177;-2743.4,-2513.37;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;260;-3435.785,-1539.762;Float;True;Property;_TextureSample1;Texture Sample 1;24;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;251;720.2132,-108.4211;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;220;-131.8263,-188.9172;Float;False;6;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;415.4357,-928.699;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LayeredBlendNode;173;-242.616,-777.1154;Float;False;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1031.295,-535.8076;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartBlend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;288;3;290;0
WireConnection;296;0;288;0
WireConnection;277;0;64;0
WireConnection;277;1;276;0
WireConnection;280;0;64;0
WireConnection;280;1;279;0
WireConnection;283;0;64;0
WireConnection;283;1;282;0
WireConnection;291;0;296;0
WireConnection;293;0;280;0
WireConnection;293;1;291;0
WireConnection;294;0;277;0
WireConnection;294;1;291;0
WireConnection;265;0;17;0
WireConnection;265;1;248;0
WireConnection;267;0;17;0
WireConnection;267;1;241;0
WireConnection;264;0;17;0
WireConnection;264;1;249;0
WireConnection;292;0;283;0
WireConnection;292;1;291;0
WireConnection;98;0;267;0
WireConnection;98;1;101;0
WireConnection;98;2;294;0
WireConnection;98;3;19;0
WireConnection;98;4;285;0
WireConnection;115;0;265;0
WireConnection;115;1;116;0
WireConnection;115;2;293;0
WireConnection;115;3;19;0
WireConnection;115;4;286;0
WireConnection;257;0;264;0
WireConnection;257;1;143;0
WireConnection;257;2;292;0
WireConnection;257;3;19;0
WireConnection;257;4;287;0
WireConnection;148;0;143;0
WireConnection;148;1;257;0
WireConnection;125;0;101;0
WireConnection;125;1;98;0
WireConnection;126;0;116;0
WireConnection;126;1;115;0
WireConnection;162;0;119;0
WireConnection;193;0;189;0
WireConnection;193;1;148;0
WireConnection;191;0;188;0
WireConnection;191;1;126;0
WireConnection;192;0;184;0
WireConnection;192;1;125;0
WireConnection;161;0;162;0
WireConnection;161;1;119;0
WireConnection;159;1;192;0
WireConnection;159;0;137;0
WireConnection;163;0;164;0
WireConnection;163;1;161;0
WireConnection;157;1;191;0
WireConnection;157;0;137;0
WireConnection;154;1;193;0
WireConnection;154;0;137;0
WireConnection;156;0;159;0
WireConnection;158;0;154;0
WireConnection;166;0;163;0
WireConnection;63;0;64;0
WireConnection;63;1;18;0
WireConnection;155;0;157;0
WireConnection;135;1;166;1
WireConnection;135;2;155;0
WireConnection;266;0;17;0
WireConnection;266;1;240;0
WireConnection;134;1;166;0
WireConnection;134;2;156;0
WireConnection;295;0;63;0
WireConnection;295;1;291;0
WireConnection;151;1;166;2
WireConnection;151;2;158;0
WireConnection;178;0;119;0
WireConnection;170;0;178;0
WireConnection;129;0;134;0
WireConnection;129;1;135;0
WireConnection;129;2;151;0
WireConnection;261;0;266;0
WireConnection;261;1;259;0
WireConnection;261;2;295;0
WireConnection;261;3;19;0
WireConnection;261;4;284;0
WireConnection;243;0;115;0
WireConnection;243;1;271;0
WireConnection;182;0;129;0
WireConnection;246;0;257;0
WireConnection;246;1;271;0
WireConnection;169;0;170;0
WireConnection;169;1;170;1
WireConnection;169;2;170;2
WireConnection;262;0;261;0
WireConnection;262;1;271;0
WireConnection;239;0;98;0
WireConnection;239;1;271;0
WireConnection;253;0;262;0
WireConnection;232;0;239;0
WireConnection;232;1;233;0
WireConnection;167;0;169;0
WireConnection;167;1;182;0
WireConnection;236;0;246;0
WireConnection;236;1;237;0
WireConnection;256;0;246;0
WireConnection;230;0;262;0
WireConnection;230;1;231;0
WireConnection;254;0;239;0
WireConnection;255;0;243;0
WireConnection;234;0;243;0
WireConnection;234;1;235;0
WireConnection;144;0;236;0
WireConnection;144;1;145;0
WireConnection;190;0;167;0
WireConnection;142;0;234;0
WireConnection;142;1;141;0
WireConnection;109;0;230;0
WireConnection;109;1;4;0
WireConnection;108;0;232;0
WireConnection;108;1;106;0
WireConnection;205;0;255;4
WireConnection;205;1;207;0
WireConnection;10;0;254;4
WireConnection;10;1;9;0
WireConnection;213;0;256;4
WireConnection;213;1;215;0
WireConnection;222;0;253;4
WireConnection;222;1;227;0
WireConnection;229;0;167;0
WireConnection;229;1;222;0
WireConnection;229;2;10;0
WireConnection;229;3;205;0
WireConnection;229;4;213;0
WireConnection;214;0;256;1
WireConnection;214;1;219;0
WireConnection;214;2;218;0
WireConnection;132;0;190;0
WireConnection;132;1;109;0
WireConnection;132;2;108;0
WireConnection;132;3;142;0
WireConnection;132;4;144;0
WireConnection;93;0;254;1
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;223;0;253;1
WireConnection;223;1;225;0
WireConnection;223;2;228;0
WireConnection;206;0;255;1
WireConnection;206;1;211;0
WireConnection;206;2;210;0
WireConnection;111;1;239;0
WireConnection;111;5;272;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;13;1;262;0
WireConnection;13;5;66;0
WireConnection;39;0;38;0
WireConnection;39;1;132;0
WireConnection;250;0;229;0
WireConnection;250;1;252;0
WireConnection;204;0;208;0
WireConnection;204;1;206;0
WireConnection;171;1;243;0
WireConnection;171;5;273;0
WireConnection;221;0;226;0
WireConnection;221;1;223;0
WireConnection;212;0;216;0
WireConnection;212;1;214;0
WireConnection;172;1;246;0
WireConnection;172;5;274;0
WireConnection;177;1;119;0
WireConnection;177;0;137;0
WireConnection;260;0;259;0
WireConnection;260;1;261;0
WireConnection;251;0;250;0
WireConnection;220;0;167;0
WireConnection;220;1;221;0
WireConnection;220;2;7;0
WireConnection;220;3;204;0
WireConnection;220;4;212;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;173;0;167;0
WireConnection;173;1;13;0
WireConnection;173;2;111;0
WireConnection;173;3;171;0
WireConnection;173;4;172;0
WireConnection;0;0;132;0
WireConnection;0;1;173;0
WireConnection;0;2;37;0
WireConnection;0;3;220;0
WireConnection;0;4;251;0
ASEEND*/
//CHKSM=B93B1ADA3918F039D346C00D6F63A85DC965D1E9