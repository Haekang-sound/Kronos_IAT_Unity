#ifndef UNIVERSAL_BRDF_INCLUDED
#define UNIVERSAL_BRDF_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Deprecated.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#include "ShaderLibrary/BSDF.hlsl"

// Standard dielectric reflectivity coef at incident angle (= 4%)
#define kDielectricSpec half4(0.04, 0.04, 0.04, 1.0 - 0.04) 

struct BRDFData
{
    half3 albedo;
    half3 diffuse;
    half3 specular;
    half reflectivity;
    half perceptualRoughness;
    half roughness;
    half roughness2;
    half grazingTerm;

    half normalizationTerm; // roughness * 4.0 + 2.0
    half roughness2MinusOne; // roughness^2 - 1.0
};

half ReflectivitySpecular(half3 specular)
{
    return Max3(specular.r, specular.g, specular.b);
}

half OneMinusReflectivityMetallic(half metallic)
{
    half oneMinusDielectricSpec = kDielectricSpec.a;

    return oneMinusDielectricSpec - metallic * oneMinusDielectricSpec;
}

half MetallicFromReflectivity(half reflectivity)
{
    half oneMinusDielectricSpec = kDielectricSpec.a;
    return (reflectivity - kDielectricSpec.r) / oneMinusDielectricSpec;
}

inline void InitializeBRDFDataDirect(half3 albedo, half3 diffuse, half3 specular, half reflectivity, 
                                        half oneMinusReflectivity, half smoothness, inout half alpha, 
                                        out BRDFData outBRDFData)
{
    outBRDFData = (BRDFData)0;
    outBRDFData.albedo = albedo;
    outBRDFData.diffuse = diffuse;
    outBRDFData.specular = specular;
    outBRDFData.reflectivity = reflectivity;

    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(smoothness);
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    outBRDFData.roughness2 = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.grazingTerm = saturate(smoothness + reflectivity);
    outBRDFData.normalizationTerm = outBRDFData.roughness * 4.0h + 2.0h;
    outBRDFData.roughness2MinusOne = outBRDFData.roughness2 - 1.0h;

    #if defined(_ALPHAPREMULTIPLY_ON)
        outBRDFData.diffuse *= alpha;
    #endif
}

inline void InitializeBRDFDataDirect(InputData inputData, inout SurfaceData surfaceData, half3 diffuse, half3 specular,
                                        half reflectivity, half oneMinusReflectivity, out BRDFData outBRDFData)
{
    outBRDFData = (BRDFData)0;
    outBRDFData.albedo = surfaceData.albedo;
    outBRDFData.diffuse = diffuse;
    outBRDFData.specular = specular;
    outBRDFData.reflectivity = reflectivity;
    #if defined (_MATERIAL_FEATURE_IRIDESCENCE)
        outBRDFData.specular = IridescenceSpecular(inputData.normalWS, inputData.viewDirectionWS, 
                                                    outBRDFData.specular, surfaceData.iridescenceTMS, 
                                                    surfaceData.clearCoatMask);
    #endif

    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surfaceData.smoothness);
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    outBRDFData.roughness2 = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.grazingTerm = saturate(surfaceData.smoothness + reflectivity);
    outBRDFData.normalizationTerm = outBRDFData.roughness * 4.0h + 2.0h;
    outBRDFData.roughness2MinusOne = outBRDFData.roughness2 - 1.0h;

    #if defined(_ALPHAPREMULTIPLY_ON)
        outBRDFData.diffuse *= surfaceData.alpha;
    #endif
}


inline void InitializeBRDFData(InputData inputData, inout SurfaceData surfaceData, out BRDFData outBRDFData)
{
    #ifdef _SPECULAR_SETUP
        half reflectivity = ReflectivitySpecular(surfaceData.specular);
        half oneMinusReflectivity = 1.0h - reflectivity;
        half3 brdfDiffuse = surfaceData.albedo * oneMinusReflectivity;
        half3 brdfSpecular = surfaceData.specular;
    #else
        half oneMinusReflectivity = OneMinusReflectivityMetallic(surfaceData.metallic);
        half reflectivity = 1.0h - oneMinusReflectivity;
        half3 brdfDiffuse = surfaceData.albedo * oneMinusReflectivity;
        half3 brdfSpecular = lerp(kDieletricSpec.rgb, surfaceData.albedo, surfaceData.metallic);
    #endif

    InitializeBRDFDataDirect(inputData, surfaceData, brdfDiffuse, brdfSpecular, reflectivity, oneMinusReflectivity, outBRDFData);
}

inline void InitializeSpecularBRDFData(InputData inputData, inout SurfaceData surfaceData, out BRDFData outBRDFData)
{
    half reflectivity = ReflectivitySpecular(surfaceData.specular);
    half oneMinusReflectivity = 1.0h - reflectivity;
    half3 brdfDiffuse = surfaceData.albedo * oneMinusReflectivity;
    half3 brdfSpecular = surfaceData.specular;

    InitializeBRDFDataDirect(inputData, surfaceData, brdfDiffuse, brdfSpecular, reflectivity, oneMinusReflectivity, outBRDFData);
}

inline void InitializeBRDFDataDirectSheen(half3 albedo, half3 diffuse, half3 specular, half smoothness, inout half alpha, out BRDFData outBRDFData)
{
    outBRDFData = (BRDFData)0;
    outBRDFData.albedo = albedo;
    outBRDFData.diffuse = diffuse;
    outBRDFData.specular = specular;
    outBRDFData.reflectivity = 1.0h;

    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(lerp(0.0h, 0.5h, smoothness));
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    outBRDFData.roughness2 = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.grazingTerm = 0.0h;
    outBRDFData.normalizationTerm   = outBRDFData.roughness * half(4.0) + half(2.0);
    outBRDFData.roughness2MinusOne  = outBRDFData.roughness2 - half(1.0);

#ifdef _ALPHAPREMULTIPLY_ON
    outBRDFData.diffuse *= alpha;
    alpha = alpha; // NOTE: alpha modified and propagated up.
#endif
}

inline void InitializeFabricBRDFData(SurfaceData surfaceData, inout half alpha, out BRDFData outBRDFData)
{
    #if defined (_MATERIAL_FEATURE_SHEEN)
        InitializeBRDFDataDirectSheen(surfaceData.albedo, surfaceData.albedo, surfaceData.specular, 
                                        surfaceData.smoothness, surfaceData.alpha, outBRDFData);
    #else
        half reflectivity = ReflectivitySpecular(surfaceData.specular);
        half oneMinusReflectivity = 1.0h - reflectivity;
        half3 brdfDiffuse = surfaceData.albedo * oneMinusReflectivity;
        InitializeBRDFDataDirect(surfaceData.albedo, brdfDiffuse, surfaceData.specular, reflectivity, 
                                    oneMinusReflectivity, surfaceData.smoothness, surfaceData.alpha, outBRDFData);
    #endif
}

#endif