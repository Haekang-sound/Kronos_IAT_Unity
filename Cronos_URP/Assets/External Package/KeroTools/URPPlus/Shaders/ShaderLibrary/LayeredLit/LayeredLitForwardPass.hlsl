#ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED

#include "ShaderLibrary/LitLighting.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LayeredLit/LayeredDisplacement.hlsl"

#if defined(_NORMALMAP) || (_NORMALMAP1) || (_NORMALMAP2) || (_NORMALMAP3)
    #define _NORMAL
#endif

#if defined(_BENTNORMALMAP) || (_BENTNORMALMAP1) || (_BENTNORMALMAP2) || (_BENTNORMALMAP3)
    #define _BENTNORMAL
#endif

#if defined(_DETAIL_MAP) || (_DETAIL_MAP1) || (_DETAIL_MAP2) || (_DETAIL_MAP3)
    #define _DETAIL
#endif

#if defined(_NORMAL) || defined(_BENTNORMAL) || defined(_DETAIL) || defined(_RAIN_NORMALMAP) || defined(_DOUBLESIDED_ON) || defined(_PIXEL_DISPLACEMENT)
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

#if defined(_LAYER_MASK_VERTEX_COLOR_MUL) || (_LAYER_MASK_VERTEX_COLOR_ADD)
    #define VERTEX_COLOR
#endif

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float2 texcoord : TEXCOORD0;
    float2 staticLightmapUV : TEXCOORD1;
    float2 dynamicLightmapUV : TEXCOORD2;
    #if defined(VERTEX_COLOR)
    float4 vertexColor : COLOR;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
    float3 positionWS : TEXCOORD1;
    #endif

    float3 normalWS : TEXCOORD2;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS : TEXCOORD3; // xyz: tangent, w: sign
    #endif
    float3 viewDirWS : TEXCOORD4;

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
    half4 fogFactorAndVertexLight : TEXCOORD5; // x: fogFactor, yzw: vertex light
    #else
    half fogFactor : TEXCOORD5;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord : TEXCOORD6;
    #endif

    #if defined(VERTEX_COLOR)
    half4 vertexColor : COLOR;
    #endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
    #ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD9; // Dynamic lightmap UVs
    #endif

    float4 positionCS : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, SurfaceData surfaceData, out VectorsData vData, out InputData inputData)
{
    inputData = (InputData)0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        inputData.positionWS = input.positionWS;
    #endif

    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sgn = input.tangentWS.w; // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        inputData.tangentToWorld = tangentToWorld;
    #endif
    
    inputData.normalWS = input.normalWS;
    #if defined(_NORMAL) || defined(_DETAIL) || defined(_DOUBLESIDED_ON)
        inputData.normalWS = TransformTangentToWorld(surfaceData.normalTS, tangentToWorld);
    #endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    inputData.viewDirectionWS = viewDirWS;

    half3 bentNormalWS = inputData.normalWS;
    #if defined(_BENTNORMAL)
        bentNormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(surfaceData.bentNormalTS, tangentToWorld));
    #endif

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

    vData = CreateVectorsData(input.normalWS.xyz, inputData.normalWS, bentNormalWS, 0.0, viewDirWS, 0.0);

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

#ifndef TESSELLATION_ON
Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
        output.vertexColor = input.vertexColor;
    #endif

    output.uv = input.texcoord;

    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    #ifdef _VERTEX_DISPLACEMENT
        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(input.texcoord, layerTexCoord);

        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
        positionRWS += normalInput.normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

    half fogFactor = 0;
    #if !defined(_FOG_FRAGMENT)
        fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
    #endif

    // already normalized from normal transform to WS.
    output.normalWS = normalInput.normalWS;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    real sign = input.tangentOS.w * GetOddNegativeScale();
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
#else
TessellationControlPoint LitPassVertex(Attributes input)
{
    TessellationControlPoint output = (TessellationControlPoint)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    // already normalized from normal transform to WS.
    output.positionOS = input.positionOS;
    output.normalOS = input.normalOS;
    output.tangentOS = input.tangentOS;
    output.texcoord = input.texcoord;

    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
    #ifdef DYNAMICLIGHTMAP_ON
    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif

    #if defined(VERTEX_COLOR)
    output.vertexColor = input.vertexColor;
    #endif

    return output;
}

