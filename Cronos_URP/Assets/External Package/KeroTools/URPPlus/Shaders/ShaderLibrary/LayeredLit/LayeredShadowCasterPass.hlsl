#ifndef UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
#define UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif
#include "ShaderLibrary/LayeredLit/LayeredDisplacement.hlsl"

#if !defined(SHADER_API_GLES)
    #define UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS 1
#endif

#if defined(_SURFACE_TYPE_TRANSPARENT) && defined(UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS)
    #define USE_DITHER_MASK 1
#endif

#ifdef USE_DITHER_MASK
    TEXTURE3D(_DitherMaskLOD); SAMPLER(sampler_DitherMaskLOD);
#endif

#if defined(_LAYER_MASK_VERTEX_COLOR_MUL) || (_LAYER_MASK_VERTEX_COLOR_ADD)
    #define VERTEX_COLOR
#endif

// Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
// For Directional lights, _LightDirection is used when applying shadow Normal Bias.
// For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
float3 _LightDirection;
float3 _LightPosition;

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
};

float4 GetShadowPositionHClip(Attributes input)
{
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

    half4 vertexColor = half4(1.0, 1.0, 1.0, 1.0);
    #if defined(VERTEX_COLOR)
        vertexColor = input.vertexColor;
    #endif

    #ifdef _VERTEX_DISPLACEMENT
        LayerTexCoord layerTexCoord;
        InitializeTexCoordinates(input.texcoord, layerTexCoord);

        half3 positionRWS = TransformObjectToWorld(input.positionOS.xyz);
        half3 height = ComputePerVertexDisplacement(layerTexCoord, vertexColor, 1);
        positionRWS += normalWS * height;
        input.positionOS = mul(unity_WorldToObject, half4(positionRWS, 1.0));
    #endif

    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);

    #if _CASTING_PUNCTUAL_LIGHT_SHADOW
        float3 lightDirectionWS = normalize(_LightPosition - positionWS);
    #else
        float3 lightDirectionWS = _LightDirection;
    #endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

    #if UNITY_REVERSED_Z
        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #else
        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #endif

    return positionCS;
}

#ifndef TESSELLATION_ON
Varyings ShadowPassVertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionCS = GetShadowPositionHClip(input);
    #if defined(VERTEX_COLOR)
    output.vertexColor = input.vertexColor;
    #endif

    return output;
}
#else
TessellationControlPoint ShadowPassVertex(Attributes input)
{
    TessellationControlPoint output = (TessellationControlPoint)0;
    UNITY_SETUP_INSTANCE_ID(input);

    output.texcoord = input.texcoord;
    output.normalOS = input.normalOS;
    output.positionOS = input.positionOS;
    #if defined(VERTEX_COLOR)
    output.vertexColor = input.vertexColor;
    #endif

    return output;
}

[domain("tri")]
Varyings ShadowDomain(TessellationFactors tessFactors, const OutputPatch<TessellationControlPoint, 3> input, float3 baryCoords : SV_DomainLocation)
{
    Varyings output = (Varyings)0;
    Attributes data = (Attributes)0;

    UNITY_SETUP_INSTANCE_ID(input[0]);

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

    output.positionCS = TransformWorldToHClip(ApplyShadowBias(vertexInput.positionWS, normalInput.normalWS, _LightDirection));
    #if UNITY_REVERSED_Z
        output.positionCS.z = min(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
    #else
        output.positionCS.z = max(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
    #endif

    return output;
}
#endif

half4 ShadowPassFragment(Varyings input) : SV_TARGET
{
    half shadowCutoff = _AlphaCutoff;
    #ifdef _SHADOW_CUTOFF
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
    half alpha = Alpha(alphasBlend, shadowCutoff);

    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(input.positionCS);
    #endif

    #ifdef USE_DITHER_MASK
        half alphaRef = SAMPLE_TEXTURE3D(_DitherMaskLOD, sampler_DitherMaskLOD, float3(input.positionCS.xy * 0.25, alpha * 0.9375)).a;
        clip (alphaRef - 0.01);
    #endif

    return 0;
}

#endif
