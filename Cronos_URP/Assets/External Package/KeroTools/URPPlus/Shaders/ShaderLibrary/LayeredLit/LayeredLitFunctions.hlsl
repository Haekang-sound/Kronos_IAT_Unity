#ifndef UNIVERSAL_LAYEREDLIT_FUNCTIONS_INPUT_INCLUDED
#define UNIVERSAL_LAYEREDLIT_FUNCTIONS_INPUT_INCLUDED

#include "ShaderLibrary/LayeredLit/LayerTexCoord.hlsl"

#define _MAX_LAYER 4

#if defined(_LAYEREDLIT_4_LAYERS)
    #define _LAYER_COUNT 4
#elif defined(_LAYEREDLIT_3_LAYERS)
    #define _LAYER_COUNT 3
#else
    #define _LAYER_COUNT 2
#endif

#define LAYERS_HEIGHTMAP_ENABLE (defined(_HEIGHTMAP) || defined(_HEIGHTMAP1) || (_LAYER_COUNT > 2 && defined(_HEIGHTMAP2)) || (_LAYER_COUNT > 3 && defined(_HEIGHTMAP3)))

half BlendLayeredScalar(half x0, half x1, half x2, half x3, half weight[_MAX_LAYER])
{
    half result = 0.0;

    result = x0 * weight[0] + x1 * weight[1];

    #if _LAYER_COUNT >= 3
        result += x2 * weight[2];
    #endif

    #if _LAYER_COUNT >= 4
        result += x3 * weight[3];
    #endif

    return result;
}

half2 BlendLayeredVector2(half2 x0, half2 x1, half2 x2, half2 x3, half weight[_MAX_LAYER])
{
    half2 result = half2(0.0, 0.0);

    result = x0 * weight[0] + x1 * weight[1];

    #if _LAYER_COUNT >= 3
        result += (x2 * weight[2]);
    #endif

    #if _LAYER_COUNT >= 4
        result += x3 * weight[3];
    #endif

    return result;
}

half3 BlendLayeredVector3(half3 x0, half3 x1, half3 x2, half3 x3, half weight[_MAX_LAYER])
{
    half3 result = half3(0.0, 0.0, 0.0);

    result = x0 * weight[0] + x1 * weight[1];

    #if _LAYER_COUNT >= 3
        result += (x2 * weight[2]);
    #endif

    #if _LAYER_COUNT >= 4
        result += x3 * weight[3];
    #endif

    return result;
}

void ComputeMaskWeights(half4 inputMasks, out half outWeights[_MAX_LAYER])
{
    ZERO_INITIALIZE_ARRAY(half, outWeights, _MAX_LAYER);

    half masks[_MAX_LAYER];
    masks[0] = inputMasks.a;

    masks[1] = inputMasks.r;
    
    #if _LAYER_COUNT > 2
        masks[2] = inputMasks.g;
    #else
        masks[2] = 0.0;
    #endif

    #if _LAYER_COUNT > 3
        masks[3] = inputMasks.b;
    #else
        masks[3] = 0.0;
    #endif

    // calculate weight of each layers
    // Algorithm is like this:
    // Top layer have priority on others layers
    // If a top layer doesn't use the full weight, the remaining can be use by the following layer.
    half weightsSum = 0.0;

    UNITY_UNROLL
    for (int i = _LAYER_COUNT - 1; i >= 0; --i)
    {
        outWeights[i] = min(masks[i], (1.0 - weightsSum));
        weightsSum = saturate(weightsSum + masks[i]);
    }
}

half4 GetBlendMask(float2 uv, TEXTURE2D_PARAM(layerMaskMap, sampler_layerMaskMap), half4 vertexColor, bool useLodSampling = false, float lod = 0)
{
    float4 blendMasks = useLodSampling ? 
                        SAMPLE_TEXTURE2D_LOD(layerMaskMap, sampler_layerMaskMap, uv, lod) : 
                        SAMPLE_TEXTURE2D(layerMaskMap, sampler_layerMaskMap, uv);

    #if defined(_LAYER_MASK_VERTEX_COLOR_MUL)
        blendMasks *= saturate(vertexColor);
    #elif defined(_LAYER_MASK_VERTEX_COLOR_ADD)
        blendMasks = saturate(blendMasks + vertexColor * 2.0 - 1.0);
    #endif

    return blendMasks;
}

half GetInfluenceMask(LayerTexCoord layerTexCoord, TEXTURE2D_PARAM(layerInfluenceMaskMap, sampler_layerInfluenceMaskMap), float lod = 0)
{
    #ifdef _INFLUENCEMASK_MAP
        return SAMPLE_TEXTURE2D_LOD(layerInfluenceMaskMap, sampler_layerInfluenceMaskMap, layerTexCoord.baseUV0, lod).r;
    #else
        return 1.0;
    #endif
}

void SetEnabledHeightByLayer(inout half height0, inout half height1, inout half height2, inout half height3)
{
#ifndef _HEIGHTMAP
    height0 = 0.0;
#endif
#ifndef _HEIGHTMAP1
    height1 = 0.0;
#endif
#ifndef _HEIGHTMAP2
    height2 = 0.0;
#endif
#ifndef _HEIGHTMAP3
    height3 = 0.0;
#endif

#if _LAYER_COUNT < 4
    height3 = 0.0;
#endif
#if _LAYER_COUNT < 3
    height2 = 0.0;
#endif
}

