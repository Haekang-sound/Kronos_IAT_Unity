#ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED

#include "ShaderLibrary/Eye/EyeLighting.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

#if defined(_SCLERA_NORMALMAP) || defined(_IRIS_NORMALMAP) || defined(_DOUBLESIDED_ON)
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

// keep this file in sync with LitGBufferPass.hlsl

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float2 texcoord : TEXCOORD0;
    float2 staticLightmapUV : TEXCOORD1;
    float2 dynamicLightmapUV : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS : TEXCOORD1;
    #endif

    float3 normalWS : TEXCOORD2;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS : TEXCOORD3; // xyz: tangent, w: sign
    #endif
    float3 viewDirWS : TEXCOORD4;

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
    half4 fogFactorAndVertexLight   : TEXCOORD5; // x: fogFactor, yzw: vertex light
    #else
    half fogFactor : TEXCOORD5;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD6;
    #endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
    #ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD9; // Dynamic lightmap UVs
    #endif

    float4 positionCS : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, out VectorsData vData, out InputData inputData)
{
    inputData = (InputData)0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        inputData.positionWS = input.positionWS;
    #endif

    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    inputData.normalWS = NormalizeNormalPerPixel(input.normalWS);
    inputData.viewDirectionWS = viewDirWS;

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        inputData.shadowCoord = input.shadowCoord;
    #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
        inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
    #else
        inputData.shadowCoord = float4(0, 0, 0, 0);
    #endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactorAndVertexLight.x);
        inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    #else
        inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactor);
    #endif

    #if defined(DYNAMICLIGHTMAP_ON)
        inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
    #else
        inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
    #endif

    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

    vData = CreateVectorsData(input.normalWS.xyz, inputData.normalWS, viewDirWS);

    #if defined(DEBUG_DISPLAY)
        #if defined(DYNAMICLIGHTMAP_ON)
            inputData.dynamicLightmapUV = input.dynamicLightmapUV;
        #endif

        #if defined(LIGHTMAP_ON)
            inputData.staticLightmapUV = input.staticLightmapUV;
        #else
            inputData.vertexSH = input.vertexSH;
        #endif
    #endif
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Physically Based) shader
Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

    half fogFactor = 0;
    #if !defined(_FOG_FRAGMENT)
        fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
    #endif

    output.uv = input.texcoord;

    // already normalized from normal transform to WS.
    output.normalWS = normalInput.normalWS;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sign = input.tangentOS.w * GetOddNegativeScale();
        half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
        output.tangentWS = tangentWS;
    #endif

    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
    #ifdef DYNAMICLIGHTMAP_ON
        output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
    #else
        output.fogFactor = fogFactor;
    #endif

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        output.positionWS = vertexInput.positionWS;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

    output.positionCS = vertexInput.positionCS;

    return output;
}

inline void TransformEyeVectorsToOS(Varyings input, inout float3 positionOS, inout float3 viewDirOS, inout float3 normalOS)
{
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    positionOS = (TransformWorldToObject(input.positionWS)) * _MeshScale;
    viewDirOS = TransformWorldToObjectDir(viewDirWS);
    normalOS = TransformWorldToObjectDir(normalize(input.normalWS));
}

// Used in Standard (Physically Based) shader
void LitPassFragment(
    Varyings input
    , half faceSign : VFACE
    , out half4 outColor : SV_Target0
    #ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
    #endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    
    float3 positionOS, viewDirOS, normalOS;
    TransformEyeVectorsToOS(input, positionOS, viewDirOS, normalOS);
    
    float3 refractedPosOS = CorneaRefraction(positionOS + float3(0.0, 0.0, _PositionOffset.z), viewDirOS, normalOS, 1.333h, 0.02h) + _PositionOffset.xyz;
    float2 irisUV = IrisUVLocation(refractedPosOS, 0.225h);
    float2 scleraUV = ScleraUVLocation(positionOS);
    
    half scleraLimbalRing = ScleraLimbalRing(positionOS, viewDirOS, 0.225h, _LimbalRingSizeSclera, _LimbalRingFade, _LimbalRingIntensity);

    half surfaceMask = CalculateSurfaceMask(positionOS);
    half mydriasisK = 1.0;

    float2 circlePupilAnim = CirclePupilAnimation(irisUV, _PupilRadius, saturate(mydriasisK * _PupilAperture), _MinimalPupilAperture, _MaximalPupilAperture);
    float2 irisOffset = circlePupilAnim;

    half3 irisAlbedo = SAMPLE_TEXTURE2D(_IrisMap, sampler_IrisMap, irisOffset).rgb;
    half3 scleraAlbedo = SAMPLE_TEXTURE2D(_ScleraMap, sampler_ScleraMap, scleraUV).rgb * scleraLimbalRing;

    half irisLimbalRing = IrisLimbalRing(irisUV, viewDirOS, _LimbalRingSizeIris, _LimbalRingFade, _LimbalRingIntensity);
    half3 irisColor = IrisOutOfBoundColorClamp(irisOffset, irisAlbedo, _IrisClampColor.rgb) * irisLimbalRing;

    half3 irisNormal = SampleEyeNormal(irisOffset, TEXTURE2D_ARGS(_IrisNormalMap, SAMPLER_NORMALMAP_IDX), _IrisNormalScale);
    half3 scleraNormal = SampleEyeNormal(scleraUV, TEXTURE2D_ARGS(_ScleraNormalMap, SAMPLER_NORMALMAP_IDX), _ScleraNormalScale);

    half3 diffuseNormalTS = lerp(scleraNormal, irisNormal, surfaceMask);
    half3 specularNormalTS = lerp(scleraNormal, half3(0.0, 0.0, 1.0), surfaceMask);

    half3 surfaceNormal = NormalizeNormalPerPixel(input.normalWS);
    half3 diffuseNormalWS = surfaceNormal;
    half3 specularNormalWS = surfaceNormal;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sgn = input.tangentWS.w; // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);

        #if defined(_IRIS_NORMALMAP) || (_DOUBLESIDED_ON)
            diffuseNormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(diffuseNormalTS, tangentToWorld));
        #endif

        #if defined(_SCLERA_NORMALMAP) || (_DOUBLESIDED_ON)
            specularNormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(specularNormalTS, tangentToWorld));
        #endif
    #endif

    SurfaceData surfaceData = EmptyFill();
    surfaceData.albedo = lerp(scleraAlbedo, irisColor, surfaceMask);
    surfaceData.smoothness = lerp(_ScleraSmoothness, _CorneaSmoothness, surfaceMask);
    surfaceData.emission = SampleEmission(irisOffset, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), 
                                            surfaceData.albedo, _EmissionColor.rgb, _EmissionScale * surfaceMask);

    #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
        GeometricAAFiltering(input.normalWS.xyz, _SpecularAAScreenSpaceVariance, _SpecularAAThreshold, surfaceData.smoothness);
    #endif

    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(input.positionCS);
    #endif

    InputData inputData;
    VectorsData vData;
    InitializeInputData(input, vData, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

    #ifdef _DBUFFER
        ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
    #endif

    half4 color = EyeFragment(inputData, surfaceData, vData, diffuseNormalWS, specularNormalWS);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, IsSurfaceTypeTransparent(_Surface));

    outColor = color;

    #ifdef _WRITE_RENDERING_LAYERS
        uint renderingLayers = GetMeshRenderingLayer();
        outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
    #endif
}

#endif
