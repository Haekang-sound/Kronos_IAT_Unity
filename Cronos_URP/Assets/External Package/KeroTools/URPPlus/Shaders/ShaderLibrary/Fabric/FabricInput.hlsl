#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/Fabric/ThreadMapping.hlsl"
#include "ShaderLibrary/Fabric/FabricProperties.hlsl"
#include "ShaderLibrary/Fabric/FabricMaps.hlsl"
#include "ShaderLibrary/Weather.hlsl"

inline void InitializeSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = EmptyFill();

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half4 albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.albedo = albedoAlpha.rgb;
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _AlphaCutoff);
    outSurfaceData.albedo = AlphaModulate(outSurfaceData.albedo, outSurfaceData.alpha);

    half4 MAODS = half4(0.0, 1.0, 1.0, _Smoothness);
    half4 metallicGlossMinMax = half4(0.0, 1.0, _SmoothnessRemapMin, _SmoothnessRemapMax);
    half2 aoMinMax = half2(_AORemapMin, _AORemapMax);
    half4 maskMap = SampleMaskMap(uv, TEXTURE2D_ARGS(_MaskMap, sampler_MaskMap), metallicGlossMinMax, aoMinMax, MAODS);

    outSurfaceData.specular = SAMPLE_TEXTURE2D(_SpecularColorMap, sampler_SpecularColorMap, uv).rgb * _SpecularColor.rgb;

    outSurfaceData.anisotropy = _Anisotropy;
    outSurfaceData.smoothness = maskMap.a;
    outSurfaceData.occlusion = maskMap.g;

    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_NormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    outSurfaceData.bentNormalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BentNormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);

    outSurfaceData.emission = SampleEmission(uv, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap),
                                                outSurfaceData.albedo, _EmissionColor.rgb, _EmissionScale);

    float2 threadUV = TRANSFORM_TEX(uv, _ThreadMap);
    #ifdef _THREADMAP
        half3 threadRemap = half3(_ThreadAOScale, _ThreadNormalScale, _ThreadSmoothnessScale);
        ApplyThreadMapping(threadUV, threadRemap, outSurfaceData);
    #endif

    #ifdef _FUZZMAP
        half fuzz = lerp(0.0h, SAMPLE_TEXTURE2D(_FuzzMap, sampler_FuzzMap, _FuzzScale * threadUV).r, _FuzzIntensity);
        outSurfaceData.albedo = saturate(outSurfaceData.albedo + fuzz.xxx);
    #endif

    outSurfaceData.diffusionColor = _DiffusionColor.rgb;
    outSurfaceData.thickness = _Thickness;
    #ifdef _THICKNESSCURVATUREMAP
        outSurfaceData.thickness = RemapValue(SAMPLE_TEXTURE2D(_ThicknessCurvatureMap, sampler_ThicknessCurvatureMap, uv).r, _ThicknessRemap.xy);
    #endif
    outSurfaceData.translucencyScale = _TranslucencyScale;
    outSurfaceData.translucencyPower = 100.0h * _TranslucencyPower;
    outSurfaceData.translucencyAmbient = _TranslucencyAmbient;
    outSurfaceData.translucencyDistortion = _TranslucencyDistortion;
    outSurfaceData.translucencyShadows = _TranslucencyShadows;

    outSurfaceData.horizonFade = _HorizonFade;
    outSurfaceData.giOcclusionBias = _GIOcclusionBias;
}

#endif