#ifndef GLOBAL_ILLUMINATION_INCLUDED
#define GLOBAL_ILLUMINATION_INCLUDED

#include "ShaderLibrary/EnviromentBRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/GlobalIllumination.hlsl"
#include "ShaderLibrary/SpecularOcclusion.hlsl"

half3 AnisotropyReflectVector(VectorsData vData, inout half perceptualRoughness, half anisotropy)
{
    #ifdef _MATERIAL_FEATURE_SHEEN
        return reflect(-vData.viewDirectionWS, vData.normalWS);
    #else
        half3 modifiedNormalWS = vData.normalWS;
        real3 bitangentWS = vData.tangentWS.w * cross(vData.normalWS, vData.tangentWS.xyz);
        GetGGXAnisotropicModifiedNormalAndRoughness(bitangentWS, vData.tangentWS.xyz, vData.normalWS, 
                                                    vData.viewDirectionWS, anisotropy, perceptualRoughness, 
                                                    modifiedNormalWS, perceptualRoughness);
                                                    
        return reflect(-vData.viewDirectionWS, modifiedNormalWS);
    #endif
}

half3 FabricGI(SurfaceData surfaceData, BRDFData brdfData, VectorsData vData, float3 positionWS, 
                half3 bakedGI, float2 normalizedScreenSpaceUV, half occlusion)
{
    half3 reflectVector = AnisotropyReflectVector(vData, brdfData.perceptualRoughness, surfaceData.anisotropy);
    half NoV = saturate(dot(vData.normalWS, vData.viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = bakedGI;
    half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, positionWS, brdfData.perceptualRoughness, 1.0, normalizedScreenSpaceUV);

    half specularOcclusion = CalculateSpecularOcclusion(brdfData, surfaceData, vData, indirectSpecular, bakedGI, occlusion);
    indirectSpecular *= specularOcclusion;

    half3 color = FabricEnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);

    if (IsOnlyAOLightingFeatureEnabled())
    {
        color = half3(1, 1, 1); // "Base white" for AO debug lighting mode
    }

    return color * occlusion;
}

#if !USE_FORWARD_PLUS
half3 FabricGI(SurfaceData surfaceData, BRDFData brdfData, VectorsData vData,
                                float3 positionWS, half3 bakedGI, half occlusion)
{
    return FabricGI(surfaceData, brdfData, vData, positionWS, bakedGI, float2(0.0, 0.0), occlusion);
}
#endif

half3 FabricGI(SurfaceData surfaceData, BRDFData brdfData, VectorsData vData, half3 bakedGI, half occlusion)
{
    half3 reflectVector = AnisotropyReflectVector(vData, brdfData.perceptualRoughness, surfaceData.anisotropy);
    half NoV = saturate(dot(vData.normalWS, vData.viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = bakedGI;
    half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, 1.0);

    half specularOcclusion = CalculateSpecularOcclusion(brdfData, surfaceData, vData, indirectSpecular, bakedGI, occlusion);
    indirectSpecular *= specularOcclusion;

    half3 color = FabricEnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);

    if (IsOnlyAOLightingFeatureEnabled())
    {
        color = half3(1, 1, 1); // "Base white" for AO debug lighting mode
    }
    
    return color * occlusion;
}

#endif