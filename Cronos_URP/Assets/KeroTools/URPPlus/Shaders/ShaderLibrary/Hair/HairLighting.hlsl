#ifndef UNIVERSAL_LIGHTING_INCLUDED
#define UNIVERSAL_LIGHTING_INCLUDED

#include "ShaderLibrary/VectorsData.hlsl"
#include "ShaderLibrary/BRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
#include "ShaderLibrary/UniversalGlobalIllumination.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/BSDF.hlsl"
#include "ShaderLibrary/Radiance.hlsl"
#include "ShaderLibrary/LightingFunctions.hlsl"

#if !defined(LIGHTMAP_ON)
// TODO: Controls things like these by exposing SHADER_QUALITY levels (low, medium, high)
#if !defined(_NORMALMAP)
        // Evaluates SH fully in vertex
        #define EVALUATE_SH_VERTEX
#elif !SHADER_HINT_NICE_QUALITY
// Evaluates L2 SH in vertex and L0L1 in pixel
#define EVALUATE_SH_MIXED
#endif
// Otherwise evaluate SH fully per-pixel
#endif

#if defined(LIGHTMAP_ON)
    #define DECLARE_LIGHTMAP_OR_SH(lmName, shName, index) float2 lmName : TEXCOORD##index
    #define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT) OUT.xy = lightmapUV.xy * lightmapScaleOffset.xy + lightmapScaleOffset.zw;
    #define OUTPUT_SH(normalWS, OUT)
#else
    #define DECLARE_LIGHTMAP_OR_SH(lmName, shName, index) half3 shName : TEXCOORD##index
    #define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT)
    #define OUTPUT_SH(normalWS, OUT) OUT.xyz = SampleSHVertex(normalWS)
#endif

half RoughnessToBlinnPhongSpecularExponent(half roughness)
{
    return clamp(2 * rcp(roughness * roughness) - 2, FLT_EPS, rcp(FLT_EPS));
}

void HairSpecularWithSingleRoughness(BRDFData brdfData, HairData hairData, VectorsData vData, half3 lightDirectionWS,
                                        half NdotL, out half3 specR, out half3 specT)
{
    half NdotV = saturate(dot(vData.geomNormalWS, vData.viewDirectionWS));

    half LdotV, NdotH, LdotH, invLenLV;
    GetBSDFAngle(vData.viewDirectionWS, lightDirectionWS, NdotL, NdotV, LdotV, NdotH, LdotH, invLenLV);

    half perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(hairData.perceptualSmoothness);
    half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
    half specularExponent = RoughnessToBlinnPhongSpecularExponent(roughness);
    half3 t1 = ShiftTangent(vData.bitangentWS, vData.normalWS, hairData.specularShift);
    half3 t2 = ShiftTangent(vData.bitangentWS, vData.normalWS, hairData.secondarySpecularShift);

    half3 H = (lightDirectionWS + vData.viewDirectionWS) * invLenLV;

    half3 hairSpec1 = hairData.specularTint * D_KajiyaKay(t1, H, specularExponent);
    half3 hairSpec2 = hairData.secondarySpecularTint * D_KajiyaKay(t2, H, specularExponent);
    half f0 = 0.5 + (perceptualRoughness + perceptualRoughness * LdotV);
    half3 F = F_Schlick(f0, LdotH) * INV_PI;
    
    half scatterFresnel1 = pow(saturate(-LdotV), 9.0) * pow(saturate(1.0 - NdotV * NdotV), 12.0) * hairData.transmissionIntensity;
    half scatterFresnel2 = saturate(PositivePow((1.0 - NdotV), 20.0));
    
    specR = 0.25h * F * (hairSpec1 + hairSpec2) * NdotL * saturate(NdotV * FLT_MAX);
    specT = hairData.transmissionColor * (scatterFresnel1 + hairData.transmissionIntensity * scatterFresnel2);
}

