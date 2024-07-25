#ifndef UNIVERSAL_LIGHT_FUNCTIONS_INCLUDED
#define UNIVERSAL_LIGHT_FUNCTIONS_INCLUDED

#include "ShaderLibrary/Radiance.hlsl"

/////////////////////////////////////
/***********Lighting Data***********/
/////////////////////////////////////

struct LightingData
{
    half3 giColor;
    half3 mainLightColor;
    half3 additionalLightsColor;
    half3 vertexLightingColor;
    half3 emissionColor;
};

LightingData CreateLightingData(InputData inputData, SurfaceData surfaceData)
{
    LightingData lightingData;

    lightingData.giColor = inputData.bakedGI;
    lightingData.emissionColor = surfaceData.emission;
    lightingData.vertexLightingColor = 0;
    lightingData.mainLightColor = 0;
    lightingData.additionalLightsColor = 0;

    return lightingData;
}

half3 CalculateLightingColor(LightingData lightingData, half3 albedo)
{
    half3 lightingColor = 0;

    if (IsOnlyAOLightingFeatureEnabled())
    {
        return lightingData.giColor; // Contains white + AO
    }

    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_GLOBAL_ILLUMINATION))
    {
        lightingColor += lightingData.giColor;
    }

    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_MAIN_LIGHT))
    {
        lightingColor += lightingData.mainLightColor;
    }

    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_ADDITIONAL_LIGHTS))
    {
        lightingColor += lightingData.additionalLightsColor;
    }

    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_VERTEX_LIGHTING))
    {
        lightingColor += lightingData.vertexLightingColor;
    }

    lightingColor *= albedo;

    if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_EMISSION))
    {
        lightingColor += lightingData.emissionColor;
    }

    return lightingColor;
}

half4 CalculateFinalColor(LightingData lightingData, half alpha)
{
    half3 finalColor = CalculateLightingColor(lightingData, 1);

    return half4(finalColor, alpha);
}

half4 CalculateFinalColor(LightingData lightingData, half3 albedo, half alpha, float fogCoord)
{
    #if defined(_FOG_FRAGMENT)
    #if (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))
        float viewZ = -fogCoord;
        float nearToFarZ = max(viewZ - _ProjectionParams.y, 0);
        half fogFactor = ComputeFogFactorZ0ToFar(nearToFarZ);
    #else
    half fogFactor = 0;
    #endif
    #else
    half fogFactor = fogCoord;
    #endif
    half3 lightingColor = CalculateLightingColor(lightingData, albedo);
    half3 finalColor = MixFog(lightingColor, fogFactor);

    return half4(finalColor, alpha);
}

/////////////////////////////////////
/***********BRDF Specular***********/
/////////////////////////////////////

