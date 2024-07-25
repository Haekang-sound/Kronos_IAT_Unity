#ifndef UNIVERSAL_LAYERED_DISPLACEMENT_PASS_INCLUDED
#define UNIVERSAL_LAYERED_DISPLACEMENT_PASS_INCLUDED

#include "ShaderLibrary/LayeredLit/LayeredSurfaceInput.hlsl"

#if defined(_PIXEL_DISPLACEMENT) && defined(_DEPTHOFFSET_ON)
    #define _DEPTHOFFSET
#endif

float3 GetViewDirectionTangentSpace(float4 tangentWS, half3 normalWS, half3 viewDirWS)
{
    half3 unnormalizedNormalWS = normalWS;
    const half renormFactor = 1.0 / length(unnormalizedNormalWS);

    float crossSign = (tangentWS.w > 0.0 ? 1.0 : -1.0);
    half3 bitang = crossSign * cross(normalWS.xyz, tangentWS.xyz);

    half3 WorldSpaceNormal = renormFactor * normalWS.xyz;
    half3 WorldSpaceTangent = renormFactor * tangentWS.xyz;
    half3 WorldSpaceBiTangent = renormFactor * bitang;

    half3x3 tangentSpaceTransform = half3x3(WorldSpaceTangent, WorldSpaceBiTangent, WorldSpaceNormal);
    half3 viewDirTS = mul(tangentSpaceTransform, viewDirWS);

    return viewDirTS;
}

real3 GetDisplacementObjectScale(bool vertexDisplacement)
{
    real3 objectScale = real3(1.0, 1.0, 1.0);

    real4x4 worldTransform;
    if (vertexDisplacement)
    {
        worldTransform = GetObjectToWorldMatrix();
    }

    else
    {
        worldTransform = GetWorldToObjectMatrix();
    }

    objectScale.x = length(real3(worldTransform._m00, worldTransform._m01, worldTransform._m02));
#if !defined(_PIXEL_DISPLACEMENT) || (defined(_PIXEL_DISPLACEMENT_LOCK_OBJECT_SCALE))
    objectScale.y = length(real3(worldTransform._m10, worldTransform._m11, worldTransform._m12));
#endif
    objectScale.z = length(real3(worldTransform._m20, worldTransform._m21, worldTransform._m22));

    return objectScale;
}

half GetMaxDisplacement()
{
    half maxDisplacement = 0.0;

    // _HeightAmplitudeX can be negative if min and max are inverted, but the max displacement must be positive, take abs()
    #if defined(_HEIGHTMAP)
        maxDisplacement = abs(_HeightAmplitude);
    #endif

    #if defined(_HEIGHTMAP1)
        maxDisplacement = max(  abs(_HeightAmplitude1)
                                #if defined(_MAIN_LAYER_INFLUENCE_MODE)
                                + abs(_HeightAmplitude) * _InheritBaseHeight1
                                #endif
                                , maxDisplacement);
    #endif

    #if _LAYER_COUNT >= 3
    #if defined(_HEIGHTMAP2)
        maxDisplacement = max(  abs(_HeightAmplitude2)
                                #if defined(_MAIN_LAYER_INFLUENCE_MODE)
                                + abs(_HeightAmplitude) * _InheritBaseHeight2
                                #endif
                                , maxDisplacement);
    #endif
    #endif

    #if _LAYER_COUNT >= 4
    #if defined(_HEIGHTMAP3)
        maxDisplacement = max(  abs(_HeightAmplitude3)
                                #if defined(_MAIN_LAYER_INFLUENCE_MODE)
                                + abs(_HeightAmplitude) * _InheritBaseHeight3
                                #endif
                                , maxDisplacement);
    #endif
    #endif

    return maxDisplacement;
}

