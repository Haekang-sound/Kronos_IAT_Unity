#ifndef UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LitDisplacement.hlsl"

#if defined(_NORMALMAP) || (defined(_PIXEL_DISPLACEMENT) && defined(_SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS)) || defined(_THREADMAP)
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float4 tangentOS : TANGENT;
    float2 texcoord : TEXCOORD0;
    float3 normal : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD1;
    half3 normalWS : TEXCOORD2;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS : TEXCOORD3;
    #endif

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS : TEXCOORD4; // xyz: tangent, w: sign
    #endif

    half3 viewDirWS : TEXCOORD5;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings DepthNormalsVertex(Attributes input)
{
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normal, input.tangentOS);
    #ifdef _VERTEX_DISPLACEMENT
        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        positionRWS += normalInput.normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
    output.normalWS = half3(normalInput.normalWS);
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sign = input.tangentOS.w * float(GetOddNegativeScale());
        half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
    #endif

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        output.positionWS = vertexInput.positionWS;
    #endif

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        output.tangentWS = tangentWS;
    #endif
    
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);

    return output;
}

float3 ApplyNormals(Varyings input, float2 uv)
{
    #ifdef REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
        float sgn = input.tangentWS.w; // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        float3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_NormalMap, sampler_NormalMap), _NormalScale);

        #ifdef _THREADMAP
            float2 threadUV = TRANSFORM_TEX(uv, _ThreadMap);
            half2 threadAG = SAMPLE_TEXTURE2D(_ThreadMap, sampler_ThreadMap, threadUV).ag;

            normalTS = ThreadNormal(threadAG, normalTS, _ThreadNormalScale);
        #endif

        return TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
    #else
        return input.normalWS.xyz;
    #endif
}

void DepthNormalsFragment(
    Varyings input
    , out half4 outNormalWS : SV_Target0
    #ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
    #endif
    #ifdef _DEPTHOFFSET
    , out float outputDepth : SV_Depth
    #endif
)
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

    #if defined(_GBUFFER_NORMALS_OCT)
        float3 normalWS = normalize(input.normalWS);
        float2 octNormalWS = PackNormalOctQuadEncode(normalWS);           // values between [-1, +1], must use fp32 on some platforms
        float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);   // values between [ 0,  1]
        half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);      // values between [ 0,  1]
        outNormalWS = half4(packedNormalWS, 0.0);
    #else
        float3 normalWS = input.normalWS;

        float2 uv = input.uv;

        #ifdef _SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS
            #ifdef _PIXEL_DISPLACEMENT
                half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);

                ApplyPOM(uv, viewDirTS, viewDirWS, input.positionWS
                        #ifdef _DEPTHOFFSET
                            , outputDepth
                        #endif
                        );
            #endif

            normalWS = ApplyNormals(input, uv);
        #endif

        outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
    #endif

    #ifdef _WRITE_RENDERING_LAYERS
        uint renderingLayers = GetMeshRenderingLayer();
        outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
    #endif
}

#endif