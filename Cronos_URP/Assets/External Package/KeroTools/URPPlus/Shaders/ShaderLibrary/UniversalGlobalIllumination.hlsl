#ifndef GLOBAL_ILLUMINATION_INCLUDED
#define GLOBAL_ILLUMINATION_INCLUDED

#include "ShaderLibrary/EnviromentBRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/GlobalIllumination.hlsl"
#include "ShaderLibrary/SpecularOcclusion.hlsl"

half3 ComputeReflectVector(VectorsData vData, half anisotropy, inout half perceptualRoughness) 
{
    #ifdef _MATERIAL_FEATURE_ANISOTROPY
        real3 bitangentWS = vData.tangentWS.w * cross(vData.normalWS, vData.tangentWS.xyz);
        half3 modifiedNormalWS;
        GetGGXAnisotropicModifiedNormalAndRoughness(bitangentWS, vData.tangentWS.xyz, vData.normalWS,
                                                     vData.viewDirectionWS, anisotropy, perceptualRoughness,
                                                     modifiedNormalWS, perceptualRoughness);
        return reflect(-vData.viewDirectionWS, modifiedNormalWS);
    #else
        return reflect(-vData.viewDirectionWS, vData.normalWS);
    #endif
}

half3 ComputeCoatReflectVector(half3 reflectVector, half3 viewDirectionWS, half3 normalWS, half3 coatNormalWS) 
{
    half3 coatReflectVector = reflectVector;

    #if defined (_MATERIAL_FEATURE_ANISOTROPY)
        coatReflectVector = reflect(-viewDirectionWS, normalWS);
    #elif defined (_COATNORMALMAP)
        coatReflectVector = reflect(-viewDirectionWS, coatNormalWS);
    #endif

    return coatReflectVector;
}

void ComputeIndirectSpecular(VectorsData vData, float3 positionWS, float2 normalizedScreenSpaceUV, 
                                half perceptualRoughness, half coatPerceptualRoughness, half anisotropy, 
                                out half3 indirectSpecular, out half3 coatIndirectSpecular) 
{
#if !USE_FORWARD_PLUS
    normalizedScreenSpaceUV = float2(0.0, 0.0);
#endif

    half3 reflectVector = ComputeReflectVector(vData, anisotropy, perceptualRoughness);
    indirectSpecular = GlossyEnvironmentReflection(reflectVector, positionWS, perceptualRoughness, 1.0, normalizedScreenSpaceUV);
    coatIndirectSpecular = 0.0f;

#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
    half3 coatReflectVector =  ComputeCoatReflectVector(reflectVector, vData.viewDirectionWS, vData.normalWS, vData.coatNormalWS);
    coatIndirectSpecular = GlossyEnvironmentReflection(coatReflectVector, positionWS, coatPerceptualRoughness, 1.0f, normalizedScreenSpaceUV);
#endif
}

half3 ComplexGlobalIllumination(SurfaceData surfaceData, BRDFData brdfData, BRDFData brdfDataCoat, VectorsData vData,
                                half3 bakedGI, half3 indirectSpecular, half3 coatIndirectSpecular, half occlusion)
{
    half3 reflectVector = ComputeReflectVector(vData, surfaceData.anisotropy, brdfData.perceptualRoughness);
    half NoV = saturate(dot(vData.normalWS, vData.viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = bakedGI;
    half specularOcclusion = CalculateSpecularOcclusion(brdfData, surfaceData, vData, indirectSpecular, bakedGI, occlusion);
    indirectSpecular *= specularOcclusion;

    #if !defined(_MATERIAL_FEATURE_IRIDESCENCE)
        half3 color = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
    #else
        half3 color = EnvironmentBRDFIridescence(brdfData, indirectDiffuse, indirectSpecular, brdfData.specular);
    #endif

    if (IsOnlyAOLightingFeatureEnabled())
    {
        color = half3(1, 1, 1); // "Base white" for AO debug lighting mode
    }

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        // TODO: "grazing term" causes problems on full roughness
        half3 coatColor = EnvironmentBRDFClearCoat(brdfDataCoat, surfaceData.clearCoatMask, coatIndirectSpecular, fresnelTerm);
        half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * fresnelTerm;

        return (color * (1.0 - coatFresnel * surfaceData.clearCoatMask) + coatColor) * occlusion;
    #else
        return color * occlusion;
    #endif
}

// Backwards compatiblity
half3 ComplexGlobalIllumination(SurfaceData surfaceData, BRDFData brdfData, VectorsData vData, half3 bakedGI, 
                                    half3 indirectSpecular, half3 coatIndirectSpecular, half occlusion)
{
    const BRDFData noClearCoat = (BRDFData)0;

    return ComplexGlobalIllumination(surfaceData, brdfData, noClearCoat, vData, bakedGI, indirectSpecular, 
                                        coatIndirectSpecular, occlusion);
}

#endif