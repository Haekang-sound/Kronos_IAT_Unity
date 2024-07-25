#ifndef UNIVERSAL_LAYERED_TEXCOORD_INCLUDED
#define UNIVERSAL_LAYERED_TEXCOORD_INCLUDED

struct LayerTexCoord
{
    float2 layerMaskUV;
    
    float2 baseUV0;
    float2 baseUV1;
    float2 baseUV2;
    float2 baseUV3;

    float2 detailUV0;
    float2 detailUV1;
    float2 detailUV2;
    float2 detailUV3;

    float2 uvSpaceScale[4];

    float2 emissionUV;
};

#endif