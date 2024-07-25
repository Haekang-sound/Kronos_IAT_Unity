#ifndef UNIVERSAL_BSDF_INCLUDED
#define UNIVERSAL_BSDF_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

#define CLEAR_COAT_IOR 1.5

half _MicroShadowOpacity;

void GeometricAAFiltering(float3 geometricNormalWS, float screenSpaceVariance, float threshold, 
                            inout float perceptualSmoothness)
{
    float variance = GeometricNormalVariance(geometricNormalWS, screenSpaceVariance);
    perceptualSmoothness = NormalFiltering(perceptualSmoothness, variance, threshold);
}

void GeometricAAFiltering(float3 geometricNormalWS, float screenSpaceVariance, float threshold, 
                            inout float perceptualSmoothness, inout float coatPerceptualSmoothness)
{
    float variance = GeometricNormalVariance(geometricNormalWS, screenSpaceVariance);
    perceptualSmoothness = NormalFiltering(perceptualSmoothness, variance, threshold);

    #if defined(_SHADER_QUALITY_COAT_GEOMETRIC_AA) && (_CLEARCOAT)
        coatPerceptualSmoothness = NormalFiltering(coatPerceptualSmoothness, variance, threshold);
    #endif
}

#ifdef _MATERIAL_FEATURE_IRIDESCENCE
TEXTURE2D(_IridescenceLUT);             SAMPLER(sampler_IridescenceLUT);

half3 CalculateIridescence(half eta_1, half cosTheta1, half2 iridescenceTS, half3 specular)
{
#if defined(_SHADER_QUALITY_IRIDESCENCE_APPROXIMATION)
    half eta_2 = 2.0 - iridescenceTS.x;
    half sinTheta2Sq = Sq(eta_1 / eta_2) * (1.0 - Sq(cosTheta1));
    half cosTheta2 = sqrt(1.0 - sinTheta2Sq);
    half k = iridescenceTS.y + (cosTheta2 * (PI * iridescenceTS.x));
    half R0 = IorToFresnel0(eta_2, eta_1);
    half R12 = saturate(sqrt(F_Schlick(R0, cosTheta1)));

    half3 iridescenceColor = lerp(2.0 * SAMPLE_TEXTURE2D(_IridescenceLUT, sampler_IridescenceLUT, float2(k * 1.5, 1.0)).rgb - 0.2, R12, R12); //just because :)

    return lerp(specular, iridescenceColor, R12);
#else
    return EvalIridescence(eta_1, cosTheta1, iridescenceTS.x, specular);
#endif
}

half3 IridescenceSpecular(half3 normalWS, half3 viewDirectionWS, half3 specular, half3 iridescenceTMS, half clearCoatMask)
{
    half NdotV = dot(normalWS, viewDirectionWS);
    half clampedNdotV = ClampNdotV(NdotV);
    half viewAngle = clampedNdotV;
    half topIor = 1.0;

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        topIor = lerp(1.0, CLEAR_COAT_IOR, clearCoatMask);
        viewAngle = sqrt(1.0 + Sq(1.0 / topIor) * (Sq(dot(normalWS, viewDirectionWS)) - 1.0));
    #endif

    if (iridescenceTMS.y > 0.0)
    {
        specular = lerp(specular, CalculateIridescence(topIor, viewAngle, iridescenceTMS.xz, specular), iridescenceTMS.y);
    }

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        specular = lerp(specular, ConvertF0ForAirInterfaceToF0ForClearCoat15Fast(specular), clearCoatMask);
    #endif
    
    return specular;
}
#endif

half DV_Anisotropy(VectorsData vData, half perceptualRoughness, half anisotropy, half3 lightDirWS)
{
    float3 H = SafeNormalize(float3(lightDirWS) + float3(vData.viewDirectionWS));
    //half3 bitangentWS = tangentWS.w * cross(normalWS, tangentWS.xyz);
    half3 bitangentWS = cross(vData.normalWS, vData.tangentWS.xyz);

    half NdotL = dot(vData.normalWS, lightDirWS);
    half clampedNdotV = ClampNdotV(dot(vData.normalWS, vData.viewDirectionWS));
    half NdotH = saturate(dot(vData.normalWS, H));
    
    half roughnessT;
    half roughnessB;

    ConvertAnisotropyToRoughness(perceptualRoughness, anisotropy, roughnessT, roughnessB);

    half TdotH = dot(vData.tangentWS.xyz, H);
    half TdotL = dot(vData.tangentWS.xyz, lightDirWS);
    half TdotV = dot(vData.tangentWS.xyz, vData.viewDirectionWS);
    half BdotH = dot(bitangentWS, H);
    half BdotL = dot(bitangentWS, lightDirWS);
    half BdotV = dot(bitangentWS, vData.viewDirectionWS);

    roughnessT = max(0.01, roughnessT);
    roughnessB = max(0.01, roughnessB);

    half partLambdaV = GetSmithJointGGXAnisoPartLambdaV(TdotV, BdotV, clampedNdotV, roughnessT, roughnessB);
    half DV = DV_SmithJointGGXAniso(TdotH, BdotH, NdotH, clampedNdotV, TdotL, BdotL, abs(NdotL), roughnessT, roughnessB,
                                    partLambdaV);

    return max(0, DV * PI * saturate(NdotL));
}

#endif