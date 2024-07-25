#ifndef UNIVERSAL_UVMAPPING_INCLUDED
#define UNIVERSAL_UVMAPPING_INCLUDED

struct UVMapping
{
    float2 uv;  // Current uv or planar uv

    // Triplanar specific
    float2 uvZY;
    float2 uvXZ;
    float2 uvXY;

    float3 normalWS; // vertex normal
    float3 triplanarWeights;
};

UVMapping InitializeUVData(float3 position, float3 normalWS, float2 uv)
{
    UVMapping uvData = (UVMapping)0;

    uvData.uv = uv;

    uvData.uvXY = position.xy;
    uvData.uvXZ = position.xz;
    uvData.uvZY = position.zy;

    uvData.normalWS = normalWS;
    uvData.triplanarWeights = ComputeTriplanarWeights(normalWS);

    return uvData;
}


#endif