float2 GetMinUvSize(LayerTexCoord layerTexCoord)
{
    float2 minUvSize = float2(FLT_MAX, FLT_MAX);

    #if defined(_HEIGHTMAP)
        minUvSize = min(layerTexCoord.baseUV0 * _HeightMap_TexelSize.zw, minUvSize);
    #endif

    #if defined(_HEIGHTMAP1)
        minUvSize = min(layerTexCoord.baseUV1 * _HeightMap1_TexelSize.zw, minUvSize);
    #endif

    #if _LAYER_COUNT >= 3
    #if defined(_HEIGHTMAP2)
        minUvSize = min(layerTexCoord.baseUV2 * _HeightMap2_TexelSize.zw, minUvSize);
    #endif
    #endif

    #if _LAYER_COUNT >= 4
    #if defined(_HEIGHTMAP3)
        minUvSize = min(layerTexCoord.baseUV3 * _HeightMap3_TexelSize.zw, minUvSize);
    #endif
    #endif

    return minUvSize;
}

void ApplyDisplacementTileScale(inout half height0, inout half height1, inout half height2, inout half height3)
{
    // When we change the tiling, we have want to conserve the ratio with the displacement (and this is consistent with per pixel displacement)
#ifdef _DISPLACEMENT_LOCK_TILING_SCALE
    half tileObjectScale = 1.0;
    #ifdef _LAYER_TILING_COUPLED_WITH_UNIFORM_OBJECT_SCALE
    // Extract scaling from world transform
    float4x4 worldTransform = GetObjectToWorldMatrix();
    // assuming uniform scaling, take only the first column
    tileObjectScale = length(float3(worldTransform._m00, worldTransform._m01, worldTransform._m02));
    #endif

    // TODO: precompute all these scaling factors!
    height0 *= _InvTilingScale;
    #if !defined(_MAIN_LAYER_INFLUENCE_MODE)
    height0 /= tileObjectScale;  // We only affect layer0 in case we are not in influence mode (i.e we should not change the base object)
    #endif
    height1 = (height1 / tileObjectScale) * _InvTilingScale1;
    height2 = (height2 / tileObjectScale) * _InvTilingScale2;
    height3 = (height3 / tileObjectScale) * _InvTilingScale3;
#endif
}

#if defined(_PIXEL_DISPLACEMENT) && LAYERS_HEIGHTMAP_ENABLE
struct PerPixelHeightDisplacementParam
{
    half4 blendMasks;
    float2 uv[4];
    //float2 uvSpaceScale[4];
#if defined(_MAIN_LAYER_INFLUENCE_MODE) && defined(_HEIGHTMAP)
    half heightInfluence[4];
#endif
};

half ComputePerPixelHeightDisplacement(float2 texOffsetCurrent, half lod, half4 blendMasks, LayerTexCoord layerTexCoord)
{
    // See function ComputePerVertexDisplacement() for comment about the weights/influenceMask/BlendMask

    // Note: Amplitude is handled in uvSpaceScale, no need to multiply by it here.
    half height0 = SAMPLE_TEXTURE2D_LOD(_HeightMap, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0 + texOffsetCurrent * layerTexCoord.uvSpaceScale[0] , lod).r;
    half height1 = SAMPLE_TEXTURE2D_LOD(_HeightMap1, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV1 + texOffsetCurrent * layerTexCoord.uvSpaceScale[1], lod).r;
    half height2 = SAMPLE_TEXTURE2D_LOD(_HeightMap2, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV2 + texOffsetCurrent * layerTexCoord.uvSpaceScale[2], lod).r;
    half height3 = SAMPLE_TEXTURE2D_LOD(_HeightMap3, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV3 + texOffsetCurrent * layerTexCoord.uvSpaceScale[3], lod).r;

    SetEnabledHeightByLayer(height0, height1, height2, height3);

    #if defined(_HEIGHT_BASED_BLEND)
        // Modify blendMask to take into account the height of the layer. Higher height should be more visible.
        blendMasks = ApplyHeightBlend(half4(height0, height1, height2, height3), blendMasks, _HeightTransition);
    #endif

    half weights[_MAX_LAYER];
    ComputeMaskWeights(blendMasks, weights);

#if defined(_MAIN_LAYER_INFLUENCE_MODE) && defined(_HEIGHTMAP)
    half influenceMask = blendMasks.a;
    #ifdef _INFLUENCEMASK_MAP
    influenceMask *= SAMPLE_TEXTURE2D_LOD(_LayerInfluenceMaskMap, sampler_LayerInfluenceMaskMap, layerTexCoord.baseUV0, lod).r;
    #endif
    height1 += height0 * _InheritBaseHeight1 * influenceMask;
    height2 += height0 * _InheritBaseHeight2 * influenceMask;
    height3 += height0 * _InheritBaseHeight3 * influenceMask;
#endif

    return BlendLayeredScalar(height0, height1, height2, height3, weights);
}

