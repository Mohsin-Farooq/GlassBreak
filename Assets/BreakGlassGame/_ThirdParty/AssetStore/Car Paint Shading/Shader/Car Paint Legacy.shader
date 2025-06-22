Shader "Car Paint Shading/Car Paint Legacy" {
	Properties {
		_BumpTex ("Bump", 2D) = "black" {}
		_SparkleTex ("Sparkle", 2D) = "black" {}
		_EnvTex ("Environment", CUBE) = "black" {}
		_EnvBrightness ("Env Brightness", Range(0, 10)) = 1
		_PaintColor0 ("Paint Color 0", Color) = (0.4, 0, 0.35, 1)
		_PaintColorMid ("Paint Color Mid", Color) = (0.6, 0, 0, 1)
		_PaintColor2 ("Paint Color 2", Color) = (0, 0.35, 0, 1)
		_FlakeColor ("Sparkle Color", Color) = (0.5, 0.5, 0, 1)
		_SparkleScale ("Sparkle Scale", Range(1, 32)) = 20
		_SparklePert ("Sparkle Perturbation", Range(0, 2)) = 1
	}
	SubShader {
		Pass {
			Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "LightMode" = "ForwardBase" }
		
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			uniform float4 _BumpTex_ST;
			uniform sampler2D _BumpTex;
			uniform sampler2D _SparkleTex;
			uniform samplerCUBE _EnvTex;
			uniform float _EnvBrightness;
			uniform float4 _PaintColor0;
			uniform float4 _PaintColorMid;
			uniform float4 _PaintColor2;
			uniform float4 _FlakeColor;
			uniform float _SparkleScale;
			uniform float _SparklePert;
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float3 wldview : TEXCOORD1;
				float3 wldnor : TEXCOORD2;
				float3 wldtan : TEXCOORD3;
				float3 wldbin : TEXCOORD4;
				LIGHTING_COORDS(5, 6)
			};
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			v2f vert (appdata_tan v)
			{
				TANGENT_SPACE_ROTATION;
				float2 uv = TRANSFORM_TEX(v.texcoord, _BumpTex);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex.xy = uv;
				o.tex.zw = uv * _SparkleScale;
				o.wldview = WorldSpaceViewDir(v.vertex);
				o.wldnor = mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL);
				o.wldtan = mul((float3x3)unity_ObjectToWorld, v.tangent.xyz);
				o.wldbin = mul((float3x3)unity_ObjectToWorld, binormal.xyz);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			float4 frag (v2f i) : SV_TARGET
			{
				float4 bump = tex2D(_BumpTex, i.tex.xy) * 2 - 1;
				float3 N = normalize(i.wldnor);
				float3 T = normalize(i.wldtan);
				float3 B = normalize(i.wldbin);
				float3 V = normalize(i.wldview);
				
				float3 flakeBump = tex2D(_SparkleTex, i.tex.zw).xyz * 2 - 1;
				float3 flakeN = normalize(N + B * flakeBump.x - T * flakeBump.y);

				N = normalize(N + B * bump.x - T * bump.y);
				
				float ndv = saturate(dot(N, V));
				float3 refl = 2 * N * ndv - V;
				
				float4 envMap = texCUBE(_EnvTex, refl);
				envMap.rgb *= envMap.a;
				envMap.rgb *= _EnvBrightness;
				
				float3 N1 = 0.1 * flakeN + 1.0 * N;
//				float3 N1 = _MicroflakePerturbationA * flakeN + _NormalPerturbation * N;
				float3 N2 = _SparklePert * (flakeN + N);
				
				float fsnl1 = saturate(dot(N1, V));
				float fsnl2 = saturate(dot(N2, V));
				float fsnl1Sq = fsnl1 * fsnl1;
				float4 paintColor = fsnl1 * _PaintColor0 + fsnl1Sq * _PaintColorMid + fsnl1Sq * fsnl1Sq * _PaintColor2 + pow(fsnl2, 16) * _FlakeColor;

				return envMap * (1.0 - 0.5 * ndv) + paintColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