half3 HairLighting(BRDFData brdfData, HairData hairData, VectorsData vData, Light light, bool specularHighlightsOff)
{
    half3 lightAttenuation = light.color * light.distanceAttenuation * light.shadowAttenuation;
    half NdotL = saturate(dot(vData.normalWS, light.direction));
    half3 radiance = NdotL * lightAttenuation;

    half3 brdf = brdfData.diffuse * radiance;
    half3 specularR, specularT;
#ifndef _SPECULARHIGHLIGHTS_OFF
    [branch] if (!specularHighlightsOff)
    {
        HairSpecularWithSingleRoughness(brdfData, hairData, vData, light.direction, NdotL, specularR, specularT);
        brdf += saturate(specularR + specularT) * lightAttenuation;
    }
#endif // _SPECULARHIGHLIGHTS_OFF
    return brdf;
}

///////////////////////////////////////////////////////////////////////////////
//                      Fragment Functions                                   //
///////////////////////////////////////////////////////////////////////////////

half4 HairFragment(InputData inputData, SurfaceData surfaceData, VectorsData vData, HairData hairData)
{
    bool specularHighlightsOff = false;
    #if defined(_SPECULARHIGHLIGHTS_OFF)
        specularHighlightsOff = true;
    #endif

    BRDFData brdfData;

    // NOTE: can modify "surfaceData"...
    InitializeSpecularBRDFData(inputData, surfaceData, brdfData);

    #if defined(DEBUG_DISPLAY)
    half4 debugColor;

    if (CanDebugOverrideOutputColor(inputData, surfaceData, brdfData, debugColor))
    {
        return debugColor;
    }
    #endif
    
    half4 shadowMask = CalculateShadowMask(inputData);
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    uint meshRenderingLayers = GetMeshRenderingLayer();
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

    // NOTE: We don't apply AO to the GI here because it's done in the lighting calculation below...
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
    LightingData lightingData = CreateLightingData(inputData, surfaceData);
    
    half3 indirectSpecular, coatIndirectSpecular;
    ComputeIndirectSpecular(vData, inputData.positionWS, inputData.normalizedScreenSpaceUV, 
                            brdfData.perceptualRoughness, 0.0, surfaceData.anisotropy, 
                            indirectSpecular, coatIndirectSpecular);

    lightingData.giColor = ComplexGlobalIllumination(surfaceData, brdfData, vData, inputData.bakedGI, indirectSpecular,
                                                        coatIndirectSpecular, aoFactor.indirectAmbientOcclusion);

    #ifdef _LIGHT_LAYERS
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
    #endif
    {
        lightingData.mainLightColor = HairLighting(brdfData, hairData, vData, mainLight, specularHighlightsOff);
    }

    #ifdef _STATICSPECULARHIGHLIGHT_ON
        half3 staticLightVector = _StaticLightVector.xyz;
        half3 staticAttenuation = _StaticLightColor.rgb * _StaticLightVector.w;
        half staticNdotL = saturate(dot(vData.normalWS, staticLightVector));

        half3 staticSpecularR, staticSpecularT;
        HairSpecularWithSingleRoughness(brdfData, hairData, vData, staticLightVector, staticNdotL, staticSpecularR, staticSpecularT);

        lightingData.mainLightColor += saturate(staticSpecularR + staticSpecularT) * staticAttenuation;
    #endif

    #if defined(_ADDITIONAL_LIGHTS)
    uint pixelLightCount = GetAdditionalLightsCount();

    #if USE_FORWARD_PLUS
    for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

    #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
    #endif
        {
            lightingData.additionalLightsColor += HairLighting(brdfData, hairData, vData, light, specularHighlightsOff);
        }
    }
    #endif

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

    #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
    #endif
        {
            lightingData.additionalLightsColor += HairLighting(brdfData, hairData, vData, light, specularHighlightsOff);
        }
    LIGHT_LOOP_END
    #endif

    #if defined(_ADDITIONAL_LIGHTS_VERTEX)
    lightingData.vertexLightingColor += inputData.vertexLighting * brdfData.diffuse;
    #endif

    return CalculateFinalColor(lightingData, surfaceData.alpha);
}

#endif