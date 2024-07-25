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

half _TessellationFactor;
half _TessellationEdgeLength;
half _TessellationFactorMinDistance;
half _TessellationFactorMaxDistance;
half _TessellationShapeFactor;
half _TessellationBackFaceCullEpsilon;

float4 _BaseMap_ST;
half4 _BaseColor;
half _AlphaRemapMin;
half _AlphaRemapMax;
half4 _SpecularColor;
half _Metallic;
half _Smoothness;
half _MetallicRemapMin;
half _MetallicRemapMax;
half _SmoothnessRemapMin;
half _SmoothnessRemapMax;
half _AORemapMin;
half _AORemapMax;
half _NormalScale;

float4 _HeightMap_TexelSize;
half _HeightCenter;
half _HeightAmplitude;
half _HeightOffset;
half _HeightPoMAmplitude;

half _ClearCoatMask;
half _ClearCoatSmoothness;
half _CoatNormalScale;

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

half _Ior;
half4 _TransmittanceColor;
half _ChromaticAberration;
half _RefractionShadowAttenuation;

half4 _DetailMap_ST;
half _DetailAlbedoScale;
half _DetailNormalScale;
half _DetailSmoothnessScale;

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
    UNITY_DOTS_INSTANCED_PROP(float , _Surface)
    UNITY_DOTS_INSTANCED_PROP(float4, _DoubleSidedConstants)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaCutoff)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaCutoffShadow)
    UNITY_DOTS_INSTANCED_PROP(float , _SpecularAAScreenSpaceVariance)
    UNITY_DOTS_INSTANCED_PROP(float , _SpecularAAThreshold)

    UNITY_DOTS_INSTANCED_PROP(float , _PPDMinSamples)
    UNITY_DOTS_INSTANCED_PROP(float , _PPDMaxSamples)
    UNITY_DOTS_INSTANCED_PROP(float , _PPDLodThreshold)
    UNITY_DOTS_INSTANCED_PROP(float4, _InvPrimScale)
    UNITY_DOTS_INSTANCED_PROP(float, _InvTilingScale)

    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactor)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationEdgeLength)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactorMinDistance)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationFactorMaxDistance)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationShapeFactor)
    UNITY_DOTS_INSTANCED_PROP(float , _TessellationBackFaceCullEpsilon)

    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _SpecularColor)
    UNITY_DOTS_INSTANCED_PROP(float , _Metallic)
    UNITY_DOTS_INSTANCED_PROP(float , _Smoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _MetallicRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _MetallicRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float , _SmoothnessRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _SmoothnessRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float , _AORemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _AORemapMax)
    UNITY_DOTS_INSTANCED_PROP(float , _NormalScale)

    UNITY_DOTS_INSTANCED_PROP(float4, _HeightMap_TexelSize)
    UNITY_DOTS_INSTANCED_PROP(float , _HeightCenter)
    UNITY_DOTS_INSTANCED_PROP(float , _HeightAmplitude)
    UNITY_DOTS_INSTANCED_PROP(float , _HeightOffset)
    UNITY_DOTS_INSTANCED_PROP(float , _HeightPoMAmplitude)

    UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatMask)
    UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatSmoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _CoatNormalScale)

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

    UNITY_DOTS_INSTANCED_PROP(float , _Ior)
    UNITY_DOTS_INSTANCED_PROP(float4, _TransmittanceColor)
    UNITY_DOTS_INSTANCED_PROP(float , _ChromaticAberration)
    UNITY_DOTS_INSTANCED_PROP(float , _RefractionShadowAttenuation)

    UNITY_DOTS_INSTANCED_PROP(float , _DetailMap_ST)
    UNITY_DOTS_INSTANCED_PROP(float , _DetailAlbedoScale)
    UNITY_DOTS_INSTANCED_PROP(float , _DetailNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float , _DetailSmoothnessScale)

    UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
    UNITY_DOTS_INSTANCED_PROP(float , _EmissionScale)
    UNITY_DOTS_INSTANCED_PROP(float , _EmissionFresnelPower)
    
    UNITY_DOTS_INSTANCED_PROP(float , _HorizonFade)
    UNITY_DOTS_INSTANCED_PROP(float , _GIOcclusionBias)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _Surface                            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Surface)
#define _DoubleSidedConstants               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _DoubleSidedConstants)
#define _AlphaCutoff                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoff)
#define _AlphaCutoffShadow                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoffShadow)
#define _SpecularAAThreshold                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAThreshold)
#define _SpecularAAScreenSpaceVariance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAScreenSpaceVariance)

#define _PPDMinSamples                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PPDMinSamples)
#define _PPDMaxSamples                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PPDMaxSamples)
#define _PPDLodThreshold                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PPDLodThreshold)
#define _InvPrimScale                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _InvPrimScale)
#define _InvTilingScale                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _InvTilingScale)

#define _TessellationFactor                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactor)
#define _TessellationEdgeLength             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationEdgeLength)
#define _TessellationFactorMinDistance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactorMinDistance)
#define _TessellationFactorMaxDistance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationFactorMaxDistance)
#define _TessellationShapeFactor            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationShapeFactor)
#define _TessellationBackFaceCullEpsilon    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TessellationBackFaceCullEpsilon)

#define _BaseMap_ST                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseMap_ST)
#define _BaseColor                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
#define _SpecularColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _SpecularColor)
#define _Metallic                           UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Metallic)
#define _Smoothness                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Smoothness)
#define _MetallicRemapMin                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _MetallicRemapMin)
#define _MetallicRemapMax                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _MetallicRemapMax)
#define _SmoothnessRemapMin                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SmoothnessRemapMin)
#define _SmoothnessRemapMax                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SmoothnessRemapMax)
#define _AlphaRemapMin                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMin)
#define _AlphaRemapMax                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMax)
#define _AORemapMin                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AORemapMin)
#define _AORemapMax                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AORemapMax)
#define _NormalScale                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _NormalScale)

#define _HeightMap_TexelSize                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _HeightMap_TexelSize)
#define _HeightCenter                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HeightCenter)
#define _HeightAmplitude                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HeightAmplitude)
#define _HeightOffset                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HeightOffset)
#define _HeightPoMAmplitude                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HeightPoMAmplituder)

#define _ClearCoatMask                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ClearCoatMask)
#define _ClearCoatSmoothness                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ClearCoatSmoothness)
#define _CoatNormalScale                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _CoatNormalScale)

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

#define _Ior                                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Ior)
#define _TransmittanceColor                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _TransmittanceColor)
#define _ChromaticAberration                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ChromaticAberration)
#define _RefractionShadowAttenuation        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _RefractionShadowAttenuation)

#define _DetailMap_ST                       UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _DetailMap_ST)
#define _DetailAlbedoScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _DetailAlbedoScale)
#define _DetailNormalScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _DetailNormalScale)
#define _DetailSmoothnessScale              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _DetailSmoothnessScale)

#define _EmissionColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _EmissionColor)
#define _EmissionScale                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _EmissionScale)
#define _EmissionFresnelPower               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _EmissionFresnelPower)

#define _HorizonFade                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HorizonFade)
#define _GIOcclusionBias                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _GIOcclusionBias)
#endif