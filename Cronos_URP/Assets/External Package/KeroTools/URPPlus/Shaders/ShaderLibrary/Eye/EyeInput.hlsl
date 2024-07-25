#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/Eye/EyeProperties.hlsl"
#include "ShaderLibrary/Eye/EyeMaps.hlsl"

half CalculateSurfaceMask(float3 positionOS)
{
    half irisRadius = 0.225h;
    half osRadius = length(positionOS.xy);
    half innerBlendRegionRadius = irisRadius - 0.02;
    half outerBlendRegionRadius = irisRadius + 0.02;
    half irisFactor = osRadius - irisRadius;
    half blendLerpFactor = 1.0 - irisFactor / (0.04);
    blendLerpFactor = pow(blendLerpFactor, 8.0);
    blendLerpFactor = 1.0 - blendLerpFactor;

    return (osRadius > outerBlendRegionRadius) ? 0.0 : ((osRadius < irisRadius) ? 1.0 : (lerp(1.0, 0.0, blendLerpFactor)));
}

half3 SampleEyeNormal(float2 uv, TEXTURE2D_PARAM(normalMap, sampler_normalMap), half scale = half(1.0))
{
    #if defined (_SCLERA_NORMALMAP) || defined (_IRIS_NORMALMAP)
        half4 normal = SAMPLE_TEXTURE2D(normalMap, sampler_normalMap, uv);
        return ScaleNormal(normal, scale);
    #else
        return half3(0.0h, 0.0h, 1.0h);
    #endif
}

inline void InitializeSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = EmptyFill();

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half4 albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.albedo = albedoAlpha.rgb;
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _AlphaCutoff);
    outSurfaceData.albedo = AlphaModulate(outSurfaceData.albedo, outSurfaceData.alpha);
}

#endif