[domain("tri")]
Varyings Domain(TessellationFactors tessFactors, const OutputPatch<TessellationControlPoint, 3> input, float3 baryCoords : SV_DomainLocation)
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

    data.staticLightmapUV = BARYCENTRIC_INTERPOLATE(staticLightmapUV);
    #ifdef DYNAMICLIGHTMAP_ON
    data.dynamicLightmapUV = BARYCENTRIC_INTERPOLATE(dynamicLightmapUV);
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

    output.normalWS = normalInput.normalWS;
    output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
        output.positionWS = vertexInput.positionWS;
    #endif

    output.positionCS = TransformWorldToHClip(vertexInput.positionWS);

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sign = data.tangentOS.w * GetOddNegativeScale();
        output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
    #endif

    #if defined(LIGHTMAP_ON)
        output.staticLightmapUV = data.staticLightmapUV;
    #endif
    #ifdef DYNAMICLIGHTMAP_ON
        output.dynamicLightmapUV = data.dynamicLightmapUV;
    #endif
    OUTPUT_SH(output.normalWS, output.vertexSH);

    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
    half fogFactor = 0;
    #if !defined(_FOG_FRAGMENT)
        fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
    #endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
    #else
        output.fogFactor = fogFactor;
    #endif

    return output;
}
#endif

// Used in Standard (Physically Based) shader
void LitPassFragment(
    Varyings input
    , half faceSign : VFACE
    , out half4 outColor : SV_Target0
    #ifdef _DEPTHOFFSET
    , out float outputDepth : SV_Depth
    #endif
    #ifdef _WRITE_RENDERING_LAYERS
    , out float4 outRenderingLayers : SV_Target1
    #endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
    #endif

    LayerTexCoord layerTexCoord;
    InitializeTexCoordinates(input.uv, layerTexCoord);

    #ifdef _PIXEL_DISPLACEMENT
        half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
        half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);

        half4 blendMasks = GetBlendMask(layerTexCoord.layerMaskUV, TEXTURE2D_ARGS(_LayerMaskMap, sampler_LayerMaskMap), vertexColor);
        float depthOffset = ApplyPerPixelDisplacement(viewDirTS, viewDirWS, blendMasks, input.positionWS, layerTexCoord);

        #ifdef _DEPTHOFFSET
            outputDepth = depthOffset;
        #endif
    #endif

    SurfaceData surfaceData;
    InitializeSurfaceData(layerTexCoord, vertexColor, surfaceData);
    
    #ifdef _DOUBLESIDED_ON
        ApplyDoubleSidedFlipOrMirror(faceSign, _DoubleSidedConstants.xyz, surfaceData.normalTS);
    #endif
    
    #ifdef _WEATHER_ON
        ApplyWeather(input.positionWS, input.normalWS.xyz, input.uv, 
                        surfaceData.albedo, surfaceData.normalTS, surfaceData.smoothness);
    #endif

    #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
        GeometricAAFiltering(input.normalWS.xyz, _SpecularAAScreenSpaceVariance, _SpecularAAThreshold, surfaceData.smoothness);
    #endif

    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(input.positionCS);
    #endif

    InputData inputData;
    VectorsData vData;
    InitializeInputData(input, surfaceData, vData, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

    #ifdef _EMISSION_FRESNEL
        half NoV = saturate(dot(vData.normalWS, vData.viewDirectionWS));
        half fresnelTerm = pow(1.0 - NoV, _EmissionFresnelPower);
        surfaceData.emission *= fresnelTerm;
    #endif

    #ifdef _DBUFFER
        ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
    #endif

    half4 color = ComplexLitFragment(inputData, surfaceData, vData);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, IsSurfaceTypeTransparent(_Surface));

    outColor = color;

    #ifdef _WRITE_RENDERING_LAYERS
        uint renderingLayers = GetMeshRenderingLayer();
        outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
    #endif
}

#endif