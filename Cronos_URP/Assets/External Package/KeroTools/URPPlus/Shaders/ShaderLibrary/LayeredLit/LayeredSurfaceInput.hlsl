#ifndef UNIVERSAL_LAYERED_INPUT_SURFACE_INCLUDED
#define UNIVERSAL_LAYERED_INPUT_SURFACE_INCLUDED

#include "ShaderLibrary/SurfaceInput.hlsl"
#include "ShaderLibrary/DetailMapping.hlsl"

half3 SampleLayeredNormal(float2 uv, TEXTURE2D_PARAM(normalMap, sampler_normalMap), half scale = half(1.0))
{
    #if defined (_NORMALMAP) || (_NORMALMAP1) || (_NORMALMAP2) || (_NORMALMAP3)
        half4 normal = SAMPLE_TEXTURE2D(normalMap, sampler_normalMap, uv);
        return ScaleNormal(normal, scale);
    #else
        return half3(0.0h, 0.0h, 1.0h);
    #endif
}

half3 SampleLayeredBentNormal(float2 uv, TEXTURE2D_PARAM(bentNormalMap, sampler_bentNormalMap), half scale = half(1.0))
{
    #if defined (_BENTNORMALMAP) || (_BENTNORMALMAP1) || (_BENTNORMALMAP2) || (_BENTNORMALMAP3)
        half4 normal = SAMPLE_TEXTURE2D(bentNormalMap, sampler_bentNormalMap, uv);
        return ScaleNormal(normal, scale);
    #else
        return half3(0.0h, 0.0h, 1.0h);
    #endif
}

void InitializeLayeredMaskMap(LayerTexCoord layerTexCoord, inout LayeredData layeredData)
{
    layeredData.maskMap0 = half4(_Metallic, 1.0, 1.0, _Smoothness);
    layeredData.maskMap1 = half4(_Metallic1, 1.0, 1.0, _Smoothness1);
    layeredData.maskMap2 = half4(_Metallic2, 1.0, 1.0, _Smoothness2);
    layeredData.maskMap3 = half4(_Metallic3, 1.0, 1.0, _Smoothness3);

    #ifdef _MASKMAP
        half4 metallicSmoothnessRemap = half4(_MetallicRemapMin, _MetallicRemapMax, _SmoothnessRemapMin, _SmoothnessRemapMax);
        half2 aoRemap = half2(_AORemapMin, _AORemapMax);
        layeredData.maskMap0 = SampleMaskMap(layerTexCoord.baseUV0, TEXTURE2D_ARGS(_MaskMap, SAMPLER_MASKMAP_IDX), 
                                                metallicSmoothnessRemap, aoRemap, layeredData.maskMap0);
    #endif

    #ifdef _MASKMAP1
        half4 metallicSmoothnessRemap1 = half4(_MetallicRemapMin1, _MetallicRemapMax1, _SmoothnessRemapMin1, _SmoothnessRemapMax1);
        half2 aoRemap1 = half2(_AORemapMin1, _AORemapMax1);
        layeredData.maskMap1 = SampleMaskMap(layerTexCoord.baseUV1, TEXTURE2D_ARGS(_MaskMap1, SAMPLER_MASKMAP_IDX), 
                                                metallicSmoothnessRemap1, aoRemap1, layeredData.maskMap1);
    #endif

    #ifdef _MASKMAP2
        half4 metallicSmoothnessRemap2 = half4(_MetallicRemapMin2, _MetallicRemapMax2, _SmoothnessRemapMin2, _SmoothnessRemapMax2);
        half2 aoRemap2 = half2(_AORemapMin2, _AORemapMax2);
        layeredData.maskMap2 = SampleMaskMap(layerTexCoord.baseUV2, TEXTURE2D_ARGS(_MaskMap2, SAMPLER_MASKMAP_IDX), 
                                                metallicSmoothnessRemap2, aoRemap2, layeredData.maskMap2);
    #endif

    #ifdef _MASKMAP3
        half4 metallicSmoothnessRemap3 = half4(_MetallicRemapMin3, _MetallicRemapMax3, _SmoothnessRemapMin3, _SmoothnessRemapMax3);
        half2 aoRemap3 = half2(_AORemapMin3, _AORemapMax3);
        layeredData.maskMap3 = SampleMaskMap(layerTexCoord.baseUV3, TEXTURE2D_ARGS(_MaskMap3, SAMPLER_MASKMAP_IDX), 
                                                metallicSmoothnessRemap3, aoRemap3, layeredData.maskMap3);
    #endif
}

