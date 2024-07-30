#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/SurfaceInput.hlsl"
#include "ShaderLibrary/Hair/HairData.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/Hair/HairProperties.hlsl"
#include "ShaderLibrary/Hair/HairMaps.hlsl"

#define DEFAULT_HAIR_SPECULAR_VALUE 0.0465

inline void InitializeSurfaceData(float2 uv, out SurfaceData outSurfaceData, out HairData outHairData)
{
    /////////////////////////////////
    /**********SurfaceData**********/
    /////////////////////////////////
    outSurfaceData = EmptyFill();
    float2 baseUV = TRANSFORM_TEX(uv, _BaseMap);

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half4 albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, baseUV, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.albedo = albedoAlpha.rgb;
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _AlphaCutoff);
    outSurfaceData.albedo = AlphaModulate(outSurfaceData.albedo, outSurfaceData.alpha);

    outSurfaceData.specular = DEFAULT_HAIR_SPECULAR_VALUE;

    #ifdef _AO_MAP
        half2 aoRemap = half2(_AORemapMin, _AORemapMax);
        half ambientOcclusion = SAMPLE_TEXTURE2D(_AmbientOcclusionMap, sampler_AmbientOcclusionMap, baseUV).r;
        outSurfaceData.occlusion = RemapValue(ambientOcclusion, aoRemap);
    #endif

    outSurfaceData.normalTS = SampleNormal(baseUV, TEXTURE2D_ARGS(_NormalMap, sampler_NormalMap), _NormalScale);

    ////////////////////////////////
    /***********HairData***********/
    ////////////////////////////////
    half smoothnessValue = _Smoothness;
    #ifdef _SMOOTHNESS_MASK
        float2 smoothnessUV = TRANSFORM_TEX(uv, _SmoothnessMaskMap);
        half2 smoothnessMaskRemap = half2(_SmoothnessRemapMin, _SmoothnessRemapMax);
        half smoothnessMask = SAMPLE_TEXTURE2D(_SmoothnessMaskMap, sampler_SmoothnessMaskMap, smoothnessUV).r;
        smoothnessValue = RemapValue(smoothnessMask, smoothnessMaskRemap);
    #endif

    outHairData.specularTint = albedoAlpha.a * lerp(half3(1.0h, 1.0h, 1.0h), _SpecularColor.rgb, 0.3h) * _SpecularMultiplier;
    outHairData.secondarySpecularTint = albedoAlpha.a * lerp(half3(0.0h, 0.0h, 0.0h), _SpecularColor.rgb, 0.5h) * _SecondarySpecularMultiplier;

    outHairData.specularShift = _SpecularShift;
    outHairData.secondarySpecularShift = _SecondarySpecularShift;
    outHairData.perceptualSmoothness = smoothnessValue;
    outHairData.secondaryPerceptualSmoothness = smoothnessValue;
    
    outHairData.specularTint *= outHairData.perceptualSmoothness;
    outHairData.secondarySpecularTint *= outHairData.secondaryPerceptualSmoothness;

    outHairData.transmissionColor = _TransmissionColor.rgb;
    outHairData.transmissionIntensity = _TransmissionIntensity;
}

#endif