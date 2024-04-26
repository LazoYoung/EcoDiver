// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_StandartVertexCDetails"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoBaseDetail("AlbedoBaseDetail", 2D) = "white" {}
		_AlbedoDetailStrenth("AlbedoDetailStrenth", Float) = 0
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		_EmissionMap("EmissionMap", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMultiplayer("EmissionMultiplayer", Float) = 0
		_DetailNormals("DetailNormals", 2D) = "bump" {}
		_MaskIntens("MaskIntens", Range( 0 , 1)) = 0
		_MaskContrast("MaskContrast", Float) = 1
		_RVCMaskContrast("RVCMaskContrast", Range( 0 , 5)) = 1
		_YContrast("YContrast", Float) = 1
		_DetailNormalsScale("DetailNormalsScale", Float) = 1
		_RedNormals("RedNormals", 2D) = "bump" {}
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
		_RVCAlbedo("RVCAlbedo", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_RVCTint("RVCTint", Color) = (0,0,0,0)
		_YProjTint("YProjTint", Color) = (0.4941176,0.4941176,0.4941176,0.4941176)
		_AlbedoDetailBrightness("AlbedoDetailBrightness", Float) = 1
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
			float4 vertexColor : COLOR;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _DetailNormalsScale;
		uniform sampler2D _DetailNormals;
		uniform float4 _DetailNormals_ST;
		uniform float _NormalMapDepth;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _BVCNormalsScale;
		uniform sampler2D _RedNormals;
		uniform float4 _RedNormals_ST;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AlbedoColor;
		uniform float4 _RVCTint;
		uniform sampler2D _RVCAlbedo;
		uniform float4 _RVCAlbedo_ST;
		uniform float _RVCMaskContrast;
		uniform float4 _YProjTint;
		uniform float _MaskIntens;
		uniform float _YContrast;
		uniform float _MaskContrast;
		uniform sampler2D _AlbedoBaseDetail;
		uniform float4 _AlbedoBaseDetail_ST;
		uniform float _AlbedoDetailBrightness;
		uniform float _AlbedoDetailStrenth;
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
			float2 uv_DetailNormals = i.uv_texcoord * _DetailNormals_ST.xy + _DetailNormals_ST.zw;
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode13 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ) ,_NormalMapDepth );
			float2 uv_RedNormals = i.uv_texcoord * _RedNormals_ST.xy + _RedNormals_ST.zw;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode114 = tex2D( _Mask, uv_Mask );
			float ifLocalVar243 = 0;
			if( ( i.vertexColor.r * i.vertexColor.g * i.vertexColor.b ) >= 0.99 )
				ifLocalVar243 = 0.0;
			else
				ifLocalVar243 = i.vertexColor.b;
			float HeightMask245 = saturate(pow(((tex2DNode114.r*ifLocalVar243)*4)+(ifLocalVar243*2),1.0));
			float3 lerpResult142 = lerp( BlendNormals( UnpackScaleNormal( tex2D( _DetailNormals, uv_DetailNormals ) ,_DetailNormalsScale ) , tex2DNode13 ) , BlendNormals( UnpackScaleNormal( tex2D( _RedNormals, uv_RedNormals ) ,_BVCNormalsScale ) , tex2DNode13 ) , HeightMask245);
			o.Normal = lerpResult142;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _Albedo, uv_Albedo ) * _AlbedoColor );
			float2 uv_RVCAlbedo = i.uv_texcoord * _RVCAlbedo_ST.xy + _RVCAlbedo_ST.zw;
			float4 blendOpSrc229 = temp_output_3_0;
			float4 blendOpDest229 = ( _RVCTint * tex2D( _RVCAlbedo, uv_RVCAlbedo ) );
			float ifLocalVar231 = 0;
			if( ( i.vertexColor.r * i.vertexColor.g * i.vertexColor.b ) >= 0.99 )
				ifLocalVar231 = 0.0;
			else
				ifLocalVar231 = i.vertexColor.r;
			float clampResult228 = clamp( ifLocalVar231 , 0.0 , 1.0 );
			float HeightMask247 = saturate(pow(((tex2DNode114.r*clampResult228)*4)+(clampResult228*2),_RVCMaskContrast));
			float4 lerpResult246 = lerp( temp_output_3_0 , ( saturate( (( blendOpDest229 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest229 - 0.5 ) ) * ( 1.0 - blendOpSrc229 ) ) : ( 2.0 * blendOpDest229 * blendOpSrc229 ) ) )) , HeightMask247);
			float4 blendOpSrc218 = lerpResult246;
			float4 blendOpDest218 = _YProjTint;
			float clampResult222 = clamp( ( ( WorldNormalVector( i , tex2DNode13 ).y - _MaskIntens ) * _YContrast ) , 0.0 , 1.0 );
			float HeightMask210 = saturate(pow(((tex2DNode114.r*clampResult222)*4)+(clampResult222*2),_MaskContrast));
			float clampResult219 = clamp( HeightMask210 , 0.0 , 1.0 );
			float4 lerpResult213 = lerp( lerpResult246 , ( saturate( (( blendOpDest218 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest218 - 0.5 ) ) * ( 1.0 - blendOpSrc218 ) ) : ( 2.0 * blendOpDest218 * blendOpSrc218 ) ) )) , clampResult219);
			float2 uv_AlbedoBaseDetail = i.uv_texcoord * _AlbedoBaseDetail_ST.xy + _AlbedoBaseDetail_ST.zw;
			float4 blendOpSrc157 = lerpResult213;
			float4 blendOpDest157 = ( tex2D( _AlbedoBaseDetail, uv_AlbedoBaseDetail ) * _AlbedoDetailBrightness );
			float4 lerpResult159 = lerp( lerpResult213 , ( saturate(  (( blendOpSrc157 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc157 - 0.5 ) ) * ( 1.0 - blendOpDest157 ) ) : ( 2.0 * blendOpSrc157 * blendOpDest157 ) ) )) , _AlbedoDetailStrenth);
			o.Albedo = lerpResult159.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 tex2DNode33 = tex2D( _EmissionMap, uv_EmissionMap );
			o.Emission = ( _EmissionColor * ( _EmissionMultiplayer * lerp(tex2DNode33,( temp_output_3_0 * tex2DNode33.a ),_EmissionSwitch) ) ).rgb;
			float2 uv_MetallicMap = i.uv_texcoord * _MetallicMap_ST.xy + _MetallicMap_ST.zw;
			float4 tex2DNode5 = tex2D( _MetallicMap, uv_MetallicMap );
			float lerpResult93 = lerp( tex2DNode5.r , 1.0 , _MetalicBrightnes);
			o.Metallic = ( _Metallic * lerpResult93 );
			float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
			o.Smoothness = ( lerp(lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch),( 1.0 - lerp(tex2DNode5.a,tex2D( _SmoothnessMap, uv_SmoothnessMap ).r,_SmoothFromMapSwitch) ),_SmoothRough) * _Snoothness );
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
7;362;1643;791;1453.843;1890.142;1.83312;True;True
Node;AmplifyShaderEditor.RangedFloatNode;66;-1675.297,-613.4612;Float;False;Property;_NormalMapDepth;NormalMapDepth;16;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-1379.019,-727.4025;Float;True;Property;_NormalMap;NormalMap;15;0;Create;True;0;0;False;0;None;b386894396b1b0d4e9abeaacdd596398;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;209;-1651.341,-459.8335;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;185;-1622.054,-1592.271;Float;False;Property;_MaskIntens;MaskIntens;8;0;Create;True;0;0;False;0;0;0.36;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;181;-1735.835,-1802.791;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;236;-1161.435,-519.3631;Float;False;Constant;_Float1;Float 1;31;0;Create;True;0;0;False;0;0.99;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;235;-1355.029,-471.1995;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-1209.776,-368.2791;Float;False;Constant;_Float2;Float 2;31;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;187;-1254.444,-1653.76;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;231;-1014.416,-474.2572;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;111;-571.3576,-731.3256;Float;True;Property;_RVCAlbedo;RVCAlbedo;25;0;Create;True;0;0;False;0;None;7bf0f3566beb89f4fb8785f70c000c82;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-403.4567,-799.6359;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;70d0b93d29d52c04b8bfe3c008140934;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;125;-373.1619,-1261.392;Float;False;Property;_RVCTint;RVCTint;27;0;Create;True;0;0;False;0;0,0,0,0;0.7119679,0.764151,0.4361427,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-327.5488,-513.1075;Float;False;Property;_AlbedoColor;AlbedoColor;3;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;190;-1193.037,-1416.334;Float;False;Property;_YContrast;YContrast;11;0;Create;True;0;0;False;0;1;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-53.69505,-1093.738;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;114;-1225.111,-1934.072;Float;True;Property;_Mask;Mask;26;0;Create;True;0;0;False;0;7937c4e638262954985abb567fcbb3f4;7937c4e638262954985abb567fcbb3f4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-924.163,-1617.515;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-990.8251,-244.8082;Float;False;Property;_RVCMaskContrast;RVCMaskContrast;10;0;Create;True;0;0;False;0;1;0.65;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;0.9208729,-670.8475;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;228;-753.6391,-620.4682;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;222;-713.8583,-1617.157;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-886.0778,-1459.841;Float;False;Property;_MaskContrast;MaskContrast;9;0;Create;True;0;0;False;0;1;0.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;229;139.0274,-1035.316;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.HeightMapBlendNode;247;-621.8157,-451.446;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1291.196,248.5266;Float;True;Property;_SmoothnessMap;SmoothnessMap;20;0;Create;True;0;0;False;0;None;0bf50bfa2573c69449fca8f4c9f43a25;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;246;390.8459,-780.437;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-483.3045,-288.8951;Float;True;Property;_EmissionMap;EmissionMap;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HeightMapBlendNode;210;-459.2956,-1668.031;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1412.218,-30.17479;Float;True;Property;_MetallicMap;MetallicMap;21;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;211;-373.9151,-1445.321;Float;False;Property;_YProjTint;YProjTint;28;0;Create;True;0;0;False;0;0.4941176,0.4941176,0.4941176,0.4941176;1,0.8466981,0.5707546,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;240;-2650.143,-1441.942;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-896.6956,186.1265;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;17;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;149.2783,-266.6906;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1667.916,-1249.94;Float;False;Property;_DetailNormalsScale;DetailNormalsScale;12;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;-2379.993,-1452.032;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;250;609.0364,-568.6445;Float;False;Property;_AlbedoDetailBrightness;AlbedoDetailBrightness;29;0;Create;True;0;0;False;0;1;2.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;219;-127.6493,-1605.566;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-1682.207,-915.3988;Float;False;Property;_BVCNormalsScale;BVCNormalsScale;14;0;Create;True;0;0;False;0;1;0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;-2204.866,-1292.399;Float;False;Constant;_Float3;Float 3;31;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;218;340.9874,-1300.869;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;158;691.8766,-1095.195;Float;True;Property;_AlbedoBaseDetail;AlbedoBaseDetail;1;0;Create;True;0;0;False;0;None;89ee248f3f8054548b2cbf49274c5540;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;244;-2160.237,-1501.472;Float;False;Constant;_Float4;Float 4;31;0;Create;True;0;0;False;0;0.99;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;128;-1401.475,-1299.794;Float;True;Property;_DetailNormals;DetailNormals;7;0;Create;True;0;0;False;0;None;fb6566c21f717904f83743a5a76dd0b0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;1054.036,-926.6445;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1389.108,-135.8561;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;213;704.1131,-1367.49;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;126;-1404.541,-1007.782;Float;True;Property;_RedNormals;RedNormals;13;0;Create;True;0;0;False;0;None;0f23a69e8aee27d4c8bec6f1e96c3c0e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;12;-658.3749,320.7062;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;309.5075,-553.4102;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1387.871,-249.1329;Float;False;Property;_MetalicBrightnes;MetalicBrightnes;24;0;Create;True;0;0;False;0;0.4494838;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;243;-1886.345,-1447.813;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;32;311.8829,-451.2662;Float;False;Property;_EmissionSwitch;EmissionSwitch;18;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;36;712.543,-823.2645;Float;False;Property;_EmissionColor;EmissionColor;5;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;157;1081.592,-1238.443;Float;True;HardLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;93;-989.6251,-135.9055;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;141;-917.2952,-843.1384;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;127;-904.4874,-1044.344;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-515.8754,163.1064;Float;False;Property;_SmoothRough;Smooth/Rough;19;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HeightMapBlendNode;245;-866.8876,-1299.251;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;1092.815,-710.9174;Float;False;Property;_AlbedoDetailStrenth;AlbedoDetailStrenth;2;0;Create;True;0;0;False;0;0;0.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;22;0;Create;True;0;0;False;0;1;10.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;23;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;707.6378,-499.3287;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1125.931,-564.4491;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-305.8855,-44.36689;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;142;-615.2476,-880.8334;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;159;1382.511,-1243.335;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1511.613,-437.0081;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;ASE/ASE_StandartVertexCDetails;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;5;66;0
WireConnection;181;0;13;0
WireConnection;235;0;209;1
WireConnection;235;1;209;2
WireConnection;235;2;209;3
WireConnection;187;0;181;2
WireConnection;187;1;185;0
WireConnection;231;0;235;0
WireConnection;231;1;236;0
WireConnection;231;2;239;0
WireConnection;231;3;239;0
WireConnection;231;4;209;1
WireConnection;124;0;125;0
WireConnection;124;1;111;0
WireConnection;196;0;187;0
WireConnection;196;1;190;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;228;0;231;0
WireConnection;222;0;196;0
WireConnection;229;0;3;0
WireConnection;229;1;124;0
WireConnection;247;0;114;1
WireConnection;247;1;228;0
WireConnection;247;2;248;0
WireConnection;246;0;3;0
WireConnection;246;1;229;0
WireConnection;246;2;247;0
WireConnection;210;0;114;1
WireConnection;210;1;222;0
WireConnection;210;2;221;0
WireConnection;2;0;5;4
WireConnection;2;1;6;1
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;241;0;240;1
WireConnection;241;1;240;2
WireConnection;241;2;240;3
WireConnection;219;0;210;0
WireConnection;218;0;246;0
WireConnection;218;1;211;0
WireConnection;128;5;154;0
WireConnection;249;0;158;0
WireConnection;249;1;250;0
WireConnection;213;0;246;0
WireConnection;213;1;218;0
WireConnection;213;2;219;0
WireConnection;126;5;130;0
WireConnection;12;0;2;0
WireConnection;243;0;241;0
WireConnection;243;1;244;0
WireConnection;243;2;242;0
WireConnection;243;3;242;0
WireConnection;243;4;240;3
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;157;0;213;0
WireConnection;157;1;249;0
WireConnection;93;0;5;1
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;141;0;126;0
WireConnection;141;1;13;0
WireConnection;127;0;128;0
WireConnection;127;1;13;0
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;245;0;114;1
WireConnection;245;1;243;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;142;0;127;0
WireConnection;142;1;141;0
WireConnection;142;2;245;0
WireConnection;159;0;213;0
WireConnection;159;1;157;0
WireConnection;159;2;161;0
WireConnection;0;0;159;0
WireConnection;0;1;142;0
WireConnection;0;2;37;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=2AA494E3C87AA88FC712276982285FE700F2F6CD