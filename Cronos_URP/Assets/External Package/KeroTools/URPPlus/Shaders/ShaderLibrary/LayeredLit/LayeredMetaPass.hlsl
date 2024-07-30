#ifndef UNIVERSAL_LAYERED_LIT_META_PASS_INCLUDED
#define UNIVERSAL_LAYERED_LIT_META_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
#include "ShaderLibrary/LayeredLit/LayeredDisplacement.hlsl"

#if defined(_LAYER_MASK_VERTEX_COLOR_MUL) || (_LAYER_MASK_VERTEX_COLOR_ADD)
    #define VERTEX_COLOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 uv0 : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float2 uv2 : TEXCOORD2;
    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif

    #ifdef EDITOR_VISUALIZATION
    float2 VizUV : TEXCOORD1;
    float4 LightCoord : TEXCOORD2;
    #endif
};

Varyings LayeredLitVertexMeta(Attributes input)
{
    Varyings output = (Varyings)0;

    output.uv = input.uv0;
    
    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
        output.vertexColor = input.vertexColor;
    #endif

    #ifdef _VERTEX_DISPLACEMENT
        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 normalWS = TransformObjectToWorldNormal(input.normalOS);

        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(input.uv0, layerTexCoord);

        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
        positionRWS += normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif

    output.positionCS = UnityMetaVertexPosition(input.positionOS.xyz, input.uv1, input.uv2);

    #ifdef EDITOR_VISUALIZATION
    UnityEditorVizData(input.positionOS.xyz, input.uv0, input.uv1, input.uv2, output.VizUV, output.LightCoord);
    #endif
    
    return output;
}

half4 UniversalFragmentMeta(Varyings fragIn, MetaInput metaInput)
{
    #ifdef EDITOR_VISUALIZATION
    metaInput.VizUV = fragIn.VizUV;
    metaInput.LightCoord = fragIn.LightCoord;
    #endif

    return UnityMetaFragment(metaInput);
}

half4 LayeredLitFragmentMeta(Varyings input) : SV_Target
{
    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
    #endif

    LayerTexCoord layerTexCoord;
    InitializeTexCoordinates(input.uv, layerTexCoord);

    LayeredData layeredData;
    InitializeLayeredData(layerTexCoord, layeredData);

    SurfaceData surfaceData;
    InitializeSurfaceData(layerTexCoord, vertexColor, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness,
                       surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = surfaceData.emission;

    return UniversalFragmentMeta(input, metaInput);
}

#endif