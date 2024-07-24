#ifndef UNIVERSAL_DETAIL_MAPPING
#define UNIVERSAL_DETAIL_MAPPING

//real3 NormalScale(real3 normal, real Scale)
//{
//    return real3(normal.rg * Scale, lerp(1.0h, normal.b, Scale));
//}

//////////////////////////////////////
/*********URP Detail Mapping*********/
//////////////////////////////////////

/*real3 ScaleDetailAlbedo(real3 detailAlbedo, real scale)
{
    return real(2.0) * detailAlbedo * scale - scale + real(1.0);
}

real3 ApplyDetailAlbedo(real2 detailUv, TEXTURE2D_PARAM(detailAlbedoMap, sampler_detailAlbedoMap), real3 albedo,
                        real3 scale, real detailMask)
{
    #if defined(_DETAIL)
    real3 detailAlbedo = SAMPLE_TEXTURE2D(detailAlbedoMap, sampler_detailAlbedoMap, detailUv).rgb;

    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
    #if defined(_DETAIL_SCALED)
    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, scale);
    #else
    detailAlbedo = real(2.0) * detailAlbedo;
    #endif

    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
    #else
    return albedo;
    #endif
}

real3 ApplyDetailNormal(real2 detailUv, TEXTURE2D_PARAM(detailNormalMap, sampler_detailNormalMap), real3 normalTS,
                        real detailMask)
{
    #if defined(_DETAIL)
    #if BUMP_SCALE_NOT_SUPPORTED
    real3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(detailNormalMap, sampler_detailNormalMap, detailUv));
    #else
    real3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(detailNormalMap, sampler_detailNormalMap, detailUv), _DetailNormalMapScale);
    #endif

    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
    // For visual consistancy we going to do in all cases
    detailNormalTS = normalize(detailNormalTS);

    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); //TODO: detailMask should lerp the angle of the quaternion rotation, not the normals
    #else
    return normalTS;
    #endif
}*/

///////////////////////////////////////
/*********HDRP Detail Mapping*********/
///////////////////////////////////////

half3 DetailAlbedo(half3 baseColor, half detailBaseColor, half detailScale, half detailMask)
{
    half albedoDetailSpeed = saturate(detailBaseColor * detailScale);
    albedoDetailSpeed *= albedoDetailSpeed;
    half3 baseColorOverlay = lerp(sqrt(baseColor), 
                                (detailBaseColor < 0.0h) ? half3(0.0h, 0.0h, 0.0h) : half3(1.0h, 1.0h, 1.0h),
                                albedoDetailSpeed);
    baseColorOverlay *= baseColorOverlay;

    return lerp(baseColor, saturate(baseColorOverlay), detailMask);
}

half DetailSmoothness(half smoothness, half detailSmoothness, half detailSmoothnessScale, half detailMask)
{
    detailSmoothness = saturate(detailSmoothness * 2.0h - 1.0h);
    half smoothnessDetailSpeed = saturate(detailSmoothness * detailSmoothnessScale);
    half smoothnessOverlay = lerp(smoothness, detailSmoothness, smoothnessDetailSpeed);

    return lerp(smoothness, smoothnessOverlay, detailMask);
}

half3 DetailNormal(half3 normalTS, half3 normalDetail, half detailMask)
{
    return lerp(normalTS, BlendNormalRNM(normalTS, normalDetail), detailMask);
}

half3 ApplyDetailNormal(half3 normalTS, half2 normalAG, half normalScale, half detailMask)
{
    half3 detailNormalTS = UnpackNormalAG(half4(1.0h, normalAG.x, 1.0h, normalAG.y), normalScale);
    detailNormalTS = normalize(detailNormalTS);

    return DetailNormal(normalTS, detailNormalTS, detailMask);
}

// detailRemap:
// x - DetailAlbedoScale
// y - DetailNormalScale
// z - DetailSmoothnessScale
void ApplyHDRPDetailMapping(half4 detail, half3 detailRemap, half detailMask, 
                            inout half3 albedo, inout half3 normalTS, inout half smoothness)
{
    albedo = DetailAlbedo(albedo, detail.r, detailRemap.x, detailMask);
    normalTS = ApplyDetailNormal(normalTS, detail.ag, detailRemap.y, detailMask);
    smoothness = DetailSmoothness(smoothness, detail.b, detailRemap.z, detailMask);
}

#endif