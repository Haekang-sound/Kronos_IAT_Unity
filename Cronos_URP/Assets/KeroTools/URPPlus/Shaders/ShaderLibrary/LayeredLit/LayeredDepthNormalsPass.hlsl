#ifndef UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_DEPTH_NORMALS_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LayeredLit/LayeredDisplacement.hlsl"

#if defined(_NORMALMAP) || defined(_NORMALMAP1) || defined(_NORMALMAP2) || defined(_NORMALMAP3)
    #define _NORMAL
#endif

#if defined(_DETAIL_MAP) || defined(_DETAIL_MAP1) || defined(_DETAIL_MAP2) || defined(_DETAIL_MAP3)
    #define _DETAIL
#endif

#if defined(_NORMAL) || defined(_DETAIL) || defined(_DOUBLESIDED_ON) || defined(_PIXEL_DISPLACEMENT)
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

#if defined(_LAYER_MASK_VERTEX_COLOR_MUL) || (_LAYER_MASK_VERTEX_COLOR_ADD)
    #define VERTEX_COLOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float4 tangentOS : TANGENT;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    
    half3 normalWS : TEXCOORD2;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS : TEXCOORD3;
    #endif

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS : TEXCOORD4; // xyz: tangent, w: sign
    #endif

    half3 viewDirWS : TEXCOORD5;

    float2 uv : TEXCOORD1;

    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

#ifndef TESSELLATION_ON
Varyings DepthNormalsVertex(Attributes input)
{
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = input.texcoord;
    #if defined(VERTEX_COLOR)
        output.vertexColor = input.vertexColor;
        half4 vertexColor = input.vertexColor;
    #else
        half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #endif

    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    #ifdef _VERTEX_DISPLACEMENT
        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(output.uv, layerTexCoord);

        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
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
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS, data.tangentOS);

    #ifdef _TESSELLATION_DISPLACEMENT
        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(data.texcoord, layerTexCoord);

        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
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

void ApplyLayeredDetailNormal(LayerTexCoord layerTexCoord, LayeredData layeredData, half weights[_MAX_LAYER], inout half3 normalTS)
{
    #ifdef _DETAIL_MAP
        half4 detailMap0 = SAMPLE_TEXTURE2D(_DetailMap, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV0);
        normalTS = ApplyDetailNormal(normalTS, detailMap0.ag, _DetailNormalScale, layeredData.maskMap0.b * weights[0]);
    #endif

    #ifdef _DETAIL_MAP1
        half4 detailMap1 = SAMPLE_TEXTURE2D(_DetailMap1, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV1);
        normalTS = ApplyDetailNormal(normalTS, detailMap1.ag, _DetailNormalScale1, layeredData.maskMap1.b * weights[1]);
    #endif

    #ifdef _DETAIL_MAP2
        half4 detailMap2 = SAMPLE_TEXTURE2D(_DetailMap2, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV2);
        normalTS = ApplyDetailNormal(normalTS, detailMap2.ag, _DetailNormalScale2, layeredData.maskMap2.b * weights[2]);
    #endif

    #ifdef _DETAIL_MAP3
        half4 detailMap3 = SAMPLE_TEXTURE2D(_DetailMap3, SAMPLER_DETAILMAP_IDX, layerTexCoord.detailUV3);
        normalTS = ApplyDetailNormal(normalTS, detailMap3.ag, _DetailNormalScale3, layeredData.maskMap3.b * weights[3]);
    #endif
}

float3 ApplyNormals(Varyings input, LayerTexCoord layerTexCoord, LayeredData layeredData, half4 blendMasks, half faceSign, half weights[_MAX_LAYER])
{
    #if defined(_NORMAL) || defined(_DETAIL)
        float sgn = input.tangentWS.w;      // should be either +1 or -1
        half3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);

        half3 normalTS = half3(0.0, 0.0, 1.0);

        #if defined(_MAIN_LAYER_INFLUENCE_MODE)
        float influenceMask = GetInfluenceMask(layerTexCoord, TEXTURE2D_ARGS(_LayerInfluenceMaskMap, sampler_LayerInfluenceMaskMap));

        if (influenceMask > 0.0f)
        {
            normalTS = ComputeMainNormalInfluence(influenceMask, layeredData.normalMap0, layeredData.normalMap1, layeredData.normalMap2, layeredData.normalMap3, blendMasks.a, weights);
        }
        else
        #endif
        {
            normalTS = BlendLayeredVector3(layeredData.normalMap0, layeredData.normalMap1, layeredData.normalMap2, layeredData.normalMap3, weights);
        }

        ApplyLayeredDetailNormal(layerTexCoord, layeredData, weights, normalTS);

        #ifdef _DOUBLESIDED_ON
            ApplyDoubleSidedFlipOrMirror(faceSign, _DoubleSidedConstants.xyz, normalTS);
        #endif

        return TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
    #else
        return input.normalWS;
    #endif
}

void DepthNormalsFragment(
    Varyings input
    , half faceSign : VFACE
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

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(_VERTEX_COLOR)
        vertexColor = input.vertexColor;
    #endif

    LayerTexCoord layerTexCoord;
    InitializeTexCoordinates(input.uv, layerTexCoord);

    half weights[_MAX_LAYER];
    half4 blendMasks = GetBlendMask(layerTexCoord.layerMaskUV, TEXTURE2D_ARGS(_LayerMaskMap, sampler_LayerMaskMap), vertexColor);

    #ifdef _PIXEL_DISPLACEMENT
        half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
        half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
        float depthOffset = ApplyPerPixelDisplacement(viewDirTS, viewDirWS, blendMasks, input.positionWS, layerTexCoord);

        #ifdef _DEPTHOFFSET
            outputDepth = depthOffset;
        #endif
    #endif

    LayeredData layeredData;
    InitializeLayeredData(layerTexCoord, layeredData);
    ComputeLayerWeights(layeredData, blendMasks, _HeightTransition, weights);

    half alphasBlend = BlendLayeredScalar(layeredData.baseColor0.a, layeredData.baseColor1.a, layeredData.baseColor2.a, layeredData.baseColor3.a, weights);
    half alpha = Alpha(alphasBlend, shadowCutoff);

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
        #ifdef _SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS
            normalWS = ApplyNormals(input, layerTexCoord, layeredData, blendMasks, faceSign, weights);
        #endif

        outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
    #endif

    #ifdef _WRITE_RENDERING_LAYERS
        uint renderingLayers = GetMeshRenderingLayer();
        outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
    #endif
}

#endif