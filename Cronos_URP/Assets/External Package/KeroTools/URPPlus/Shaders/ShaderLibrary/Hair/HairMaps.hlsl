#ifndef EYE_MAPS_INCLUDED
#define EYE_MAPS_INCLUDED

TEXTURE2D(_BaseMap);                SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;
float4 _BaseMap_MipInfo;
TEXTURE2D(_NormalMap);              SAMPLER(sampler_NormalMap);
TEXTURE2D(_AmbientOcclusionMap);    SAMPLER(sampler_AmbientOcclusionMap);
TEXTURE2D(_SmoothnessMaskMap);      SAMPLER(sampler_SmoothnessMaskMap);

#endif