#ifndef UNIVERSAL_SPECULAR_OCCLUSION_INCLUDED
#define UNIVERSAL_SPECULAR_OCCLUSION_INCLUDED

#if defined(_BENTNORMALMAP) || (_BENTNORMALMAP1) || (_BENTNORMALMAP2) || (_BENTNORMALMAP3)
    #define _BENTNORMAL
#endif

half SpecularOcclusionFromBentAO(half3 V, half3 normalWS, half3 bentNormalWS, half ambientOcclusion, half roughness)
{
    float vs = -1.0f / /*min(*/(sqrt(1.0f - ambientOcclusion) - 1.0f)/*, -0.001f)*/;
    float us = 0.8f;

    float NoV = dot(V, normalWS);
    float3 NDFAxis = (2 * NoV * normalWS - V) * (0.5f / max(roughness * roughness * NoV, 0.001f));

    float umLength1 = length(NDFAxis + vs * bentNormalWS);
    float umLength2 = length(NDFAxis + us * normalWS);
    float d1 = 1 - exp(-2 * umLength1);
    float d2 = 1 - exp(-2 * umLength2);
    float expFactor1 = exp(umLength1 - umLength2 + us - vs);

    return saturate(expFactor1 * (d1 * umLength2) / (d2 * umLength1));
}

half GetLuminance(half3 colorLinear)
{
    #if _TONEMAP_ACES
        return AcesLuminance(colorLinear);
    #else
        return Luminance(colorLinear);
    #endif
}

half SpecularOcclusionFromGI(half3 indirectSpecular, half3 bakedGI, half giOcclusionBias)
{
    half exposure = PI + GetLuminance(indirectSpecular);

    return saturate((giOcclusionBias + GetLuminance(bakedGI)) * exposure);
}

half CalculateSpecularOcclusion(BRDFData brdfData, SurfaceData surfaceData, VectorsData vData, 
                                half3 indirectSpecular, half3 bakedGI, half occlusion)
{
    half specularOcclusion = 1.0;

    #if defined(_AO_SPECULAR_OCCLUSION)
        half NdotV = saturate(dot(vData.normalWS, vData.viewDirectionWS));
        specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(NdotV, occlusion, brdfData.roughness);
    #elif defined(_BENTNORMAL) && (_BENTNORMAL_SPECULAR_OCCLUSION)
        specularOcclusion = SpecularOcclusionFromBentAO(vData.viewDirectionWS, vData.normalWS, vData.bentNormalWS, occlusion, brdfData.roughness);
    #elif defined(_GI_SPECULAR_OCCLUSION)
        specularOcclusion = SpecularOcclusionFromGI(indirectSpecular, bakedGI, surfaceData.giOcclusionBias);
    #endif

    #ifdef _HORIZON_SPECULAR_OCCLUSION
        specularOcclusion *= GetHorizonOcclusion(vData.viewDirectionWS, vData.normalWS, vData.geomNormalWS, surfaceData.horizonFade);
    #endif

    return specularOcclusion;
}

#endif