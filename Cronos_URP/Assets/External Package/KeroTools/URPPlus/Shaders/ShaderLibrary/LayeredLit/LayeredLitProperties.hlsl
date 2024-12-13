// NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
CBUFFER_START(UnityPerMaterial)
half _Surface;
half4 _DoubleSidedConstants;
half _AlphaCutoff;
half _AlphaCutoffShadow;
half _SpecularAAScreenSpaceVariance;
half _SpecularAAThreshold;

half _PPDMinSamples;
half _PPDMaxSamples;
half _PPDLodThreshold;
half4 _InvPrimScale;
half _InvTilingScale;
half _InvTilingScale1;
half _InvTilingScale2;
half _InvTilingScale3;

half _TessellationFactor;
half _TessellationEdgeLength;
half _TessellationFactorMinDistance;
half _TessellationFactorMaxDistance;
half _TessellationShapeFactor;
half _TessellationBackFaceCullEpsilon;

float4 _LayerMaskMap_ST;
half _HeightTransition;
half _LayerCount;

half _OpacityAsDensity;
half _OpacityAsDensity1;
half _OpacityAsDensity2;
half _OpacityAsDensity3;

float4 _BaseMap_ST;
float4 _BaseMap1_ST;
float4 _BaseMap2_ST;
float4 _BaseMap3_ST;
half4 _BaseColor;
half4 _BaseColor1;
half4 _BaseColor2;
half4 _BaseColor3;
half _InheritBaseColor1;
half _InheritBaseColor2;
half _InheritBaseColor3;

half _AlphaRemapMin;
half _AlphaRemapMin1;
half _AlphaRemapMin2;
half _AlphaRemapMin3;
half _AlphaRemapMax;
half _AlphaRemapMax1;
half _AlphaRemapMax2;
half _AlphaRemapMax3;

half _Metallic;
half _Metallic1;
half _Metallic2;
half _Metallic3;
half _MetallicRemapMin;
half _MetallicRemapMin1;
half _MetallicRemapMin2;
half _MetallicRemapMin3;
half _MetallicRemapMax;
half _MetallicRemapMax1;
half _MetallicRemapMax2;
half _MetallicRemapMax3;

half _Smoothness;
half _Smoothness1;
half _Smoothness2;
half _Smoothness3;
half _SmoothnessRemapMin;
half _SmoothnessRemapMin1;
half _SmoothnessRemapMin2;
half _SmoothnessRemapMin3;
half _SmoothnessRemapMax;
half _SmoothnessRemapMax1;
half _SmoothnessRemapMax2;
half _SmoothnessRemapMax3;

half _AORemapMin;
half _AORemapMin1;
half _AORemapMin2;
half _AORemapMin3;
half _AORemapMax;
half _AORemapMax1;
half _AORemapMax2;
half _AORemapMax3;

half _NormalScale; 
half _NormalScale1; 
half _NormalScale2; 
half _NormalScale3;
half _InheritBaseNormal1; 
half _InheritBaseNormal2; 
half _InheritBaseNormal3;

half _HeightAmplitude;
half _HeightAmplitude1;
half _HeightAmplitude2;
half _HeightAmplitude3;
half _HeightCenter;
half _HeightCenter1;
half _HeightCenter2;
half _HeightCenter3;
half _HeightPoMAmplitude;
half _HeightPoMAmplitude1;
half _HeightPoMAmplitude2;
half _HeightPoMAmplitude3;
half _InheritBaseHeight1;
half _InheritBaseHeight2;
half _InheritBaseHeight3;

float4 _DetailMap_ST;
float4 _DetailMap1_ST;
float4 _DetailMap2_ST;
float4 _DetailMap3_ST;
half _DetailAlbedoScale;
half _DetailAlbedoScale1;
half _DetailAlbedoScale2;
half _DetailAlbedoScale3;
half _DetailNormalScale;
half _DetailNormalScale1;
half _DetailNormalScale2;
half _DetailNormalScale3;
half _DetailSmoothnessScale;
half _DetailSmoothnessScale1;
half _DetailSmoothnessScale2;
half _DetailSmoothnessScale3;

half4 _PuddlesFramesSize;
half _PuddlesNormalScale;
half _PuddlesSize;
half _PuddlesAnimationSpeed;

half _RainNormalScale;
half _RainSize;
half _RainAnimationSpeed;
half _RainDistortionScale;
half _RainDistortionSize;
half _RainWetnessFactor;