void InitializeLayeredNormalMap(LayerTexCoord layerTexCoord, inout LayeredData layeredData)
{
    layeredData.normalMap0 = SampleLayeredNormal(layerTexCoord.baseUV0, TEXTURE2D_ARGS(_NormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    layeredData.normalMap1 = SampleLayeredNormal(layerTexCoord.baseUV1, TEXTURE2D_ARGS(_NormalMap1, SAMPLER_NORMALMAP_IDX), _NormalScale1);
    layeredData.normalMap2 = SampleLayeredNormal(layerTexCoord.baseUV2, TEXTURE2D_ARGS(_NormalMap2, SAMPLER_NORMALMAP_IDX), _NormalScale2);
    layeredData.normalMap3 = SampleLayeredNormal(layerTexCoord.baseUV3, TEXTURE2D_ARGS(_NormalMap3, SAMPLER_NORMALMAP_IDX), _NormalScale3);
}

void InitializeLayeredBentNormalMap(LayerTexCoord layerTexCoord, inout LayeredData layeredData)
{
    layeredData.bentNormalMap0 = SampleLayeredBentNormal(layerTexCoord.baseUV0, TEXTURE2D_ARGS(_BentNormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    layeredData.bentNormalMap1 = SampleLayeredBentNormal(layerTexCoord.baseUV1, TEXTURE2D_ARGS(_BentNormalMap1, SAMPLER_NORMALMAP_IDX), _NormalScale1);
    layeredData.bentNormalMap2 = SampleLayeredBentNormal(layerTexCoord.baseUV2, TEXTURE2D_ARGS(_BentNormalMap2, SAMPLER_NORMALMAP_IDX), _NormalScale2);
    layeredData.bentNormalMap3 = SampleLayeredBentNormal(layerTexCoord.baseUV3, TEXTURE2D_ARGS(_BentNormalMap3, SAMPLER_NORMALMAP_IDX), _NormalScale3);
}

void InitializeLayeredHeightMap(LayerTexCoord layerTexCoord, inout LayeredData layeredData)
{
    #ifdef _HEIGHTMAP
        layeredData.heightMap0 = (SAMPLE_TEXTURE2D_LOD(_HeightMap, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0, 1).r - _HeightCenter) * _HeightAmplitude;
    #endif
    
    #ifdef _HEIGHTMAP1
        layeredData.heightMap1 = (SAMPLE_TEXTURE2D_LOD(_HeightMap1, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0, 1).r - _HeightCenter1) * _HeightAmplitude1;
    #endif
    
    #ifdef _HEIGHTMAP2
        layeredData.heightMap2 = (SAMPLE_TEXTURE2D_LOD(_HeightMap2, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0, 1).r - _HeightCenter2) * _HeightAmplitude2;
    #endif
    
    #ifdef _HEIGHTMAP3
        layeredData.heightMap3 = (SAMPLE_TEXTURE2D_LOD(_HeightMap3, SAMPLER_HEIGHTMAP_IDX, layerTexCoord.baseUV0, 1).r - _HeightCenter3) * _HeightAmplitude3;
    #endif
}

void InitializeLayeredData(LayerTexCoord layerTexCoord, out LayeredData layeredData)
{
    layeredData = CreateEmptyLayeredData();

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half2 alphaRemap1 = half2(_AlphaRemapMin1, _AlphaRemapMax1);
    half2 alphaRemap2 = half2(_AlphaRemapMin2, _AlphaRemapMax2);
    half2 alphaRemap3 = half2(_AlphaRemapMin3, _AlphaRemapMax3);

    layeredData.baseColor0 = SampleAlbedoAlpha(_BaseColor, alphaRemap, layerTexCoord.baseUV0, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    layeredData.baseColor1 = SampleAlbedoAlpha(_BaseColor1, alphaRemap1, layerTexCoord.baseUV1, TEXTURE2D_ARGS(_BaseMap1, sampler_BaseMap1));
    layeredData.baseColor2 = SampleAlbedoAlpha(_BaseColor2, alphaRemap2, layerTexCoord.baseUV2, TEXTURE2D_ARGS(_BaseMap2, sampler_BaseMap2));
    layeredData.baseColor3 = SampleAlbedoAlpha(_BaseColor3, alphaRemap3, layerTexCoord.baseUV3, TEXTURE2D_ARGS(_BaseMap3, sampler_BaseMap3));

    InitializeLayeredMaskMap(layerTexCoord, layeredData);
    InitializeLayeredNormalMap(layerTexCoord, layeredData);
    InitializeLayeredBentNormalMap(layerTexCoord, layeredData);
    InitializeLayeredHeightMap(layerTexCoord, layeredData);
}

void ApplyLayeredDetailMap(LayerTexCoord layerTexCoord, LayeredData layeredData, half weights[_MAX_LAYER],
                            inout half3 albedo, inout half3 normalTS, inout half smoothness)
{
    #ifdef _DETAIL_MAP
        half4 detailMap0 = SAMPLE_TEXTURE2D(_DetailMap, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV0);

        albedo = DetailAlbedo(albedo, detailMap0.r, layeredData.maskMap0.b * weights[0], _DetailAlbedoScale);
        normalTS = ApplyDetailNormal(normalTS, detailMap0.ag, _DetailNormalScale, layeredData.maskMap0.b * weights[0]);
        smoothness = DetailSmoothness(smoothness, detailMap0.b, _DetailSmoothnessScale, layeredData.maskMap0.b * weights[0]);
    #endif

    #ifdef _DETAIL_MAP1
        half4 detailMap1 = SAMPLE_TEXTURE2D(_DetailMap1, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV1);

        albedo = DetailAlbedo(albedo, detailMap1.r, layeredData.maskMap1.b * weights[1], _DetailAlbedoScale1);
        normalTS = ApplyDetailNormal(normalTS, detailMap1.ag, _DetailNormalScale1, layeredData.maskMap1.b * weights[1]);
        smoothness = DetailSmoothness(smoothness, detailMap1.b, _DetailSmoothnessScale1, layeredData.maskMap1.b * weights[1]);
    #endif

    #ifdef _DETAIL_MAP2
        half4 detailMap2 = SAMPLE_TEXTURE2D(_DetailMap2, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV2);

        albedo = DetailAlbedo(albedo, detailMap2.r, layeredData.maskMap2.b * weights[2], _DetailAlbedoScale2);
        normalTS = ApplyDetailNormal(normalTS, detailMap2.ag, _DetailNormalScale2, layeredData.maskMap2.b * weights[2]);
        smoothness = DetailSmoothness(smoothness, detailMap2.b, _DetailSmoothnessScale2, layeredData.maskMap2.b * weights[2]);
    #endif

    #ifdef _DETAIL_MAP3
        half4 detailMap3 = SAMPLE_TEXTURE2D(_DetailMap3, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV3);

        albedo = DetailAlbedo(albedo, detailMap3.r, layeredData.maskMap3.b * weights[3], _DetailAlbedoScale3);
        normalTS = ApplyDetailNormal(normalTS, detailMap3.ag, _DetailNormalScale3, layeredData.maskMap3.b * weights[3]);
        smoothness = DetailSmoothness(smoothness, detailMap3.b, _DetailSmoothnessScale3, layeredData.maskMap3.b * weights[3]);
    #endif
}

#endif