float2 ParallaxOcclusionMapping(half lod, half lodThreshold, int numSteps, half3 viewDirTS, half4 blendMasks, LayerTexCoord layerTexCoord, out float outHeight)
{
    real stepSize = 1.0 / (real)numSteps;
    half2 parallaxMaxOffsetTS = (viewDirTS.xy / -viewDirTS.z);
    half2 texOffsetPerStep = stepSize * parallaxMaxOffsetTS;

    float2 texOffsetCurrent = float2(0.0, 0.0);
    half prevHeight = ComputePerPixelHeightDisplacement(texOffsetCurrent, lod, blendMasks, layerTexCoord);
    texOffsetCurrent += texOffsetPerStep;
    half currHeight = ComputePerPixelHeightDisplacement(texOffsetCurrent, lod, blendMasks, layerTexCoord);
    half rayHeight = 1.0 - stepSize;

    for (int stepIndex = 0; stepIndex < numSteps; ++stepIndex)
    {
        if (currHeight > rayHeight)
            break;

        prevHeight = currHeight;
        rayHeight -= stepSize;
        texOffsetCurrent += texOffsetPerStep;

        currHeight = ComputePerPixelHeightDisplacement(texOffsetCurrent, lod, blendMasks, layerTexCoord);
    }

    // Refine the search with secant method
#define POM_SECANT_METHOD 1
#if POM_SECANT_METHOD

    half pt0 = rayHeight + stepSize;
    half pt1 = rayHeight;
    half delta0 = pt0 - prevHeight;
    half delta1 = pt1 - currHeight;

    half delta;
    float2 offset;

    // Secant method to affine the search
    // Ref: Faster Relief Mapping Using the Secant Method - Eric Risser
    for (int i = 0; i < 3; ++i)
    {
        half intersectionHeight = (pt0 * delta1 - pt1 * delta0) / (delta1 - delta0);
        offset = (1 - intersectionHeight) * texOffsetPerStep * numSteps;
        currHeight = ComputePerPixelHeightDisplacement(offset, lod, blendMasks, layerTexCoord);
        delta = intersectionHeight - currHeight;

        if (abs(delta) <= 0.01)
            break;

        if (delta < 0.0)
        {
            delta1 = delta;
            pt1 = intersectionHeight;
        }
        else
        {
            delta0 = delta;
            pt0 = intersectionHeight;
        }
    }

#else // regular POM intersection
    half delta0 = currHeight - rayHeight;
    half delta1 = (rayHeight + stepSize) - prevHeight;
    half ratio = delta0 / (delta0 + delta1);
    float2 offset = texOffsetCurrent - ratio * texOffsetPerStep;

    currHeight = ComputePerPixelHeightDisplacement(offset, lod, blendMasks, layerTexCoord);
#endif

    outHeight = currHeight;

    // Fade the effect with lod (allow to avoid pop when switching to a discrete LOD mesh)
    offset *= (1.0 - saturate(lod - lodThreshold));

    return offset;
}