float4 _EmissionMap_ST;
half4 _EmissionColor;
half _EmissionScale;
half _EmissionFresnelPower;

half _HorizonFade;
half _GIOcclusionBias;
CBUFFER_END

// NOTE: Do not ifdef the properties for dots instancing, but ifdef the actual usage.
// Otherwise you might break CPU-side as property constant-buffer offsets change per variant.
// NOTE: Dots instancing is orthogonal to the constant buffer above.
#ifdef UNITY_DOTS_INSTANCING_ENABLED

UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
    UNITY_DOTS_INSTANCED_PROP(float, _Surface)
    UNITY_DOTS_INSTANCED_PROP(float4, _DoubleSidedConstants)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaCutoff)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaCutoffShadow)
    UNITY_DOTS_INSTANCED_PROP(float, _SpecularAAScreenSpaceVariance)
    UNITY_DOTS_INSTANCED_PROP(float, _SpecularAAThreshold)

    UNITY_DOTS_INSTANCED_PROP(float, _PPDMinSamples)
    UNITY_DOTS_INSTANCED_PROP(float, _PPDMaxSamples)
    UNITY_DOTS_INSTANCED_PROP(float, _PPDLodThreshold)
    UNITY_DOTS_INSTANCED_PROP(float4, _InvPrimScale)
    UNITY_DOTS_INSTANCED_PROP(float, _InvTilingScale)
    UNITY_DOTS_INSTANCED_PROP(float, _InvTilingScale1)
    UNITY_DOTS_INSTANCED_PROP(float, _InvTilingScale2)
    UNITY_DOTS_INSTANCED_PROP(float, _InvTilingScale3)

    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactor)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationEdgeLength)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactorMinDistance)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactorMaxDistance)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationShapeFactor)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationBackFaceCullEpsilon)

    UNITY_DOTS_INSTANCED_PROP(float, _HeightTransition)
    UNITY_DOTS_INSTANCED_PROP(float, _LayerCount)

    UNITY_DOTS_INSTANCED_PROP(float, _OpacityAsDensity)
    UNITY_DOTS_INSTANCED_PROP(float, _OpacityAsDensity1)
    UNITY_DOTS_INSTANCED_PROP(float, _OpacityAsDensity2)
    UNITY_DOTS_INSTANCED_PROP(float, _OpacityAsDensity3)

    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor1)
    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor2)
    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor3)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseColor1)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseColor2)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseColor3)

    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMin1)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMin2)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMin3)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMax1)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMax2)
    UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMax3)

    UNITY_DOTS_INSTANCED_PROP(float, _Metallic)
    UNITY_DOTS_INSTANCED_PROP(float, _Metallic1)
    UNITY_DOTS_INSTANCED_PROP(float, _Metallic2)
    UNITY_DOTS_INSTANCED_PROP(float, _Metallic3)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMin1)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMin2)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMin3)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMax1)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMax2)
    UNITY_DOTS_INSTANCED_PROP(float, _MetallicRemapMax3)

    UNITY_DOTS_INSTANCED_PROP(float, _Smoothness)
    UNITY_DOTS_INSTANCED_PROP(float, _Smoothness1)
    UNITY_DOTS_INSTANCED_PROP(float, _Smoothness2)
    UNITY_DOTS_INSTANCED_PROP(float, _Smoothness3)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMin1)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMin2)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMin3)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMax1)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMax2)
    UNITY_DOTS_INSTANCED_PROP(float, _SmoothnessRemapMax3)

    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMin)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMin1)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMin2)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMin3)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMax)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMax1)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMax2)
    UNITY_DOTS_INSTANCED_PROP(float, _AORemapMax3)

    UNITY_DOTS_INSTANCED_PROP(float, _NormalScale)
    UNITY_DOTS_INSTANCED_PROP(float, _NormalScale1)
    UNITY_DOTS_INSTANCED_PROP(float, _NormalScale2)
    UNITY_DOTS_INSTANCED_PROP(float, _NormalScale3)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseNormal1)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseNormal2)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseNormal3)

    UNITY_DOTS_INSTANCED_PROP(float, _HeightAmplitude)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightAmplitude1)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightAmplitude2)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightAmplitude3)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightCenter)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightCenter1)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightCenter2)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightCenter3)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightPoMAmplitude)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightPoMAmplitude1)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightPoMAmplitude2)
    UNITY_DOTS_INSTANCED_PROP(float, _HeightPoMAmplitude3)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseHeight1)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseHeight2)
    UNITY_DOTS_INSTANCED_PROP(float, _InheritBaseHeight3)

    UNITY_DOTS_INSTANCED_PROP(float, _DetailAlbedoScale)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailAlbedoScale1)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailAlbedoScale2)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailAlbedoScale3)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailNormalScale1)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailNormalScale2)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailNormalScale3)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailSmoothnessScale)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailSmoothnessScale1)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailSmoothnessScale2)
    UNITY_DOTS_INSTANCED_PROP(float, _DetailSmoothnessScale3)

    UNITY_DOTS_INSTANCED_PROP(float4, _PuddlesFramesSize)
    UNITY_DOTS_INSTANCED_PROP(float , _PuddlesNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float , _PuddlesSize)
    UNITY_DOTS_INSTANCED_PROP(float , _PuddlesAnimationSpeed)

    UNITY_DOTS_INSTANCED_PROP(float , _RainNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float , _RainSize)
    UNITY_DOTS_INSTANCED_PROP(float , _RainAnimationSpeed)
    UNITY_DOTS_INSTANCED_PROP(float , _RainDistortionScale)
    UNITY_DOTS_INSTANCED_PROP(float , _RainDistortionSize)
    UNITY_DOTS_INSTANCED_PROP(float , _RainWetnessFactor)

    UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
    UNITY_DOTS_INSTANCED_PROP(float, _EmissionScale)
    UNITY_DOTS_INSTANCED_PROP(float, _EmissionFresnelPower)

    UNITY_DOTS_INSTANCED_PROP(float, _HorizonFade)
    UNITY_DOTS_INSTANCED_PROP(float, _GIOcclusionBias)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _Surface                            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Surface)
