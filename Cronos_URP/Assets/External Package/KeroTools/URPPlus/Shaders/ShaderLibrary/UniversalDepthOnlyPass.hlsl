#ifndef UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
#define UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LitDisplacement.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS   : NORMAL;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;
    float4 positionCS : SV_POSITION;
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

    output.uv = TRANSFORM_TEX(data.texcoord, _BaseMap);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(data.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS);

    #ifdef _TESSELLATION_DISPLACEMENT
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
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

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a;
    Alpha(albedoAlpha, shadowCutoff);

    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(input.positionCS);
    #endif

    return input.positionCS.z;
}
#endif