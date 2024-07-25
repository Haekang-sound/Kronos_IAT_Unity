#ifndef VECTORS_DATA_INCLUDED
#define VECTORS_DATA_INCLUDED

struct VectorsData
{
    half3 geomNormalWS;
    half3 normalWS;
    half3 bentNormalWS;
    half3 coatNormalWS;
    half3 viewDirectionWS;
    half4 tangentWS;
    half3 bitangentWS;
};

VectorsData CreateVectorsData(half3 geomNormalWS, half3 normalWS, half3 viewDirectionWS)
{
    VectorsData vData = (VectorsData)0;

    vData.geomNormalWS = geomNormalWS;
    vData.normalWS = normalWS;
    vData.viewDirectionWS = viewDirectionWS;

    return vData;
}

VectorsData CreateVectorsData(half3 geomNormalWS, half3 normalWS, half3 bentNormalWS, 
                                half3 coatNormalWS, half3 viewDirectionWS, half4 tangentWS)
{
    VectorsData vData = (VectorsData)0;

    vData.geomNormalWS = geomNormalWS;
    vData.normalWS = normalWS;
    vData.bentNormalWS = bentNormalWS;
    vData.coatNormalWS = coatNormalWS;
    vData.viewDirectionWS = viewDirectionWS;
    vData.tangentWS = tangentWS;

    return vData;
}

VectorsData CreateEmptyVectorsData()
{
    const VectorsData data = (VectorsData)0;

    return data;
}

#endif