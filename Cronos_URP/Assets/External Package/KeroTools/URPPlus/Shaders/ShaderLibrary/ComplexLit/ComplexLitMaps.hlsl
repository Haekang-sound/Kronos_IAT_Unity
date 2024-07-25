#ifndef COMPLEX_LIT_MAPS_INCLUDED
#define COMPLEX_LIT_MAPS_INCLUDED

TEXTURE2D(_BaseMap);                      SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;
float4 _BaseMap_MipInfo;
TEXTURE2D(_MaskMap);                      SAMPLER(sampler_MaskMap);
TEXTURE2D(_SpecularColorMap);             SAMPLER(sampler_SpecularColorMap);
TEXTURE2D(_NormalMap);                    SAMPLER(sampler_NormalMap);
TEXTURE2D(_BentNormalMap);                SAMPLER(sampler_BentNormalMap);
TEXTURE2D(_HeightMap);                    SAMPLER(sampler_HeightMap);
TEXTURE2D(_TangentMap);                   SAMPLER(sampler_TangentMap);
TEXTURE2D(_AnisotropyMap);                SAMPLER(sampler_AnisotropyMap);
TEXTURE2D(_ThicknessCurvatureMap);        SAMPLER(sampler_ThicknessCurvatureMap);
TEXTURE2D(_IridescenceMaskMap);           SAMPLER(sampler_IridescenceMaskMap);
TEXTURE2D(_IridescenceThicknessMap);      SAMPLER(sampler_IridescenceThicknessMap);
TEXTURE2D(_ClearCoatMap);                 SAMPLER(sampler_ClearCoatMap);
TEXTURE2D(_CoatNormalMap);                SAMPLER(sampler_CoatNormalMap);
TEXTURE2D(_DetailMap);                    SAMPLER(sampler_DetailMap);
TEXTURE2D(_TransmittanceColorMap);        SAMPLER(sampler_TransmittanceColorMap);
TEXTURE2D(_EmissionMap);                  SAMPLER(sampler_EmissionMap);

#if defined(_NORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_NormalMap
#elif defined(_BENTNORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_BentNormalMap
#elif defined(_COATNORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_CoatNormalMap
#else
    #define SAMPLER_NORMALMAP_IDX sampler_NormalMap
#endif

#endif