half DirectBRDFSpecular(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
{
    float3 lightDirectionWSFloat3 = float3(lightDirectionWS);
    float3 halfDir = SafeNormalize(lightDirectionWSFloat3 + float3(viewDirectionWS));

    float NoH = saturate(dot(float3(normalWS), halfDir));
    half LoH = half(saturate(dot(lightDirectionWSFloat3, halfDir)));

    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

    half LoH2 = LoH * LoH;
    half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

    #if REAL_IS_HALF
    specularTerm = specularTerm - HALF_MIN;
    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
    #endif

    return specularTerm;
}

half ComplexDirectBRDFSpecular(BRDFData brdfData, VectorsData vData, half3 lightDirectionWS, half anisotropy)
{
    float3 lightDirectionWSFloat3 = float3(lightDirectionWS);
    float3 halfDir = SafeNormalize(lightDirectionWSFloat3 + float3(vData.viewDirectionWS));

    float NoH = saturate(dot(float3(vData.normalWS), halfDir));
    half LoH = half(saturate(dot(lightDirectionWSFloat3, halfDir)));

    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;
    half d2 = half(d * d);
    half LoH2 = LoH * LoH;
    half F = NoH * NoH * NoH;

    #ifdef _MATERIAL_FEATURE_ANISOTROPY
    half3 bitangentWS = vData.tangentWS.w * cross(vData.normalWS, vData.tangentWS.xyz);
    half3 H = SafeNormalize(lightDirectionWSFloat3 + vData.viewDirectionWS);
    half specularTerm = DV_Anisotropy(vData, brdfData.perceptualRoughness, anisotropy, lightDirectionWSFloat3) * F;
    #else
    half specularTerm = brdfData.roughness2 / (d2 * max(half(0.1), LoH2) * brdfData.normalizationTerm);
    #endif

    #if REAL_IS_HALF
    specularTerm = specularTerm - HALF_MIN;
    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
    #endif

    return specularTerm;
}

half3 SheenVD(half NoH, half smoothness)
{
    const half a = 0.22h;
    const half b = 0.585h;
    const half c = 0.75h;
    const half d = 1.25h;

    half f = pow(1.0h - NoH, c + d * smoothness);
    half sheen = saturate((a + b * smoothness) * f);

    return sheen * PI;
}

/*half3 SheenVD(half NoH, half smoothness)
{
    const half a = 0.22h;
    const half b = 0.585h;
    const half c = 0.75h;
    const half d = 1.25h;
    const half cTimesSmoothness = c + d * smoothness; // Precompute c + d * smoothness
    const half aTimesSmoothness = a + b * smoothness; // Precompute a + b * smoothness

    half f = exp2(-5.55473h * NoH * NoH); // Approximate pow(1.0 - NoH, cTimesSmoothness)
    half sheen = aTimesSmoothness * f; // Avoid the saturate operation

    return sheen * PI;
}*/

half3 CharlieDV(BRDFData brdfData, half3 viewDirWS, half3 lightDirWS, half3 normalWS)
{
    half LdotV, NdotH, LdotH, invLenLV;

    half NdotL = saturate(dot(normalWS, lightDirWS));
    half NdotV = saturate(dot(normalWS, viewDirWS));
    half clampedNdotV = ClampNdotV(NdotV);

    GetBSDFAngle(viewDirWS, lightDirWS, NdotL, NdotV, LdotV, NdotH, LdotH, invLenLV);
    
    half roughness = max(PerceptualRoughnessToRoughness(brdfData.perceptualRoughness), HALF_MIN_SQRT);

    half D = D_CharlieNoPI(NdotH, roughness);
    half V = V_Ashikhmin(NdotL, clampedNdotV);
    half3 F = F_Schlick(brdfData.specular, LdotH);

    half3 Fr = saturate(V * D) * F;
    
    return Fr;
}

half3 DirectFabricBRDFSpecular(BRDFData brdfData, VectorsData vData, half3 lightDirectionWS, half anisotropy)
{
    #ifdef _MATERIAL_FEATURE_SHEEN
        #ifdef _SHADER_QUALITY_SHEEN_PHYSICAL_BASED
            half3 specularFabric = CharlieDV(brdfData, vData.viewDirectionWS, lightDirectionWS, vData.normalWS);
        #else
            float3 halfDir = SafeNormalize(float3(lightDirectionWS) + float3(vData.viewDirectionWS));
            float NoH = saturate(dot(vData.normalWS, halfDir));
            half LoH = saturate(dot(lightDirectionWS, halfDir));
            half3 F = F_Schlick(brdfData.specular, LoH);

            half smoothness = PerceptualRoughnessToPerceptualSmoothness(brdfData.perceptualRoughness);
            half3 specularFabric = SheenVD(NoH, smoothness) * F;
        #endif
    #else
        half3 specularFabric = DV_Anisotropy(vData, brdfData.perceptualRoughness, anisotropy, lightDirectionWS);
    #endif

    return specularFabric;
}

half3 DirectBDRF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS,
                 bool specularHighlightsOff)
{
    // Can still do compile-time optimisation.
    // If no compile-time optimized, extra overhead if branch taken is around +2.5% on some untethered platforms, -10% if not taken.
    [branch] if (!specularHighlightsOff)
    {
        half specularTerm = DirectBRDFSpecular(brdfData, normalWS, lightDirectionWS, viewDirectionWS);
        half3 color = brdfData.diffuse + specularTerm * brdfData.specular;
        return color;
    }
    return brdfData.diffuse;
}

half3 DirectBRDF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
{
    #ifndef _SPECULARHIGHLIGHTS_OFF
    return brdfData.diffuse + DirectBRDFSpecular(brdfData, normalWS, lightDirectionWS, viewDirectionWS) * brdfData.specular;
    #else
    return brdfData.diffuse;
    #endif
}

half3 ComplexDirectBRDF(BRDFData brdfData, VectorsData vData, half3 lightDirectionWS, half anisotropy, bool specularHighlightsOff)
{
    // Can still do compile-time optimisation.
    // If no compile-time optimized, extra overhead if branch taken is around +2.5% on some untethered platforms, -10% if not taken.
    [branch] if (!specularHighlightsOff)
    {
        half specularTerm = ComplexDirectBRDFSpecular(brdfData, vData, lightDirectionWS, anisotropy);
        half3 color = brdfData.diffuse + specularTerm * brdfData.specular;
        return color;
    }
    return brdfData.diffuse;
}

half3 ComplexDirectBRDF(BRDFData brdfData, VectorsData vData, half3 lightDirectionWS, half anisotropy)
{
    #ifndef _SPECULARHIGHLIGHTS_OFF
    return brdfData.diffuse + ComplexDirectBRDFSpecular(brdfData, vData, lightDirectionWS, anisotropy) * brdfData.specular;
    #else
    return brdfData.diffuse;
    #endif
}

half3 ApplyClearCoatBRDF(SurfaceData surfaceData, BRDFData brdfDataCoat, VectorsData vData, half3 brdf, half3 lightDir)
{
    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        half3 clearCoatNormalWS = vData.normalWS;
        #if defined(_COATNORMALMAP)
            clearCoatNormalWS = vData.coatNormalWS;
        #endif
    
        half3 brdfCoat = kDielectricSpec.rrr * DirectBRDFSpecular(brdfDataCoat, clearCoatNormalWS, lightDir, vData.viewDirectionWS);
        half NoV = saturate(dot(clearCoatNormalWS, vData.viewDirectionWS));
    
        half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * Pow4(1.0 - NoV);
    
        return brdf * (1.0 - surfaceData.clearCoatMask * coatFresnel) + brdfCoat * surfaceData.clearCoatMask;
    #else
        return brdf;
    #endif // _CLEARCOAT
}

