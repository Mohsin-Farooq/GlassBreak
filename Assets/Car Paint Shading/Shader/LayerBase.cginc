#ifndef LAYER_BASE_INCLUDED
#define LAYER_BASE_INCLUDED

struct Input 
{
	float2 uv_MainTex;
	float2 uv_FlakesBumpMap;
	float3 viewDir;
};

// basic
fixed4 _Color;
sampler2D _MainTex, _BumpMap;
half _Glossiness, _Metallic, _BumpScale;
half4 _EmissionColor;

// flakes bump and color
sampler2D _FlakesBumpMap;
half _FlakesBumpMapScale, _FlakesBumpStrength;
fixed4 _FlakeColor;
float _FlakesColorStrength, _FlakesColorCutoff;

// fresnel
fixed4 _FresnelColor;
float _FresnelPower;

half _SparklePert;
fixed4 _PaintColor0, _PaintColor1, _PaintColor2;

void BasePaintSurface (Input IN, inout SurfaceOutputStandard o) 
{
	// surface normal from normal map
	half3 bumpNormal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);

	// scaled flake normal map
	float2 uv = IN.uv_FlakesBumpMap * _FlakesBumpMapScale;
	half3 flakeNormal = UnpackNormal(tex2D(_FlakesBumpMap, uv));

	half3 scaledFlakeNormal = flakeNormal;
	scaledFlakeNormal.xy *= _FlakesBumpStrength;
	scaledFlakeNormal.z = 0;    // Z set to 0 for better blending with other normal map.
	half3 N = normalize(bumpNormal + scaledFlakeNormal);

	o.Normal = N;

	// albedo
	fixed4 base = tex2D(_MainTex, IN.uv_MainTex);
	fixed3 c = _Color.rgb * base.rgb;

	// fresnel
	float3 V = normalize(IN.viewDir);
	float fresnel =  1.0 - max(dot(V, bumpNormal), 0.0);
	fresnel = pow(fresnel, _FresnelPower);
	c = lerp(c, _FresnelColor.rgb, fresnel);

	// flake color
#if FLAKES_COLOR_BLEND
	half ratio = saturate((1 - flakeNormal.z) * _FlakesColorStrength);
	c = lerp(c, _FlakeColor.rgb, ratio * _FlakeColor.a);
#elif FLAKES_COLOR_CUTOFF
	if (flakeNormal.z < _FlakesColorCutoff)
		c = lerp(c, _FlakeColor.rgb * _FlakesColorStrength, _FlakeColor.a);
#endif

	// multi layer emission
#if MULTI_LAYER_EMISSION
	float3 N1 = 0.1 * flakeNormal + 1.0 * N;
	float3 N2 = _SparklePert * (flakeNormal + N);
	float fsnl1 = saturate(dot(N1, V));
	float fsnl2 = saturate(dot(N2, V));
	float fsnl1Sq = fsnl1 * fsnl1;
	float4 emission = fsnl1 * _PaintColor0 + fsnl1Sq * _PaintColor1 + fsnl1Sq * fsnl1Sq * _PaintColor2 + pow(fsnl2, 16) * _FlakeColor;
#else
	float4 emission = _EmissionColor;
#endif

	o.Albedo = c;
	o.Metallic = _Metallic;
	o.Smoothness = _Glossiness;
	o.Emission = emission.rgb;
	o.Alpha = _Color.a * base.a;
}

float _ReflectionGlossiness;
fixed4 _ReflectionSpecular;

void ReflectiveCoatingSurface (Input IN, inout SurfaceOutputStandardSpecular o) 
{
	o.Normal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_MainTex), _BumpScale);
	o.Alpha = 0;
	o.Albedo = 0;
	o.Specular = _ReflectionSpecular.rgb;
	o.Smoothness = _ReflectionGlossiness;
}

#endif