#define _DoubleSidedConstants               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _DoubleSidedConstants)
#define _AlphaCutoff                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaCutoff)
#define _AlphaCutoffShadow                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaCutoffShadow)
#define _SpecularAAThreshold                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SpecularAAThreshold)
#define _SpecularAAScreenSpaceVariance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SpecularAAScreenSpaceVariance)

#define _PPDMinSamples                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _PPDMinSamples)
#define _PPDMaxSamples                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _PPDMaxSamples)
#define _PPDLodThreshold                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _PPDLodThreshold)
#define _InvPrimScale                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _InvPrimScale)
#define _InvTilingScale                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InvTilingScale)
#define _InvTilingScale1                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InvTilingScale)
#define _InvTilingScale2                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InvTilingScale)
#define _InvTilingScale3                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InvTilingScale)

#define _TessellationFactor                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactor)
#define _TessellationEdgeLength             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationEdgeLength)
#define _TessellationFactorMinDistance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactorMinDistance)
#define _TessellationFactorMaxDistance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactorMaxDistance)
#define _TessellationShapeFactor            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationShapeFactor)
#define _TessellationBackFaceCullEpsilon    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationBackFaceCullEpsilon)

#define _HeightTransition                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightTransition)
#define _LayerCount                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _LayerCount)

#define _OpacityAsDensity                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _OpacityAsDensity)
#define _OpacityAsDensity1                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _OpacityAsDensity1)
#define _OpacityAsDensity2                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _OpacityAsDensity2)
#define _OpacityAsDensity3                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _OpacityAsDensity3)

#define _BaseColor                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor)
#define _BaseColor1                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor1)
#define _BaseColor2                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor2)
#define _BaseColor3                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor3)
#define _InheritBaseColor1                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseColor1)
#define _InheritBaseColor2                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseColor2)
#define _InheritBaseColor3                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseColor3)

#define _AlphaRemapMin                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMin)
#define _AlphaRemapMin1                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMin1)
#define _AlphaRemapMin2                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMin2)
#define _AlphaRemapMin3                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMin3)
#define _AlphaRemapMax                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMax)
#define _AlphaRemapMax1                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMax1)
#define _AlphaRemapMax2                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMax2)
#define _AlphaRemapMax3                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMax3)

