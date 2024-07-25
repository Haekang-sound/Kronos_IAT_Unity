#ifndef FABRIC_MAPS_INCLUDED
#define FABRIC_MAPS_INCLUDED

TEXTURE2D(_BaseMap);                    SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;              
float4 _BaseMap_MipInfo;        
TEXTURE2D(_MaskMap);                    SAMPLER(sampler_MaskMap);
TEXTURE2D(_NormalMap);                  SAMPLER(sampler_NormalMap);
TEXTURE2D(_BentNormalMap);              SAMPLER(sampler_BentNormalMap);
TEXTURE2D(_HeightMap);                  SAMPLER(sampler_HeightMap);
TEXTURE2D(_SpecularColorMap);           SAMPLER(sampler_SpecularColorMap);
TEXTURE2D(_FuzzMap);                    SAMPLER(sampler_FuzzMap);
TEXTURE2D(_ThicknessCurvatureMap);      SAMPLER(sampler_ThicknessCurvatureMap);
TEXTURE2D(_EmissionMap);                SAMPLER(sampler_EmissionMap);

#if defined(_SCLERA_NORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_NormalMap
#elif defined(_IRIS_NORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_BentNormalMap
#else
    #define SAMPLER_NORMALMAP_IDX sampler_NormalMap
#endif

#endif