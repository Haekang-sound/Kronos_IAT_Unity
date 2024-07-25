#ifndef SIMPLE_LIT_MAPS_INCLUDED
#define SIMPLE_LIT_MAPS_INCLUDED

TEXTURE2D(_BaseMap);                SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;
float4 _BaseMap_MipInfo;
TEXTURE2D(_MaskMap);                SAMPLER(sampler_MaskMap);
TEXTURE2D(_SpecularColorMap);       SAMPLER(sampler_SpecularColorMap);
TEXTURE2D(_NormalMap);              SAMPLER(sampler_NormalMap);
TEXTURE2D(_TransmittanceColorMap);  SAMPLER(sampler_TransmittanceColorMap);
TEXTURE2D(_EmissionMap);            SAMPLER(sampler_EmissionMap);

#endif