#define _Metallic                           UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Metallic)
#define _Metallic1                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Metallic1)
#define _Metallic2                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Metallic2)
#define _Metallic3                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Metallic3)
#define _MetallicRemapMin                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMin)
#define _MetallicRemapMin1                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMin1)
#define _MetallicRemapMin2                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMin2)
#define _MetallicRemapMin3                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMin3)
#define _MetallicRemapMax                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMax)
#define _MetallicRemapMax1                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMax1)
#define _MetallicRemapMax2                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMax2)
#define _MetallicRemapMax3                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _MetallicRemapMax3)

#define _Smoothness                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Smoothness)
#define _Smoothness1                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Smoothness1)
#define _Smoothness2                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Smoothness2)
#define _Smoothness3                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Smoothness3)
#define _SmoothnessRemapMin                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMin)
#define _SmoothnessRemapMin1                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMin1)
#define _SmoothnessRemapMin2                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMin2)
#define _SmoothnessRemapMin3                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMin3)
#define _SmoothnessRemapMax                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMax)
#define _SmoothnessRemapMax1                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMax1)
#define _SmoothnessRemapMax2                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMax2)
#define _SmoothnessRemapMax3                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _SmoothnessRemapMax3)

#define _AORemapMin                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMin)
#define _AORemapMin1                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMin1)
#define _AORemapMin2                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMin2)
#define _AORemapMin3                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMin3)
#define _AORemapMax                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMax)
#define _AORemapMax1                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMax1)
#define _AORemapMax2                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMax2)
#define _AORemapMax3                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AORemapMax3)

#define _NormalScale                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _NormalScale)
#define _NormalScale1                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _NormalScale1)
#define _NormalScale2                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _NormalScale2)
#define _NormalScale3                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _NormalScale3)
#define _InheritBaseNormal1                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseNormal1)
#define _InheritBaseNormal2                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseNormal2)
#define _InheritBaseNormal3                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseNormal3)

#define _HeightAmplitude                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightAmplitude)
#define _HeightAmplitude1                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightAmplitude1)
#define _HeightAmplitude2                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightAmplitude2)
#define _HeightAmplitude3                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightAmplitude3)
#define _HeightCenter                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightCenter)
#define _HeightCenter1                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightCenter1)
#define _HeightCenter2                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightCenter2)
#define _HeightCenter3                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightCenter3)
#define _HeightPoMAmplitude                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightPoMAmplitude)
#define _HeightPoMAmplitude1                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightPoMAmplitude1)
#define _HeightPoMAmplitude2                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightPoMAmplitude2)
#define _HeightPoMAmplitude3                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HeightPoMAmplitude3)
#define _InheritBaseHeight1                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseHeight1)
#define _InheritBaseHeight2                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseHeight2)
#define _InheritBaseHeight3                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _InheritBaseHeight3)

#define _DetailAlbedoScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailAlbedoScale)
#define _DetailAlbedoScale1                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailAlbedoScale1)
#define _DetailAlbedoScale2                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailAlbedoScale2)
#define _DetailAlbedoScale3                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailAlbedoScale3)
#define _DetailNormalScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailNormalScale)
#define _DetailNormalScale1                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailNormalScale1)
#define _DetailNormalScale2                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailNormalScale2)
#define _DetailNormalScale3                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailNormalScale3)
#define _DetailSmoothnessScale              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailSmoothnessScale)
#define _DetailSmoothnessScale1             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailSmoothnessScale1)
#define _DetailSmoothnessScale2             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailSmoothnessScale2)
#define _DetailSmoothnessScale3             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _DetailSmoothnessScale3)

#define _PuddlesFramesSize                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _PuddlesFramesSize)
#define _PuddlesNormalScale                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PuddlesNormalScale)
#define _PuddlesSize                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PuddlesSize)
#define _PuddlesAnimationSpeed              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PuddlesAnimationSpeed)

#define _RainNormalScale                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainNormalScale)
#define _RainSize                           UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainSize)
#define _RainAnimationSpeed                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainAnimationSpeed)
#define _RainDistortionScale                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainDistortionScale)
#define _RainDistortionSize                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainDistortionSize)
#define _RainWetnessFactor                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RainWetnessFactor)

#define _EmissionColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _EmissionColor)
#define _EmissionScale                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _EmissionScale)
#define _EmissionFresnelPower               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _EmissionFresnelPower)

#define _HorizonFade                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _HorizonFade)
#define _GIOcclusionBias                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _GIOcclusionBias)
#endif