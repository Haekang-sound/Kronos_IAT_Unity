#ifndef EYE_MAPS_INCLUDED
#define EYE_MAPS_INCLUDED

//Legacy:
TEXTURE2D(_BaseMap);            SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;
float4 _BaseMap_MipInfo;

TEXTURE2D(_ScleraMap);          SAMPLER(sampler_ScleraMap);
TEXTURE2D(_ScleraNormalMap);    SAMPLER(sampler_ScleraNormalMap);
TEXTURE2D(_IrisMap);            SAMPLER(sampler_IrisMap);
TEXTURE2D(_IrisNormalMap);      SAMPLER(sampler_IrisNormalMap);
TEXTURE2D(_EmissionMap);        SAMPLER(sampler_EmissionMap);

#if defined(_NORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_ScleraNormalMap
#elif defined(_BENTNORMALMAP)
    #define SAMPLER_NORMALMAP_IDX sampler_IrisNormalMap
#else
    #define SAMPLER_NORMALMAP_IDX sampler_ScleraNormalMap
#endif

#endif