Shader "Car Paint Shading/Car Paint Transparent" {
	Properties {
		[Header(Basic)]
		_Color            ("Tint", Color) = (1, 1, 1, 1)
		_MainTex          ("Albedo", 2D) = "white" {}
		_Glossiness       ("Smoothness", Range(0, 1)) = 0.5
		_Metallic         ("Metallic", Range(0, 1)) = 0
		_BumpScale        ("Bump Scale", Float) = 1
		[Normal] _BumpMap ("Bump Map", 2D) = "bump" {}
		_EmissionColor    ("Emission", Color) = (0, 0, 0, 1)
		[Header(Flake Bump)]
		[NoScaleOffset] [Normal] _FlakesBumpMap ("Flakes Bump", 2D) = "bump" {}
		_FlakesBumpMapScale  ("Flakes Scale", Float) = 1.0
		_FlakesBumpStrength  ("Flakes Strength", Range(0.001, 1)) = 0.2
		[Header(Flake)]
		_FlakeColor          ("Flakes", Color) = (1, 1, 1, 1)
		_FlakesColorStrength ("Flakes Strength", Range(0, 10)) = 1
		_FlakesColorCutoff   ("Flakes Cutoff", Range(0, 0.95)) = 0.5
		[Header(Fresnel)]
		_FresnelColor ("Color", Color) = (1, 1, 1, 1)
		_FresnelPower ("Power", Range(0, 10)) = 1
		[Header(Reflection)]
		_ReflectionSpecular   ("Specular", Color) = (0.3, 0.3, 0.3, 1)
		_ReflectionGlossiness ("Smoothness", Range(0, 1)) = 1
		[Header(Flake)]
		_SparklePert ("Flakes Strength", Range(0, 10)) = 1
		_PaintColor0 ("Paint Color 0", Color) = (0.4, 0, 0.35, 1)
		_PaintColor1 ("Paint Color 1", Color) = (0.6, 0, 0, 1)
		_PaintColor2 ("Paint Color 2", Color) = (0, 0.35, 0, 1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="false" }

		// base paint layer
		CGPROGRAM
			#pragma surface BasePaintSurface Standard fullforwardshadows alpha:fade
			#pragma target 3.0
			#pragma multi_compile _ FLAKES_COLOR_BLEND FLAKES_COLOR_CUTOFF
			#pragma multi_compile _ MULTI_LAYER_EMISSION
			#include "LayerBase.cginc"
		ENDCG

		// reflective coating layer
		CGPROGRAM
			#pragma surface ReflectiveCoatingSurface StandardSpecular nofog alpha:premul
			#pragma target 3.0
			#include "LayerBase.cginc"
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "CarPaintEditor"
}
