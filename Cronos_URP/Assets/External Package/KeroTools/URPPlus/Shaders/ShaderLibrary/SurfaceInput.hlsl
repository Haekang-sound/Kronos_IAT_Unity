#ifndef UNIVERSAL_INPUT_SURFACE_INCLUDED
#define UNIVERSAL_INPUT_SURFACE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "ShaderLibrary/SurfaceData.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/DetailMapping.hlsl"

///////////////////////////////////////////////////////////////////////////////
//                      Material Property Helpers                            //
///////////////////////////////////////////////////////////////////////////////

half RemapValue(half value, half2 range)
{
    return lerp(range.x, range.y, value);
}

half4 SampleAlbedoAlpha(half2 alphaRemap, float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
{
    half4 albedoAlpha = half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
    half alpha = RemapValue(albedoAlpha.a, alphaRemap);

    return half4(albedoAlpha.rgb, alpha);
}

half4 SampleAlbedoAlpha(half4 color, half2 alphaRemap, float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
{
    half4 albedoAlpha = SampleAlbedoAlpha(alphaRemap, uv, TEXTURE2D_ARGS(albedoAlphaMap, sampler_albedoAlphaMap));

    return color * albedoAlpha;
}

half Alpha(half alpha, half cutoff)
{
    #if defined(_ALPHATEST_ON)
        clip(alpha - cutoff);
    #endif

    return alpha;
}

half4 SampleMaskMap(float2 uv, TEXTURE2D_PARAM(maskMap, sampler_maskMap), half4 metallicSmoothnessRemap, half2 aoRemap, half4 MAODS)
{
    #ifdef _MASKMAP
        half4 maskValues = SAMPLE_TEXTURE2D(maskMap, sampler_maskMap, uv);
        maskValues.r = RemapValue(maskValues.r, metallicSmoothnessRemap.xy);
        maskValues.g = RemapValue(maskValues.g, aoRemap);
        maskValues.b = maskValues.b;
        maskValues.a = RemapValue(maskValues.a, metallicSmoothnessRemap.zw);

        return maskValues;
    #else
        return MAODS;
    #endif
}

// Returns clear coat parameters
// .x/.r == mask
// .y/.g == smoothness
half2 SampleClearCoat(float2 uv, TEXTURE2D_PARAM(clearCoatMap, sampler_clearCoatMap), half2 params)
{
    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        half2 clearCoatMaskSmoothness = params;

        #if defined(_CLEARCOATMAP)
            clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(clearCoatMap, sampler_clearCoatMap, uv).rg;
        #endif

        return clearCoatMaskSmoothness;
    #else
        return half2(0.0, 1.0);
    #endif
}

half3 ScaleNormal(half4 normal, half scale)
{
    #if BUMP_SCALE_NOT_SUPPORTED
        return UnpackNormal(normal);
    #else
        return UnpackNormalScale(normal, scale);
    #endif
}

half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(normalMap, sampler_normalMap), half scale = half(1.0))
{
    #if defined (_NORMALMAP) || (_BENTNORMALMAP) || (_COATNORMALMAP) || (_RAIN_NORMALMAP)
        half4 normal = SAMPLE_TEXTURE2D(normalMap, sampler_normalMap, uv);
        return ScaleNormal(normal, scale);
    #else
        return half3(0.0h, 0.0h, 1.0h);
    #endif
}

void ApplyDoubleSidedFlipOrMirror(half faceSign, half3 doubleSidedConstants, inout half3 normalTS)
{
    normalTS = faceSign > 0 ? normalTS : normalTS * doubleSidedConstants;
}

half3 SampleEmission(float2 uv, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap), half3 albedo, half3 emissionColor,
                     half emissionScale)
{
    #ifdef _EMISSION
        half3 emission = emissionScale * emissionColor * SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb;

        #ifdef _EMISSION_WITH_BASE
            emission *= albedo;
        #endif

        return emission;
    #else
        return 0;
    #endif
}

#endif
