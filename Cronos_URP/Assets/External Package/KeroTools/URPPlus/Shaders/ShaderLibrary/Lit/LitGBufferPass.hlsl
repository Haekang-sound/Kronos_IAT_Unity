#ifndef UNIVERSAL_LIT_GBUFFER_PASS_INCLUDED
#define UNIVERSAL_LIT_GBUFFER_PASS_INCLUDED

#include "ShaderLibrary/LitLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LitDisplacement.hlsl"

#if defined(_NORMALMAP) || (_BENTNORMALMAP) || (_COATNORMALMAP) || (_DETAIL) || (_RAIN_NORMALMAP) || (_DOUBLESIDED_ON) || (_PIXEL_DISPLACEMENT)
    #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

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

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
    float3 positionWS : TEXCOORD1;
    #endif

    half3 normalWS : TEXCOORD2;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS : TEXCOORD3; // xyz: tangent, w: sign
    #endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
    half3 vertexLighting            : TEXCOORD4;    // xyz: vertex lighting
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD5;
    #endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);
    #ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV       : TEXCOORD8; // Dynamic lightmap UVs
    #endif

    float4 positionCS : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, SurfaceData surfaceData, out VectorsData vData, out InputData inputData)
{
    inputData = (InputData)0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
        inputData.positionWS = input.positionWS;
    #endif

    inputData.positionCS = input.positionCS;
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sgn = input.tangentWS.w; // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        inputData.tangentToWorld = tangentToWorld;
    #endif

    inputData.normalWS = input.normalWS;
    #if defined(_NORMALMAP) || (_DETAIL) || (_RAIN_NORMALMAP) || (_DOUBLESIDED_ON)
    inputData.normalWS = TransformTangentToWorld(surfaceData.normalTS, tangentToWorld);
    #endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    inputData.viewDirectionWS = viewDirWS;

    half3 coatNormalWS = inputData.normalWS;
    #if defined(_COATNORMALMAP)
    coatNormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(surfaceData.coatNormalTS, tangentToWorld));
    #endif

    half3 bentNormalWS = inputData.normalWS;
    #if defined(_BENTNORMALMAP)
    bentNormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(surfaceData.bentNormalTS, tangentToWorld));
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        inputData.shadowCoord = input.shadowCoord;
    #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
        inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
    #else
        inputData.shadowCoord = float4(0, 0, 0, 0);
    #endif

    inputData.fogCoord = 0.0; // we don't apply fog in the guffer pass

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        inputData.vertexLighting = input.vertexLighting.xyz;
    #else
        inputData.vertexLighting = half3(0, 0, 0);
    #endif

    #if defined(DYNAMICLIGHTMAP_ON)
        inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
    #else
        inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
    #endif

    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

    vData = CreateVectorsData(input.normalWS.xyz, inputData.normalWS, bentNormalWS, coatNormalWS, viewDirWS, 0.0);
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

#ifndef TESSELLATION_ON
Varyings LitGBufferPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    #ifdef _VERTEX_DISPLACEMENT
        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        positionRWS += normalInput.normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

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
        half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
        output.vertexLighting = vertexLight;
    #endif

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
        output.positionWS = vertexInput.positionWS;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

    output.positionCS = vertexInput.positionCS;

    return output;
}
#else
TessellationControlPoint LitGBufferPassVertex(Attributes input)
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

    output.uv = TRANSFORM_TEX(data.texcoord, _BaseMap);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(data.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(data.normalOS, data.tangentOS);

    #ifdef _TESSELLATION_DISPLACEMENT
        half3 height = ComputePerVertexDisplacement(_HeightMap, sampler_HeightMap, output.uv, 1);
        vertexInput.positionWS += normalInput.normalWS * height;
    #endif

    output.normalWS = normalInput.normalWS;
    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR) || (_WEATHER_ON)
        output.positionWS = vertexInput.positionWS;
    #endif

    output.positionCS = TransformWorldToHClip(vertexInput.positionWS);

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        float sign = data.tangentOS.w * GetOddNegativeScale();
        output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
    #endif

    OUTPUT_LIGHTMAP_UV(data.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
    #ifdef DYNAMICLIGHTMAP_ON
        output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif
    OUTPUT_SH(output.normalWS, output.vertexSH);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
        output.vertexLighting = vertexLight;
    #endif

    return output;
}
#endif

// Used in Standard (Physically Based) shader
FragmentOutput LitGBufferPassFragment(
    Varyings input
    , half faceSign : VFACE
    #ifdef _DEPTHOFFSET
    , out float outputDepth : SV_Depth
    #endif
)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    #ifdef _PIXEL_DISPLACEMENT
        half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
        half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);

        float depthOffset = ApplyPerPixelDisplacement(viewDirTS, viewDirWS, input.positionWS, input.uv);
        
        #ifdef _DEPTHOFFSET
            outputDepth = depthOffset;
        #endif
    #endif
    
    SurfaceData surfaceData;
    InitializeSurfaceData(input.uv, surfaceData);

    #ifdef _DOUBLESIDED_ON
        ApplyDoubleSidedFlipOrMirror(faceSign, _DoubleSidedConstants.xyz, surfaceData.normalTS);
    #endif

    #ifdef _WEATHER_ON
        ApplyWeather(input.positionWS, input.normalWS.xyz, input.uv, 
                        surfaceData.albedo, surfaceData.normalTS, surfaceData.smoothness);
    #endif

    #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
        GeometricAAFiltering(input.normalWS.xyz, _SpecularAAScreenSpaceVariance, _SpecularAAThreshold, 
                                surfaceData.smoothness, surfaceData.clearCoatSmoothness);
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

    // in LitForwardPass GlobalIllumination (and temporarily LightingPhysicallyBased) are called inside UniversalFragmentPBR
    // in Deferred rendering we store the sum of these values (and of emission as well) in the GBuffer
    BRDFData brdfData;
    InitializeBRDFData(inputData, surfaceData, brdfData);

    BRDFData brdfDataClearCoat = CreateClearCoatBRDFData(surfaceData, brdfData);

    Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, inputData.shadowMask);

    half3 indirectSpecular, coatIndirectSpecular;
    ComputeIndirectSpecular(vData, inputData.positionWS, inputData.normalizedScreenSpaceUV, 
                            brdfData.perceptualRoughness, brdfDataClearCoat.perceptualRoughness, 
                            surfaceData.anisotropy, indirectSpecular, coatIndirectSpecular);
                            
    half3 color = ComplexGlobalIllumination(surfaceData, brdfData, brdfDataClearCoat, vData, inputData.bakedGI, 
                                            indirectSpecular, coatIndirectSpecular, surfaceData.occlusion);  

    return BRDFDataToGbuffer(brdfData, inputData, surfaceData.smoothness, surfaceData.emission + color,
                                surfaceData.occlusion);
}

#endif
