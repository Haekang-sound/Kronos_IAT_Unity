#ifndef META_PASS_INCLUDED
#define META_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
#include "ShaderLibrary/LitDisplacement.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 uv0 : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float2 uv2 : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;

    #ifdef EDITOR_VISUALIZATION
    float2 VizUV : TEXCOORD1;
    float4 LightCoord : TEXCOORD2;
    #endif
};

#ifndef TESSELLATION_ON
Varyings VertexMeta(Attributes input)
{
    Varyings output = (Varyings)0;

    output.uv = TRANSFORM_TEX(input.uv0, _BaseMap);

    half4 positionOS = input.positionOS;
    #ifdef _VERTEX_DISPLACEMENT
        half3 positionRWS = TransformObjectToWorld(positionOS.xyz);
        half3 normalWS = TransformObjectToWorldNormal(input.normalOS);
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        positionRWS += normalWS * height;
        positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif

    output.positionCS = UnityMetaVertexPosition(positionOS.xyz, input.uv1, input.uv2);

    #ifdef EDITOR_VISUALIZATION
    UnityEditorVizData(positionOS.xyz, input.uv0, input.uv1, input.uv2, output.VizUV, output.LightCoord);
    #endif
    
    return output;
}
#else
TessellationControlPoint VertexMeta(Attributes input)
{
    TessellationControlPoint output = (TessellationControlPoint)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.positionOS = input.positionOS;
    output.normalOS = input.normalOS;
    output.uv0 = input.uv0;
    output.uv1 = input.uv1;
    output.uv2 = input.uv2;

    return output;
}

[domain("tri")]
Varyings MetaDomain(TessellationFactors tessFactors, const OutputPatch<TessellationControlPoint, 3> input, float3 baryCoords : SV_DomainLocation)
{
    Varyings output = (Varyings)0;
    Attributes data = (Attributes)0;

    UNITY_SETUP_INSTANCE_ID(input[0]);
    UNITY_TRANSFER_INSTANCE_ID(input[0], output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    data.positionOS = BARYCENTRIC_INTERPOLATE(positionOS); 
    data.normalOS = BARYCENTRIC_INTERPOLATE(normalOS);
    data.uv0 = BARYCENTRIC_INTERPOLATE(uv0);
    data.uv1 = BARYCENTRIC_INTERPOLATE(uv1);
    data.uv2 = BARYCENTRIC_INTERPOLATE(uv2);

    #if defined(_TESSELLATION_PHONG)
        float3 p0 = TransformObjectToWorld(input[0].positionOS.xyz);
        float3 p1 = TransformObjectToWorld(input[1].positionOS.xyz);
        float3 p2 = TransformObjectToWorld(input[2].positionOS.xyz);

        float3 n0 = TransformObjectToWorldNormal(input[0].normalOS);
        float3 n1 = TransformObjectToWorldNormal(input[1].normalOS);
        float3 n2 = TransformObjectToWorldNormal(input[2].normalOS);
        float3 positionWS = TransformObjectToWorld(data.positionOS.xyz);

        positionWS = PhongTessellation(positionWS, p0, p1, p2, n0, n1, n2, baryCoords, _TessellationShapeFactor);
        data.positionOS = mul(unity_WorldToObject, float4(positionWS, 1.0));
    #endif

    VertexPositionInputs vertexInput = GetVertexPositionInputs(data.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS);

    output.uv = TRANSFORM_TEX(data.uv0, _BaseMap);

    #ifdef _TESSELLATION_DISPLACEMENT
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        vertexInput.positionWS += normalInput.normalWS * height;
        data.positionOS = mul(unity_WorldToObject, half4(vertexInput.positionWS, 1.0));
    #endif

    output.positionCS = MetaVertexPosition(data.positionOS, data.uv1, data.uv2, unity_LightmapST, unity_DynamicLightmapST);

    #ifdef EDITOR_VISUALIZATION
    UnityEditorVizData(data.positionOS.xyz, data.uv0, data.uv1, data.uv2, output.VizUV, output.LightCoord);
    #endif

    return output;
}
#endif

half4 UniversalFragmentMeta(Varyings fragIn, MetaInput metaInput)
{
    #ifdef EDITOR_VISUALIZATION
    metaInput.VizUV = fragIn.VizUV;
    metaInput.LightCoord = fragIn.LightCoord;
    #endif

    return UnityMetaFragment(metaInput);
}

half4 FragmentMeta(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    InitializeSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness,
                       surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = surfaceData.emission;

    return UniversalFragmentMeta(input, metaInput);
}

#endif