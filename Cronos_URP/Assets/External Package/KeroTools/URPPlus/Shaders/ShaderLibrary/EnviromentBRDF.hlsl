#ifndef UNIVERSAL_ENVIROMENT_BRDF_INCLUDED
#define UNIVERSAL_ENVIROMENT_BRDF_INCLUDED

#include "ShaderLibrary/BRDF.hlsl"

half3 Pow5(half3 x)
{
    return x * x * x * x * x;
}

float SurfaceReduction(half roughness2)
{
    return 1.0 / (roughness2 + 1.0);
}

half3 EnvironmentBRDFSpecular(BRDFData brdfData, half fresnelTerm)
{
    float surfaceReduction = SurfaceReduction(brdfData.roughness2);
    return half3(surfaceReduction * lerp(brdfData.specular, brdfData.grazingTerm, fresnelTerm));
}

half3 EnvironmentBRDFIridescenceSpecular(BRDFData brdfData, half3 fresnelIridescent)
{
    float surfaceReduction = SurfaceReduction(brdfData.roughness2);
    return half3(surfaceReduction * lerp(brdfData.specular * Pow5(fresnelIridescent), brdfData.grazingTerm,
                                         fresnelIridescent));
}

half3 EnvironmentBRDFSheen(BRDFData brdfData, half fresnelTerm)
{
    float surfaceReduction = SurfaceReduction(brdfData.roughness2);
    return surfaceReduction * lerp(0.0, brdfData.grazingTerm, fresnelTerm);
}

half3 EnvironmentBRDF(BRDFData brdfData, half3 indirectDiffuse, half3 indirectSpecular, half fresnelTerm)
{
    half3 c = indirectDiffuse * brdfData.diffuse;
    c += indirectSpecular * EnvironmentBRDFSpecular(brdfData, fresnelTerm);
    return c;
}

half3 EnvironmentBRDFIridescence(BRDFData brdfData, half3 indirectDiffuse, half3 indirectSpecular, half3 fresnelIridescent)
{
    half3 c = indirectDiffuse * brdfData.diffuse;
    c += indirectSpecular * EnvironmentBRDFIridescenceSpecular(brdfData, fresnelIridescent);
    return c;
}

half3 FabricEnvironmentBRDF(BRDFData brdfData, half3 indirectDiffuse, half3 indirectSpecular, half fresnelTerm)
{
    half3 c = indirectDiffuse * brdfData.diffuse;

    #ifdef _MATERIAL_FEATURE_SHEEN
        half3 indirectColor = EnvironmentBRDFSheen(brdfData, fresnelTerm);
    #else
        half3 indirectColor = EnvironmentBRDFSpecular(brdfData, fresnelTerm);
    #endif

    c += indirectSpecular * indirectColor;

    return c;
}

// Environment BRDF without diffuse for clear coat
half3 EnvironmentBRDFClearCoat(BRDFData brdfData, half clearCoatMask, half3 indirectSpecular, half fresnelTerm)
{
    return indirectSpecular * EnvironmentBRDFSpecular(brdfData, fresnelTerm) * clearCoatMask;
}

#endif