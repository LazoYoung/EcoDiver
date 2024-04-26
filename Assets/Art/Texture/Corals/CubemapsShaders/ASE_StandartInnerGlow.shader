// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartInnerGlow"
{
	Properties
	{
		_InnerGlowMask("InnerGlowMask", 2D) = "white" {}
		_SructureGlowTexture("SructureGlowTexture", 2D) = "white" {}
		_StructureColor("StructureColor", Color) = (0,0,0,0)
		_InnerStructMapTilling("InnerStructMapTilling", Float) = 0
		_StructureGlowIntens("StructureGlowIntens", Float) = 0
		_FresnelPower("FresnelPower", Float) = 0
		_FresnelScale("FresnelScale", Float) = 1
		_InnerStructureDepth("InnerStructureDepth", Float) = 0
		_HeightMultiplayer("HeightMultiplayer", Float) = 0
		_InnerHeightMap("InnerHeightMap", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		_EmissionMap("EmissionMap", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMultiplayer("EmissionMultiplayer", Float) = 0
		_RimBias("RimBias", Float) = 0
		_RimPower("RimPower", Float) = 1
		_RimIntens("RimIntens", Float) = 10
		_RimColor("RimColor", Color) = (1,1,1,0)
		_NormalMap("NormalMap", 2D) = "bump" {}
		[Toggle]_SmoothFromMapSwitch("SmoothFromMapSwitch", Float) = 1
		[Toggle]_EmissionSwitch("EmissionSwitch", Float) = 0
		[Toggle]_SmoothRough("Smooth/Rough", Float) = 0
		_SmoothnessMap("SmoothnessMap", 2D) = "white" {}
		_MetallicMap("MetallicMap", 2D) = "white" {}
		_Snoothness("Snoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_NormalMapDepth("NormalMapDepth", Float) = 1
		_MetalicBrightnes("MetalicBrightnes", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
		};

		uniform float _NormalMapDepth;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _InnerGlowMask;
		uniform float4 _InnerGlowMask_ST;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _StructureGlowIntens;
		uniform float4 _StructureColor;
		uniform sampler2D _SructureGlowTexture;
		uniform float _InnerStructMapTilling;
		uniform sampler2D _InnerHeightMap;
		uniform float4 _InnerHeightMap_ST;
		uniform float _HeightMultiplayer;
		uniform float _InnerStructureDepth;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AlbedoColor;
		uniform float4 _RimColor;
		uniform float _RimBias;
		uniform float _RimIntens;
		uniform float _RimPower;
		uniform float4 _EmissionColor;
		uniform float _EmissionMultiplayer;
		uniform float _EmissionSwitch;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float _Metallic;
		uniform sampler2D _MetallicMap;
		uniform float4 _MetallicMap_ST;
		uniform float _MetalicBrightnes;
		uniform float _SmoothRough;
		uniform float _SmoothFromMapSwitch;
		uniform sampler2D _SmoothnessMap;
		uniform float4 _SmoothnessMap_ST;
		uniform float _Snoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode13 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ) ,_NormalMapDepth );
			o.Normal = tex2DNode13;
			float2 uv_InnerGlowMask = i.uv_texcoord * _InnerGlowMask_ST.xy + _InnerGlowMask_ST.zw;
			float4 tex2DNode129 = tex2D( _InnerGlowMask, uv_InnerGlowMask );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV119 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode119 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV119, _FresnelPower ) );
			float clampResult128 = clamp( fresnelNode119 , 0.0 , 1.0 );
			float2 temp_cast_0 = (_InnerStructMapTilling).xx;
			float2 uv_TexCoord151 = i.uv_texcoord * temp_cast_0;
			float2 uv_InnerHeightMap = i.uv_texcoord * _InnerHeightMap_ST.xy + _InnerHeightMap_ST.zw;
			float temp_output_137_0 = ( tex2D( _InnerHeightMap, uv_InnerHeightMap ).r * _HeightMultiplayer );
			float2 Offset112 = ( ( temp_output_137_0 - 1 ) * ase_worldViewDir.xy * _InnerStructureDepth ) + uv_TexCoord151;
			float2 temp_cast_1 = (_InnerStructMapTilling).xx;
			float2 temp_cast_2 = (0.2).xx;
			float2 uv_TexCoord113 = i.uv_texcoord * temp_cast_1 + temp_cast_2;
			float2 Offset140 = ( ( temp_output_137_0 - 1 ) * ase_worldViewDir.xy * ( _InnerStructureDepth * 2.0 ) ) + uv_TexCoord113;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _Albedo, uv_Albedo ) * _AlbedoColor );
			float4 temp_output_130_0 = ( tex2DNode129.r * ( ( ( 1.0 - clampResult128 ) * ( _StructureGlowIntens * ( _StructureColor * ( tex2D( _SructureGlowTexture, Offset112 ) + ( tex2D( _SructureGlowTexture, Offset140 ) * 0.6 ) ) ) ) ) * temp_output_3_0 ) );
			float4 temp_output_109_0 = ( temp_output_130_0 + temp_output_3_0 );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV153 = dot( ase_normWorldNormal, ase_worldViewDir );
			float fresnelNode153 = ( _RimBias + _RimIntens * pow( 1.0 - fresnelNdotV153, _RimPower ) );
			float clampResult161 = clamp( fresnelNode153 , 0.0 , 1.0 );
			float lerpResult165 = lerp( 0.0 , ( 1.0 - clampResult161 ) , tex2DNode129.r);
			float4 lerpResult167 = lerp( temp_output_109_0 , ( _RimColor * temp_output_109_0 ) , lerpResult165);
			o.Albedo = lerpResult167.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 tex2DNode33 = tex2D( _EmissionMap, uv_EmissionMap );
			o.Emission = ( ( _EmissionColor * temp_output_130_0 ) + ( _EmissionMultiplayer * lerp(tex2DNode33,( temp_output_3_0 * tex2DNode33.a ),_EmissionSwitch) ) ).rgb;
			float2 uv_MetallicMap = i.uv_texcoord * _MetallicMap_ST.xy + _MetallicMap_ST.zw;
			float4 tex2DNode5 = tex2D( _MetallicMap, uv_MetallicMap );
			float3 linearToGamma105 = LinearToGammaSpace( tex2DNode5.rgb );
			float lerpResult93 = lerp( linearToGamma105.x , 1.0 , _MetalicBrightnes);
			o.Metallic = ( _Metallic * lerpResult93 );
			float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
			float3 linearToGamma103 = LinearToGammaSpace( tex2D( _SmoothnessMap, uv_SmoothnessMap ).rgb );
			o.Smoothness = ( lerp(lerp(tex2DNode5.a,linearToGamma103.z,_SmoothFromMapSwitch),( 1.0 - lerp(tex2DNode5.a,linearToGamma103.z,_SmoothFromMapSwitch) ),_SmoothRough) * _Snoothness );
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
			#pragma target 3.0
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
91;240;1643;928;-501.9483;1436.856;1.257903;True;True
Node;AmplifyShaderEditor.RangedFloatNode;142;-1504.767,-1299.752;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;0.2;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1582.161,-982.4522;Float;False;Property;_HeightMultiplayer;HeightMultiplayer;8;0;Create;True;0;0;False;0;0;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1283.286,-767.2541;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;2;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;134;-1699.972,-1218.07;Float;True;Property;_InnerHeightMap;InnerHeightMap;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;132;-1563.871,-1455.356;Float;False;Property;_InnerStructMapTilling;InnerStructMapTilling;3;0;Create;True;0;0;False;0;0;1.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-1356.305,-900.303;Float;False;Property;_InnerStructureDepth;InnerStructureDepth;7;0;Create;True;0;0;False;0;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-947.5649,-1064.132;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;110;-753.5517,-814.7928;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-997.4015,-892.9169;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;113;-1216.686,-1187.616;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-1226.738,-1343.735;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;140;-383.6507,-965.1729;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;150;1.620084,-776.6782;Float;False;Constant;_Float3;Float 3;7;0;Create;True;0;0;False;0;0.6;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;112;-379.5427,-1187.55;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1536.795,-552.9112;Float;False;Property;_NormalMapDepth;NormalMapDepth;27;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;139;-121.3286,-988.7354;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;e451d922929dcc44da221b6ac55970b5;True;0;False;white;Auto;False;Instance;107;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;-880.1086,-1601.02;Float;False;Property;_FresnelScale;FresnelScale;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;107;-106.884,-1189.663;Float;True;Property;_SructureGlowTexture;SructureGlowTexture;1;0;Create;True;0;0;False;0;None;927e7f90538f2934eb52480ccfc9146f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;121;-850.6812,-1340.82;Float;False;Property;_FresnelPower;FresnelPower;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;205.8227,-957.3193;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-1240.977,-475.8791;Float;True;Property;_NormalMap;NormalMap;19;0;Create;True;0;0;False;0;None;dc3059aa8aac90349bd83f5a3ac1784c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;119;-464.5405,-1715.647;Float;False;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;114;-259.6114,-1379.112;Float;False;Property;_StructureColor;StructureColor;2;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;145;365.2234,-1072.941;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1.466029,-1444.989;Float;False;Property;_StructureGlowIntens;StructureGlowIntens;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;346.5651,-1231.943;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;128;-201.2771,-1703.243;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;269.934,-1393.116;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-485.1377,-666.1189;Float;True;Property;_Albedo;Albedo;10;0;Create;True;0;0;False;0;None;5ce87cde2d12f1f4f86e3a606e6fec84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;123;42.98315,-1724.925;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-456.4226,-468.3278;Float;False;Property;_AlbedoColor;AlbedoColor;11;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1961.9,260.7534;Float;True;Property;_SmoothnessMap;SmoothnessMap;23;0;Create;True;0;0;False;0;None;42646be277339f24c8d91e4a8211db89;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;461.6142,-1630.448;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;90.86964,-651.82;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LinearToGammaNode;103;-1611.315,288.9816;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;160;1276.4,-907.2811;Float;False;Property;_RimBias;RimBias;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;1290.53,-1084.318;Float;False;Property;_RimIntens;RimIntens;17;0;Create;True;0;0;False;0;10;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;1277.483,-1000.66;Float;False;Property;_RimPower;RimPower;16;0;Create;True;0;0;False;0;1;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;129;558.0709,-1154.99;Float;True;Property;_InnerGlowMask;InnerGlowMask;0;0;Create;True;0;0;False;0;None;03b325e243c25b145aafccde3ffb461a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;649.7714,-1469.665;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-483.3045,-288.8951;Float;True;Property;_EmissionMap;EmissionMap;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1775.516,-38.90791;Float;True;Property;_MetallicMap;MetallicMap;24;0;Create;True;0;0;False;0;None;4b0fb47564b537b4c99807e38fc62622;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;153;1493.051,-1101.979;Float;False;Standard;TangentNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;104;-1383.809,277.0616;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;169.5569,-437.1342;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;909.2211,-1094.56;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-1130.743,191.3664;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;20;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;161;1722.956,-1064.519;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LinearToGammaNode;105;-1418.085,18.75522;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;12;-750.9462,311.9731;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;162;1890.257,-1062.002;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1389.108,-135.8561;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;1562.912,-895.9582;Float;False;Constant;_Float4;Float 4;29;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;823.4951,-777.6929;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1387.871,-249.1329;Float;False;Property;_MetalicBrightnes;MetalicBrightnes;28;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;365.4774,-644.0977;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;1012.445,-1561.341;Float;False;Property;_EmissionColor;EmissionColor;13;0;Create;True;0;0;False;0;0,0,0,0;0.7169812,0.7169812,0.7169812,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;32;438.5262,-378.484;Float;False;Property;_EmissionSwitch;EmissionSwitch;21;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;106;-1207.845,1.661224;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;157;924.3415,-524.8787;Float;False;Property;_RimColor;RimColor;18;0;Create;True;0;0;False;0;1,1,1,0;0.5283019,0.5283019,0.5283019,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1158.789,-840.7527;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;25;0;Create;True;0;0;False;0;1;0.86;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;764.2686,-562.4058;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;165;1710.603,-824.2581;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-515.8754,163.1064;Float;False;Property;_SmoothRough;Smooth/Rough;22;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;93;-879.5681,-29.05231;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;26;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1206.788,-532.4262;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-293.0748,8.706559;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;1382.672,-723.1528;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;167;1686.188,-376.4447;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1165.502,-341.0357;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartInnerGlow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;137;0;134;1
WireConnection;137;1;138;0
WireConnection;147;0;111;0
WireConnection;147;1;148;0
WireConnection;113;0;132;0
WireConnection;113;1;142;0
WireConnection;151;0;132;0
WireConnection;140;0;113;0
WireConnection;140;1;137;0
WireConnection;140;2;147;0
WireConnection;140;3;110;0
WireConnection;112;0;151;0
WireConnection;112;1;137;0
WireConnection;112;2;111;0
WireConnection;112;3;110;0
WireConnection;139;1;140;0
WireConnection;107;1;112;0
WireConnection;149;0;139;0
WireConnection;149;1;150;0
WireConnection;13;5;66;0
WireConnection;119;0;13;0
WireConnection;119;4;110;0
WireConnection;119;2;122;0
WireConnection;119;3;121;0
WireConnection;145;0;107;0
WireConnection;145;1;149;0
WireConnection;115;0;114;0
WireConnection;115;1;145;0
WireConnection;128;0;119;0
WireConnection;117;0;118;0
WireConnection;117;1;115;0
WireConnection;123;0;128;0
WireConnection;127;0;123;0
WireConnection;127;1;117;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;103;0;6;0
WireConnection;116;0;127;0
WireConnection;116;1;3;0
WireConnection;153;0;13;0
WireConnection;153;4;110;0
WireConnection;153;1;160;0
WireConnection;153;2;155;0
WireConnection;153;3;154;0
WireConnection;104;0;103;0
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;130;0;129;1
WireConnection;130;1;116;0
WireConnection;2;0;5;4
WireConnection;2;1;104;2
WireConnection;161;0;153;0
WireConnection;105;0;5;0
WireConnection;12;0;2;0
WireConnection;162;0;161;0
WireConnection;109;0;130;0
WireConnection;109;1;3;0
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;106;0;105;0
WireConnection;37;0;36;0
WireConnection;37;1;130;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;165;0;166;0
WireConnection;165;1;162;0
WireConnection;165;2;129;1
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;93;0;106;0
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;158;0;157;0
WireConnection;158;1;109;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;152;0;37;0
WireConnection;152;1;39;0
WireConnection;167;0;109;0
WireConnection;167;1;158;0
WireConnection;167;2;165;0
WireConnection;0;0;167;0
WireConnection;0;1;13;0
WireConnection;0;2;152;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=C459D9E59BE9D782AC2AE885943F064D0558EA3A