float ApplyPerPixelDisplacement(half3 viewDirTS, half3 viewDirWS, half4 blendMasks, inout half3 positionWS, inout LayerTexCoord layerTexCoord)
{
    float2 minUvSize = GetMinUvSize(layerTexCoord);
    half lod = ComputeTextureLOD(minUvSize);

    half  maxHeight0 = abs(_HeightAmplitude);
    half  maxHeight1 = abs(_HeightAmplitude1);
    half  maxHeight2 = abs(_HeightAmplitude2);
    half  maxHeight3 = abs(_HeightAmplitude3);

    ApplyDisplacementTileScale(maxHeight0, maxHeight1, maxHeight2, maxHeight3);
    #if defined(_MAIN_LAYER_INFLUENCE_MODE) && defined(_HEIGHTMAP)
        maxHeight1 += abs(_HeightAmplitude) * _InheritBaseHeight1;
        maxHeight2 += abs(_HeightAmplitude) * _InheritBaseHeight2;
        maxHeight3 += abs(_HeightAmplitude) * _InheritBaseHeight3;
    #endif

    half weights[_MAX_LAYER];
    ComputeMaskWeights(blendMasks, weights);
    half maxHeight = BlendLayeredScalar(maxHeight0, maxHeight1, maxHeight2, maxHeight3, weights);

    layerTexCoord.uvSpaceScale[0] = _BaseMap_ST.xy * _InvPrimScale.xy;
    layerTexCoord.uvSpaceScale[1] = _BaseMap1_ST.xy * _InvPrimScale.xy;
    layerTexCoord.uvSpaceScale[2] = _BaseMap2_ST.xy * _InvPrimScale.xy;
    layerTexCoord.uvSpaceScale[3] = _BaseMap3_ST.xy * _InvPrimScale.xy;

    float2 uvSpaceScale = BlendLayeredVector2(layerTexCoord.uvSpaceScale[0], layerTexCoord.uvSpaceScale[1], layerTexCoord.uvSpaceScale[2], layerTexCoord.uvSpaceScale[3], weights);

    half height = 0;
    real NdotV = viewDirTS.z;

    viewDirTS *= GetDisplacementObjectScale(false).xzy;

    float3 viewDirUV = normalize(float3(viewDirTS.xy * maxHeight, viewDirTS.z));
    float  unitAngle = saturate(FastACosPos(viewDirUV.z) * INV_HALF_PI);            // TODO: optimize
    int numSteps = (int)lerp(_PPDMinSamples, _PPDMaxSamples, unitAngle);
    float2 offset = ParallaxOcclusionMapping(lod, _PPDLodThreshold, numSteps, viewDirUV, blendMasks, layerTexCoord, height);
    offset *= uvSpaceScale;

    layerTexCoord.baseUV0 += offset;
    layerTexCoord.baseUV1 += offset;
    layerTexCoord.baseUV2 += offset;
    layerTexCoord.baseUV3 += offset;
    layerTexCoord.emissionUV += offset;

    layerTexCoord.detailUV0 = layerTexCoord.baseUV0 * _DetailMap_ST.xy;
    layerTexCoord.detailUV1 = layerTexCoord.baseUV1 * _DetailMap1_ST.xy;
    layerTexCoord.detailUV2 = layerTexCoord.baseUV2 * _DetailMap2_ST.xy;
    layerTexCoord.detailUV3 = layerTexCoord.baseUV3 * _DetailMap3_ST.xy;

    // Since POM "pushes" geometry inwards (rather than extrude it), { height = height - 1 }.
    // Since the result is used as a 'depthOffsetVS', it needs to be positive, so we flip the sign. { height = -height + 1 }.

    #ifdef _DEPTHOFFSET
        float verticalDisplacement = (maxHeight - height * maxHeight) / max(NdotV, 0.0001);
        positionWS += verticalDisplacement * (-viewDirWS);
        return ComputeNormalizedDeviceCoordinatesWithZ(positionWS, GetWorldToHClipMatrix()).z;
    #else
        return 0.0;
    #endif
}
#endif

