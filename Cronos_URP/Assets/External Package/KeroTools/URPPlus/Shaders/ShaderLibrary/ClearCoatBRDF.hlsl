#ifndef UNIVERSAL_CLEARCOAT_BRDF_INCLUDED
#define UNIVERSAL_CLEARCOAT_BRDF_INCLUDED

#include "ShaderLibrary/BRDF.hlsl"

half3 ConvertF0ForClearCoat15(half3 f0)
{
    return ConvertF0ForAirInterfaceToF0ForClearCoat15Fast(f0);
}

inline void InitializeBRDFDataClearCoat(half clearCoatMask, half clearCoatSmoothness, inout BRDFData baseBRDFData,
                                        out BRDFData outBRDFData)
{
    outBRDFData = (BRDFData)0;
    outBRDFData.albedo = 1.0h;

    // Calculate Roughness of Clear Coat layer
    outBRDFData.diffuse = kDielectricSpec.aaa; // 1 - kDielectricSpec
    outBRDFData.specular = kDielectricSpec.rgb;
    outBRDFData.reflectivity = kDielectricSpec.r;

    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(clearCoatSmoothness);
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    outBRDFData.roughness2 = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.normalizationTerm = outBRDFData.roughness * 4.0h + 2.0h;
    outBRDFData.roughness2MinusOne = outBRDFData.roughness2 - 1.0h;
    outBRDFData.grazingTerm = saturate(clearCoatSmoothness + kDielectricSpec.x);

    // Modify Roughness of base layer using coat IOR
    half ieta = lerp(1.0h, CLEAR_COAT_IETA, clearCoatMask);
    half coatRoughnessScale = Sq(ieta);
    half sigma = RoughnessToVariance(PerceptualRoughnessToRoughness(baseBRDFData.perceptualRoughness));

    baseBRDFData.perceptualRoughness = RoughnessToPerceptualRoughness(VarianceToRoughness(sigma * coatRoughnessScale));

    // Recompute base material for new roughness, previous computation should be eliminated by the compiler (as it's unused)
    baseBRDFData.roughness = max(PerceptualRoughnessToRoughness(baseBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    baseBRDFData.roughness2 = max(baseBRDFData.roughness * baseBRDFData.roughness, HALF_MIN);
    baseBRDFData.normalizationTerm = baseBRDFData.roughness * 4.0h + 2.0h;
    baseBRDFData.roughness2MinusOne = baseBRDFData.roughness2 - 1.0h;

    // Darken/saturate base layer using coat to surface reflectance (vs. air to surface)
    baseBRDFData.specular = lerp(baseBRDFData.specular, ConvertF0ForClearCoat15(baseBRDFData.specular), clearCoatMask);
    // TODO: what about diffuse? at least in specular workflow diffuse should be recalculated as it directly depends on it.
    // TODO:
    //baseBRDFData.diffuse = 1.0h - baseBRDFData.specular; 
    //half clearCoatDiffuse = 1.0h - baseBRDFData.specular; // Calculate the clear coat diffuse component

    // Apply clear coat mask to blend between base diffuse and clear coat diffuse
    // baseBRDFData.diffuse = lerp(baseBRDFData.diffuse, clearCoatDiffuse, clearCoatMask);
}

BRDFData CreateClearCoatBRDFData(SurfaceData surfaceData, inout BRDFData brdfData)
{
    BRDFData brdfDataClearCoat = (BRDFData)0;

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
    // base brdfData is modified here, rely on the compiler to eliminate dead computation by InitializeBRDFData()
    InitializeBRDFDataClearCoat(surfaceData.clearCoatMask, surfaceData.clearCoatSmoothness, brdfData, brdfDataClearCoat);
    #endif

    return brdfDataClearCoat;
}

#endif