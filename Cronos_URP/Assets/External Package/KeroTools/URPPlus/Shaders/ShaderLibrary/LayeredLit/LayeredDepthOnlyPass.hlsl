#ifndef UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
#define UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LayeredLit/LayeredDisplacement.hlsl"

#if defined(_LAYER_MASK_VERTEX_COLOR_MUL) || (_LAYER_MASK_VERTEX_COLOR_ADD)
    #define VERTEX_COLOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;
    float4 positionCS : SV_POSITION;
    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

#ifndef TESSELLATION_ON
Varyings DepthOnlyVertex(Attributes input)
{
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
        output.vertexColor = input.vertexColor;
    #endif

    #ifdef _VERTEX_DISPLACEMENT
        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 normalWS = TransformObjectToWorldNormal(input.normalOS);

        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(input.texcoord, layerTexCoord);

        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
        positionRWS += normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif

    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    
    return output;
}
#else
TessellationControlPoint DepthOnlyVertex(Attributes input)
{
    TessellationControlPoint output = (TessellationControlPoint)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.texcoord = input.texcoord;
    output.normalOS = input.normalOS;
    output.positionOS = input.positionOS;
    #if defined(VERTEX_COLOR)
    output.vertexColor = input.vertexColor;
    #endif

    return output;
}

[domain("tri")]
Varyings DepthDomain(TessellationFactors tessFactors, const OutputPatch<TessellationControlPoint, 3> input, float3 baryCoords : SV_DomainLocation)
{
    Varyings output = (Varyings)0;
    Attributes data = (Attributes)0;

    UNITY_SETUP_INSTANCE_ID(input[0]);
    UNITY_TRANSFER_INSTANCE_ID(input[0], output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    data.positionOS = BARYCENTRIC_INTERPOLATE(positionOS); 
    data.normalOS = BARYCENTRIC_INTERPOLATE(normalOS);
    data.texcoord = BARYCENTRIC_INTERPOLATE(texcoord);
    #if defined(_VERTEX_COLOR)
    data.vertexColor = BARYCENTRIC_INTERPOLATE(vertexColor);
    half4 vertexColor = data.vertexColor;
    #else
    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #endif

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

    output.uv = data.texcoord;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(data.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS);

    #ifdef _TESSELLATION_DISPLACEMENT
        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(data.texcoord, layerTexCoord);

        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
        vertexInput.positionWS += normalInput.normalWS * height;
    #endif

    output.positionCS = TransformWorldToHClip(vertexInput.positionWS);

    return output;
}
#endif

half DepthOnlyFragment(Varyings input) : SV_TARGET
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    half shadowCutoff = _AlphaCutoff;
    #ifdef SHADOW_CUTOFF
        shadowCutoff = _AlphaCutoffShadow;
    #endif

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
    #endif

    LayerTexCoord layerTexCoord;
    InitializeTexCoordinates(input.uv, layerTexCoord);
    LayeredData layeredData;
    InitializeLayeredData(layerTexCoord, layeredData);

    half weights[_MAX_LAYER];
    half4 blendMasks = GetBlendMask(layerTexCoord.layerMaskUV, TEXTURE2D_ARGS(_LayerMaskMap, sampler_LayerMaskMap), vertexColor);
    ComputeLayerWeights(layeredData, blendMasks, _HeightTransition, weights);

    half alphasBlend = BlendLayeredScalar(layeredData.baseColor0.a, layeredData.baseColor1.a, layeredData.baseColor2.a, layeredData.baseColor3.a, weights);
    Alpha(alphasBlend, shadowCutoff);

    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(input.positionCS);
    #endif

    return input.positionCS.z;
}
#endif