// Calculate displacement for per vertex displacement mapping
half3 ComputePerVertexDisplacement(LayerTexCoord layerTexCoord, half4 vertexColor, half lod)
{
    half height0 = (SAMPLE_TEXTURE2D_LOD(_HeightMap, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0, lod).r - _HeightCenter) * _HeightAmplitude;
    half height1 = (SAMPLE_TEXTURE2D_LOD(_HeightMap1, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV1, lod).r - _HeightCenter1) * _HeightAmplitude1;
    half height2 = (SAMPLE_TEXTURE2D_LOD(_HeightMap2, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV2, lod).r - _HeightCenter2) * _HeightAmplitude2;
    half height3 = (SAMPLE_TEXTURE2D_LOD(_HeightMap3, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV3, lod).r - _HeightCenter3) * _HeightAmplitude3;

    // Scale by lod factor to ensure tessellated displacement influence is fully removed by the time we transition LODs
    #if defined(LOD_FADE_CROSSFADE) && defined(_TESSELLATION_DISPLACEMENT)
        height0 *= unity_LODFade.x;
        height1 *= unity_LODFade.x;
        height2 *= unity_LODFade.x;
        height3 *= unity_LODFade.x;
    #endif

    // Height is affected by tiling property and by object scale (depends on option).
    // Apply scaling from tiling properties (TexWorldScale and tiling from BaseColor)
    ApplyDisplacementTileScale(height0, height1, height2, height3);
    // Nullify height that are not used, so compiler can remove unused case
    SetEnabledHeightByLayer(height0, height1, height2, height3);

    half4 blendMasks = GetBlendMask(layerTexCoord.layerMaskUV, TEXTURE2D_ARGS(_LayerMaskMap, sampler_LayerMaskMap), vertexColor, true, lod);

    #if defined(_HEIGHT_BASED_BLEND)
    // Modify blendMask to take into account the height of the layer. Higher height should be more visible.
    blendMasks = ApplyHeightBlend(half4(height0, height1, height2, height3), blendMasks, _HeightTransition);
    #endif

    half weights[_MAX_LAYER];
    ComputeMaskWeights(blendMasks, weights);

    // _MAIN_LAYER_INFLUENCE_MODE is a pure visual mode that doesn't contribute to the weights of a layer
    // The motivation is like this: if a layer is visible, then we will apply influence on top of it (so it is only visual).
    // This is what is done for normal and baseColor and we do the same for height.
    // Note that if we apply influence before ApplyHeightBlend, then have a different behavior.
    #if defined(_MAIN_LAYER_INFLUENCE_MODE) && defined(_HEIGHTMAP)
        // Add main layer influence if any (simply add main layer add on other layer)
        // We multiply by the input mask for the first layer (blendMask.a) because if the mask here is black it means that the layer
        // is not actually underneath any visible layer so we don't want to inherit its height.
        half influenceMask = blendMasks.a;
        influenceMask *= GetInfluenceMask(layerTexCoord, TEXTURE2D_ARGS(_LayerInfluenceMaskMap, sampler_LayerInfluenceMaskMap));

        height1 += height0 * _InheritBaseHeight1 * influenceMask;
        height2 += height0 * _InheritBaseHeight2 * influenceMask;
        height3 += height0 * _InheritBaseHeight3 * influenceMask;
    #endif

    half heightResult = BlendLayeredScalar(height0, height1, height2, height3, weights);

   // Applying scaling of the object if requested
    #ifdef _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
        float3 objectScale = GetDisplacementObjectScale(true);
        // Reminder: mappingType is know statically, so code below is optimize by the compiler
        // Planar and Triplanar are in world space thus it is independent of object scale
        return heightResult.xxx * objectScale;
    #else
        return heightResult.xxx;
    #endif
}

#include "ShaderLibrary/Tessellation/Tessellation.hlsl"
#endif