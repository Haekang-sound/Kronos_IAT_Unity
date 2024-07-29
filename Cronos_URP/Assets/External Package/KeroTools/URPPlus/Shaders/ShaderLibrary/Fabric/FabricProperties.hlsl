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

float4 _BaseMap_ST;
half4 _BaseColor;
half _AlphaRemapMin;
half _AlphaRemapMax;
half4 _SpecularColor;
half _Anisotropy;
half _Smoothness;
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

float4 _ThreadMap_ST;
half _ThreadAOScale;
half _ThreadNormalScale;
half _ThreadSmoothnessScale;
half _FuzzScale;
half _FuzzIntensity;

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

half4 _DiffusionColor;
half _Thickness;
half4 _ThicknessRemap;
half _TranslucencyScale;
half _TranslucencyPower;
half _TranslucencyAmbient;
half _TranslucencyDistortion;
half _TranslucencyShadows;

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

    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _SpecularColor)
    UNITY_DOTS_INSTANCED_PROP(float , _Anisotropy)
    UNITY_DOTS_INSTANCED_PROP(float , _Smoothness)
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

    UNITY_DOTS_INSTANCED_PROP(float4 , _ScatteringColor)
    UNITY_DOTS_INSTANCED_PROP(float , _Thickness)
    UNITY_DOTS_INSTANCED_PROP(float4, _ThicknessRemap)
    UNITY_DOTS_INSTANCED_PROP(float , _TranslucencyPower)
    UNITY_DOTS_INSTANCED_PROP(float , _TranslucencyAmbient)
    UNITY_DOTS_INSTANCED_PROP(float , _TranslucencyDistortion)
    UNITY_DOTS_INSTANCED_PROP(float , _TranslucencyShadows)

    UNITY_DOTS_INSTANCED_PROP(float , _ThreadAOScale)
    UNITY_DOTS_INSTANCED_PROP(float , _ThreadNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float , _ThreadSmoothnessScale)
    UNITY_DOTS_INSTANCED_PROP(float , _FuzzScale)
    UNITY_DOTS_INSTANCED_PROP(float , _FuzzIntensity)
    
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

#define _BaseMap_ST                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseMap_ST)
#define _BaseColor                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
#define _SpecularColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _SpecularColor)
#define _Anisotropy                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Anisotropy)
#define _Smoothness                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Smoothness)
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

#define _ThreadAOScale                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ThreadAOScale)
#define _ThreadNormalScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ThreadNormalScale)
#define _ThreadSmoothnessScale              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ThreadSmoothnessScale)
#define _FuzzScale                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _FuzzScale)
#define _FuzzIntensity                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _FuzzIntensity)

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

#define _ScatteringColor                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _ScatteringColor)
#define _Thickness                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Thickness)
#define _ThicknessRemap                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _ThicknessRemap)
#define _TranslucencyPower                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TranslucencyPower)
#define _TranslucencyAmbient                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TranslucencyAmbient)
#define _TranslucencyDistortion             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TranslucencyDistortion)
#define _TranslucencyShadows                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TranslucencyShadows)

#define _EmissionColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _EmissionColor)
#define _EmissionScale                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _EmissionScale)
#define _EmissionFresnelPower               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _EmissionFresnelPower)

#define _HorizonFade                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _HorizonFade)
#define _GIOcclusionBias                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _GIOcclusionBias)
#endif