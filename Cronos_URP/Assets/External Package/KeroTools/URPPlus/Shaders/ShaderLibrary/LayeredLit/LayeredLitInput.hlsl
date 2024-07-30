#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredData.hlsl"
#include "ShaderLibrary/LayeredLit/LayerTexCoord.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredLitMaps.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredLitProperties.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredLitFunctions.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredSurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/Weather.hlsl"

inline void InitializeTexCoordinates(float2 uv, out LayerTexCoord layerTexCoord)
{
    layerTexCoord.layerMaskUV = TRANSFORM_TEX(uv, _LayerMaskMap);
    layerTexCoord.baseUV0     = TRANSFORM_TEX(uv, _BaseMap);
    layerTexCoord.baseUV1     = TRANSFORM_TEX(uv, _BaseMap1);
    layerTexCoord.baseUV2     = TRANSFORM_TEX(uv, _BaseMap2);
    layerTexCoord.baseUV3     = TRANSFORM_TEX(uv, _BaseMap3);

    layerTexCoord.detailUV0   = TRANSFORM_TEX(uv, _DetailMap);
    layerTexCoord.detailUV1   = TRANSFORM_TEX(uv, _DetailMap1);
    layerTexCoord.detailUV2   = TRANSFORM_TEX(uv, _DetailMap2);
    layerTexCoord.detailUV3   = TRANSFORM_TEX(uv, _DetailMap3);

    layerTexCoord.uvSpaceScale[0] = float2(1.0, 1.0);
    layerTexCoord.uvSpaceScale[1] = float2(1.0, 1.0);
    layerTexCoord.uvSpaceScale[2] = float2(1.0, 1.0);
    layerTexCoord.uvSpaceScale[3] = float2(1.0, 1.0);

    layerTexCoord.emissionUV  = TRANSFORM_TEX(uv, _EmissionMap);
}

inline void InitializeSurfaceData(LayerTexCoord layerTexCoord, half4 vertexColor, out SurfaceData outSurfaceData)
{
    outSurfaceData = EmptyFill();

    LayeredData layeredData;
    InitializeLayeredData(layerTexCoord, layeredData);

    half weights[_MAX_LAYER];
    half4 blendMasks = GetBlendMask(layerTexCoord.layerMaskUV, TEXTURE2D_ARGS(_LayerMaskMap, sampler_LayerMaskMap), vertexColor);
    ComputeLayerWeights(layeredData, blendMasks, _HeightTransition, weights);

#if defined(_MAIN_LAYER_INFLUENCE_MODE)
    float influenceMask = GetInfluenceMask(layerTexCoord, TEXTURE2D_ARGS(_LayerInfluenceMaskMap, sampler_LayerInfluenceMaskMap));

    if (influenceMask > 0.0f)
    {
        outSurfaceData.albedo = ComputeMainBaseColorInfluence(layerTexCoord, layeredData, influenceMask, blendMasks.a, weights);
        outSurfaceData.normalTS = ComputeMainNormalInfluence(influenceMask, layeredData.normalMap0, layeredData.normalMap1, layeredData.normalMap2, layeredData.normalMap3, blendMasks.a, weights);
        outSurfaceData.bentNormalTS = ComputeMainNormalInfluence(influenceMask, layeredData.bentNormalMap0, layeredData.bentNormalMap1, layeredData.bentNormalMap2, layeredData.bentNormalMap3, blendMasks.a, weights);
    }
    else
#endif
    {
        outSurfaceData.albedo = BlendLayeredVector3(layeredData.baseColor0.rgb, layeredData.baseColor1.rgb, layeredData.baseColor2.rgb, layeredData.baseColor3.rgb, weights);
        outSurfaceData.normalTS = BlendLayeredVector3(layeredData.normalMap0, layeredData.normalMap1, layeredData.normalMap2, layeredData.normalMap3, weights);
        outSurfaceData.bentNormalTS = BlendLayeredVector3(layeredData.bentNormalMap0, layeredData.bentNormalMap1, layeredData.bentNormalMap2, layeredData.bentNormalMap3, weights);
    }

    outSurfaceData.metallic = BlendLayeredScalar(layeredData.maskMap0.r, layeredData.maskMap1.r, layeredData.maskMap2.r, layeredData.maskMap3.r, weights);
    outSurfaceData.smoothness = BlendLayeredScalar(layeredData.maskMap0.a, layeredData.maskMap1.a, layeredData.maskMap2.a, layeredData.maskMap3.a, weights);
    outSurfaceData.occlusion = BlendLayeredScalar(layeredData.maskMap0.g, layeredData.maskMap1.g, layeredData.maskMap2.g, layeredData.maskMap3.g, weights);

    half blendedAlpha = BlendLayeredScalar(layeredData.baseColor0.a, layeredData.baseColor1.a, layeredData.baseColor2.a, layeredData.baseColor3.a, weights);
    outSurfaceData.alpha = Alpha(blendedAlpha, _AlphaCutoff);

    ApplyLayeredDetailMap(layerTexCoord, layeredData, weights, outSurfaceData.albedo, outSurfaceData.normalTS, outSurfaceData.smoothness);

    outSurfaceData.emission = SampleEmission(layerTexCoord.emissionUV, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap),
                                                outSurfaceData.albedo, _EmissionColor.rgb, _EmissionScale);

    outSurfaceData.horizonFade = _HorizonFade;
    outSurfaceData.giOcclusionBias = _GIOcclusionBias;
}

#endif