half GetMaxHeight(half4 heights)
{
    half maxHeight = max(heights.r, heights.g);

    #ifdef _LAYEREDLIT_4_LAYERS
        maxHeight = max(Max3(heights.r, heights.g, heights.b), heights.a);
    #endif

    #ifdef _LAYEREDLIT_3_LAYERS
        maxHeight = Max3(heights.r, heights.g, heights.b);
    #endif

    return maxHeight;
}

half GetMinHeight(half4 heights)
{
    half minHeight = min(heights.r, heights.g);

    #ifdef _LAYEREDLIT_4_LAYERS
        minHeight = min(Min3(heights.r, heights.g, heights.b), heights.a);
    #endif

    #ifdef _LAYEREDLIT_3_LAYERS
        minHeight = Min3(heights.r, heights.g, heights.b);
    #endif

    return minHeight;
}

half4 ApplyHeightBlend(half4 heights, half4 blendMask, half heightTransition)
{
    half4 maskedHeights = (heights - GetMinHeight(heights)) * blendMask.argb;

    half maxHeight = GetMaxHeight(maskedHeights);
    half transition = max(heightTransition, 1e-5);

    maskedHeights = maskedHeights - maxHeight.xxxx;

    maskedHeights = (max(0, maskedHeights + transition) + 1e-6) * blendMask.argb;

    maxHeight = GetMaxHeight(maskedHeights);
    maskedHeights = maskedHeights / max(maxHeight.xxxx, 1e-6);

    return saturate(maskedHeights.yzwx);
}

void ComputeLayerWeights(LayeredData layeredData, half4 blendMasks, half heightTransition, out half outWeights[_MAX_LAYER])
{
    heightTransition *= 0.01;

    #if defined(_DENSITY_MODE)
        half4 inputAlphaMask = half4(layeredData.baseColor0.a, layeredData.baseColor1.a, layeredData.baseColor2.a, layeredData.baseColor3.a);
        half4 opacityAsDensity = saturate((inputAlphaMask - (half4(1.0, 1.0, 1.0, 1.0) - blendMasks.argb)) * 20.0); // 20.0 is the number of steps in inputAlphaMask (Density mask. We decided 20 empirically)
        half4 useOpacityAsDensityParam = half4(_OpacityAsDensity, _OpacityAsDensity1, _OpacityAsDensity2, _OpacityAsDensity3);
        blendMasks.argb = lerp(blendMasks.argb, opacityAsDensity, useOpacityAsDensityParam);
    #endif

    #if LAYERS_HEIGHTMAP_ENABLE
        #if defined(_HEIGHT_BASED_BLEND)
        half4 heightLayers = half4(layeredData.heightMap0, layeredData.heightMap1, layeredData.heightMap2, layeredData.heightMap3);
        blendMasks = ApplyHeightBlend(heightLayers, blendMasks, heightTransition);
        #endif
    #endif

    ComputeMaskWeights(blendMasks, outWeights);
}

half3 ComputeMainBaseColorInfluence(LayerTexCoord layerTexCoord, LayeredData layeredData, half influenceMask, half inputMainLayerMask, half weights[_MAX_LAYER])
{
    half3 baseColor = BlendLayeredVector3(layeredData.baseColor0.rgb, layeredData.baseColor1.rgb, layeredData.baseColor2.rgb, layeredData.baseColor3.rgb, weights);

    half influenceFactor = BlendLayeredScalar(0.0, _InheritBaseColor1, _InheritBaseColor2, _InheritBaseColor3, weights) * influenceMask * inputMainLayerMask;

    half textureBias = 15.0;
    half3 baseMeanColor0 = SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, layerTexCoord.baseUV0, textureBias).rgb * _BaseColor.rgb;
    half3 baseMeanColor1 = SAMPLE_TEXTURE2D_LOD(_BaseMap1, sampler_BaseMap1, layerTexCoord.baseUV1, textureBias).rgb * _BaseColor1.rgb;
    half3 baseMeanColor2 = SAMPLE_TEXTURE2D_LOD(_BaseMap2, sampler_BaseMap2, layerTexCoord.baseUV2, textureBias).rgb * _BaseColor2.rgb;
    half3 baseMeanColor3 = SAMPLE_TEXTURE2D_LOD(_BaseMap3, sampler_BaseMap3, layerTexCoord.baseUV3, textureBias).rgb * _BaseColor3.rgb;

    half3 meanColor = BlendLayeredVector3(baseMeanColor0, baseMeanColor1, baseMeanColor2, baseMeanColor3, weights);

    half3 factor = baseColor > meanColor ? (layeredData.baseColor0.rgb - meanColor) : (layeredData.baseColor0.rgb * baseColor / max(meanColor, 0.001) - baseColor);
    
    return influenceFactor * factor + baseColor;
}

half3 ComputeMainNormalInfluence(half influenceMask, half3 normalTS0, half3 normalTS1, half3 normalTS2, half3 normalTS3, half inputMainLayerMask, half weights[_MAX_LAYER])
{
    half3 normalTS = BlendLayeredVector3(normalTS0, normalTS1, normalTS2, normalTS3, weights);
    
    half influenceFactor = BlendLayeredScalar(0.0, _InheritBaseNormal1, _InheritBaseNormal2, _InheritBaseNormal3, weights) * influenceMask;

    half3 neutralNormalTS = half3(0.0, 0.0, 1.0);

    half3 mainNormalTS = lerp(neutralNormalTS, normalTS0, influenceFactor);

    return lerp(normalTS, BlendNormalRNM(normalTS, mainNormalTS), influenceFactor * inputMainLayerMask);
}

#endif