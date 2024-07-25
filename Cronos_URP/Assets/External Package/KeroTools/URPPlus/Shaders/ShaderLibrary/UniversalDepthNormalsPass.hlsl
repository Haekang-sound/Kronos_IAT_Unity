#ifndef UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LitDisplacement.hlsl"

#if defined(_NORMALMAP) || (_DETAIL) || (_DOUBLESIDED_ON) || ((_PIXEL_DISPLACEMENT) && (_SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS))
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS   : NORMAL;
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

#ifndef TESSELLATION_ON
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
#else
TessellationControlPoint DepthNormalsVertex(Attributes input)
{
    TessellationControlPoint output = (TessellationControlPoint)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.positionOS = input.positionOS;
    output.normalOS = input.normalOS;
    output.tangentOS = input.tangentOS;
    output.texcoord = input.texcoord;

    return output;
}

[domain("tri")]
Varyings DepthNormalsDomain(TessellationFactors tessFactors, const OutputPatch<TessellationControlPoint, 3> input, float3 baryCoords : SV_DomainLocation)
{
    Varyings output = (Varyings)0;
    Attributes data = (Attributes)0;

    UNITY_SETUP_INSTANCE_ID(input[0]);
    UNITY_TRANSFER_INSTANCE_ID(input[0], output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    data.positionOS = BARYCENTRIC_INTERPOLATE(positionOS); 
    data.normalOS = BARYCENTRIC_INTERPOLATE(normalOS);
    data.texcoord = BARYCENTRIC_INTERPOLATE(texcoord);
    data.tangentOS = BARYCENTRIC_INTERPOLATE(tangentOS);

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
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS, data.tangentOS);

    #ifdef _TESSELLATION_DISPLACEMENT
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        vertexInput.positionWS += normalInput.normalWS * height;
    #endif

    #if defined(DEPTH_NORMALS_PASS_VARYINGS)
        output.positionCS = TransformWorldToHClip(vertexInput.positionWS);
        output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
    #endif

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sign = data.tangentOS.w * GetOddNegativeScale();
        output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
    #endif

    return output;
}
#endif

float3 ApplyNormals(Varyings input, half faceSign, float2 uv)
{
    #ifdef REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
        float sgn = input.tangentWS.w; // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        float3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_NormalMap, sampler_NormalMap), _NormalScale);

        #if defined(_DETAIL)
            float2 detailUV = TRANSFORM_TEX(uv, _DetailMap);
            half2 detailAG = SAMPLE_TEXTURE2D(_DetailMap, sampler_DetailMap, detailUV).ag;
            
            #ifdef _MASKMAP
                half detailMask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, uv).b;
            #else
                half detailMask = 1.0;
            #endif

            normalTS = ApplyDetailNormal(normalTS, detailAG, _DetailNormalScale, detailMask);
        #endif

        #ifdef _DOUBLESIDED_ON
            ApplyDoubleSidedFlipOrMirror(faceSign, _DoubleSidedConstants.xyz, normalTS);
        #endif

        return TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
    #else
        return input.normalWS.xyz;
    #endif
}

void DepthNormalsFragment(
    Varyings input
    , half faceSign : VFACE
    , out half4 outNormalWS : SV_Target0
    #ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
    #endif
    #if defined(_SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS) && defined(_DEPTHOFFSET)
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
        
                float depthOffset = ApplyPerPixelDisplacement(viewDirTS, viewDirWS, input.positionWS, uv);
                
                #ifdef _DEPTHOFFSET
                    outputDepth = depthOffset;
                #endif
            #endif

            normalWS = ApplyNormals(input, faceSign, uv);
        #endif

        outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
    #endif

    #ifdef _WRITE_RENDERING_LAYERS
        uint renderingLayers = GetMeshRenderingLayer();
        outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
    #endif
}

#endif