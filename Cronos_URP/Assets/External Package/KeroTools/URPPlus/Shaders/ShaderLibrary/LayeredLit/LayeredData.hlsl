#ifndef UNIVERSAL_LAYERED_DATA_INPUT_INCLUDED
#define UNIVERSAL_LAYERED_DATA_INPUT_INCLUDED

struct LayeredData
{
    half4 baseColor0;
    half4 baseColor1;
    half4 baseColor2;
    half4 baseColor3;

    half4 maskMap0;
    half4 maskMap1;
    half4 maskMap2;
    half4 maskMap3;

    half3 normalMap0;
    half3 normalMap1;
    half3 normalMap2;
    half3 normalMap3;

    half3 bentNormalMap0;
    half3 bentNormalMap1;
    half3 bentNormalMap2;
    half3 bentNormalMap3;
    
    half heightMap0;
    half heightMap1;
    half heightMap2;
    half heightMap3;
};

LayeredData CreateEmptyLayeredData()
{
    LayeredData data = (LayeredData)0;

    half4 maods = half4(0.0, 1.0, 1.0, 0.5);
    
    data.maskMap0 = maods;
    data.maskMap1 = maods;
    data.maskMap2 = maods;
    data.maskMap3 = maods;

    return data;
}

#endif