//////////////////////////////////////
/*********Lighting Functions*********/
//////////////////////////////////////

half3 LightingLambert(half3 lightColor, half3 lightDir, half3 normal)
{
    half NdotL = saturate(dot(normal, lightDir));
    return lightColor * NdotL;
}

half3 LightingSpecular(half3 lightColor, half3 lightDir, half3 normal, half3 viewDir, half4 specular, half smoothness)
{
    float3 halfVec = SafeNormalize(float3(lightDir) + float3(viewDir));
    half NdotH = half(saturate(dot(normal, halfVec)));
    half modifier = pow(NdotH, smoothness);
    half3 specularReflection = specular.rgb * modifier;
    return lightColor * specularReflection;
}

/////////////////////////////////////
/********Simple Lit Lighting********/
/////////////////////////////////////
half3 SimpleLitLighting(BRDFData brdfData, Light light, half3 normalWS, half3 viewDirectionWS, half alpha,
                        bool specularHighlightsOff)
{
    half3 radiance = ComputeRadiance(light, normalWS, alpha);
    half3 brdf = brdfData.diffuse;
    #ifndef _SPECULARHIGHLIGHTS_OFF
    [branch] if (!specularHighlightsOff)
    {
        brdf += brdfData.specular * DirectBRDFSpecular(brdfData, normalWS, light.direction, viewDirectionWS);
    }
    #endif // _SPECULARHIGHLIGHTS_OFF

    return brdf * radiance;
}

//////////////////////////////////////
/********Complex Lit Lighting********/
//////////////////////////////////////
half3 ComplexLitLighting(SurfaceData surfaceData, BRDFData brdfData, VectorsData vData, Light light,
                            bool specularHighlightsOff)
{
    half3 radiance = ComputeComplexRadiance(surfaceData, brdfData, vData, light);
    half3 brdf = brdfData.diffuse;
    #ifndef _SPECULARHIGHLIGHTS_OFF
    [branch] if (!specularHighlightsOff)
    {
        brdf += brdfData.specular * ComplexDirectBRDFSpecular(brdfData, vData, light.direction, surfaceData.anisotropy);
    }
    #endif // _SPECULARHIGHLIGHTS_OFF

    return brdf * radiance;
}

half3 ComplexLitLighting(SurfaceData surfaceData, BRDFData brdfData, BRDFData brdfDataCoat, VectorsData vData,
                            Light light, bool specularHighlightsOff)
{
    half3 radiance = ComputeComplexRadiance(surfaceData, brdfData, vData, light);
    half3 brdf = brdfData.diffuse;
    #ifndef _SPECULARHIGHLIGHTS_OFF
    [branch] if (!specularHighlightsOff)
    {
        brdf += brdfData.specular * ComplexDirectBRDFSpecular(brdfData, vData, light.direction, surfaceData.anisotropy);

        brdf = ApplyClearCoatBRDF(surfaceData, brdfDataCoat, vData, brdf, light.direction);
    }
    #endif // _SPECULARHIGHLIGHTS_OFF

    return brdf * radiance;
}

//////////////////////////////////////
/********Blinn Phong Lighting********/
//////////////////////////////////////

half3 CalculateBlinnPhong(Light light, InputData inputData, SurfaceData surfaceData)
{
    half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
    half3 lightDiffuseColor = LightingLambert(attenuatedLightColor, light.direction, inputData.normalWS);

    half3 lightSpecularColor = half3(0, 0, 0);
    #if defined(_SPECGLOSSMAP) || defined(_SPECULAR_COLOR)
    half smoothness = exp2(10 * surfaceData.smoothness + 1);

    lightSpecularColor += LightingSpecular(attenuatedLightColor, light.direction, inputData.normalWS, inputData.viewDirectionWS, half4(surfaceData.specular, 1), smoothness);
    #endif

    #if _ALPHAPREMULTIPLY_ON
    return lightDiffuseColor * surfaceData.albedo * surfaceData.alpha + lightSpecularColor;
    #else
    return lightDiffuseColor * surfaceData.albedo + lightSpecularColor;
    #endif
}

///////////////////////////////////////
/***********Vertex Lighting***********/
///////////////////////////////////////

half3 VertexLighting(float3 positionWS, half3 normalWS)
{
    half3 vertexLightColor = half3(0.0, 0.0, 0.0);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
    uint lightsCount = GetAdditionalLightsCount();
    LIGHT_LOOP_BEGIN(lightsCount)
        Light light = GetAdditionalLight(lightIndex, positionWS);
        half3 lightColor = light.color * light.distanceAttenuation;
        vertexLightColor += LightingLambert(lightColor, light.direction, normalWS);
    LIGHT_LOOP_END
    #endif

    return vertexLightColor;
}

#endif