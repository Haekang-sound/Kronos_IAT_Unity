#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/Lit/LitProperties.hlsl"
#include "ShaderLibrary/Lit/LitMaps.hlsl"
#include "ShaderLibrary/Weather.hlsl"

inline void InitializeSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = EmptyFill();

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half4 albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.albedo = albedoAlpha.rgb;
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _AlphaCutoff);
    outSurfaceData.albedo = AlphaModulate(outSurfaceData.albedo, outSurfaceData.alpha);

    half4 MAODS = half4(_Metallic, 1.0, 1.0, _Smoothness);
    half4 metallicGlossMinMax = half4(_MetallicRemapMin, _MetallicRemapMax, _SmoothnessRemapMin, _SmoothnessRemapMax);
    half2 aoMinMax = half2(_AORemapMin, _AORemapMax);
    half4 maskMap = SampleMaskMap(uv, TEXTURE2D_ARGS(_MaskMap, sampler_MaskMap), metallicGlossMinMax, aoMinMax, MAODS);

    #if _SPECULAR_SETUP
        outSurfaceData.metallic = half(1.0);
        outSurfaceData.specular = SAMPLE_TEXTURE2D(_SpecularColorMap, sampler_SpecularColorMap, uv).rgb * _SpecularColor.rgb;
    #else
        outSurfaceData.metallic = maskMap.r;
        outSurfaceData.specular = half3(0.0, 0.0, 0.0);
    #endif

    outSurfaceData.smoothness = maskMap.a;
    outSurfaceData.occlusion = maskMap.g;

    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_NormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    outSurfaceData.bentNormalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BentNormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    outSurfaceData.coatNormalTS = SampleNormal(uv, TEXTURE2D_ARGS(_CoatNormalMap, SAMPLER_NORMALMAP_IDX), _CoatNormalScale);

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        half2 clearCoat = SampleClearCoat(uv, TEXTURE2D_ARGS(_ClearCoatMap, sampler_ClearCoatMap), half2(_ClearCoatMask, _ClearCoatSmoothness));
        outSurfaceData.clearCoatMask = clearCoat.r;
        outSurfaceData.clearCoatSmoothness = clearCoat.g;
    #endif

    outSurfaceData.ior = _Ior;
    outSurfaceData.transmittanceColor = _TransmittanceColor.rgb * SAMPLE_TEXTURE2D(_TransmittanceColorMap, sampler_TransmittanceColorMap, uv).rgb;
    outSurfaceData.chromaticAberration = _ChromaticAberration;

    #if defined(_DETAIL)
        float2 detailUV = TRANSFORM_TEX(uv, _DetailMap);
        half4 detail = SAMPLE_TEXTURE2D(_DetailMap, sampler_DetailMap, detailUV);
        half3 detailRemap = half3(_DetailAlbedoScale, _DetailNormalScale, _DetailSmoothnessScale);
        
        ApplyHDRPDetailMapping(detail, detailRemap, maskMap.b, 
                                outSurfaceData.albedo, outSurfaceData.normalTS, outSurfaceData.smoothness);
    #endif

    outSurfaceData.emission = SampleEmission(uv, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap),
                                                outSurfaceData.albedo, _EmissionColor.rgb, _EmissionScale);

    outSurfaceData.horizonFade = _HorizonFade;
    outSurfaceData.giOcclusionBias = _GIOcclusionBias;
}

#endif