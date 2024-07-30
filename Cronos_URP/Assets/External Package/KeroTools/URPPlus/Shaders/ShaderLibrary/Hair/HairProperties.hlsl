// NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
CBUFFER_START(UnityPerMaterial)
half _Surface;
half4 _DoubleSidedConstants;
half _AlphaCutoff;
half _AlphaCutoffShadow;
half _SpecularAAScreenSpaceVariance;
half _SpecularAAThreshold;

float4 _BaseMap_ST;
half4 _BaseColor;
half _AlphaRemapMin;
half _AlphaRemapMax;
half _NormalScale;

half _AORemapMin;
half _AORemapMax;

float4 _SmoothnessMaskMap_ST;
half _Smoothness;
half _SmoothnessRemapMin;
half _SmoothnessRemapMax;

half4 _SpecularColor;
half _SpecularMultiplier;
half _SpecularShift;
half _SecondarySpecularMultiplier;
half _SecondarySpecularShift;

half4 _TransmissionColor;
half _TransmissionIntensity;

half4 _StaticLightColor;
half4 _StaticLightVector;
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

    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _AlphaRemapMax)
    UNITY_DOTS_INSTANCED_PROP(float , _NormalScale)

    UNITY_DOTS_INSTANCED_PROP(float , _AORemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _AORemapMax)

    UNITY_DOTS_INSTANCED_PROP(float , _Smoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _SmoothnessRemapMin)
    UNITY_DOTS_INSTANCED_PROP(float , _SmoothnessRemapMax)
    
    UNITY_DOTS_INSTANCED_PROP(float4, _SpecularColor)
    UNITY_DOTS_INSTANCED_PROP(float , _SpecularMultiplier)
    UNITY_DOTS_INSTANCED_PROP(float , _SpecularShift)
    UNITY_DOTS_INSTANCED_PROP(float , _SecondarySpecularMultiplier)
    UNITY_DOTS_INSTANCED_PROP(float , _SecondarySpecularShift)

    UNITY_DOTS_INSTANCED_PROP(float4, _TransmissionColor)
    UNITY_DOTS_INSTANCED_PROP(float , _TransmissionIntensity)
    
    UNITY_DOTS_INSTANCED_PROP(float4, _StaticLightColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _StaticLightVector)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _Surface                            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Surface)
#define _DoubleSidedConstants               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _DoubleSidedConstants)
#define _AlphaCutoff                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoff)
#define _AlphaCutoffShadow                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoffShadow)
#define _SpecularAAThreshold                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAThreshold)
#define _SpecularAAScreenSpaceVariance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAScreenSpaceVariance)

#define _BaseColor                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
#define _AlphaRemapMin                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMin)
#define _AlphaRemapMax                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMax)
#define _NormalScale                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _NormalScale)

#define _AORemapMin                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AORemapMin)
#define _AORemapMax                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AORemapMax)

#define _Smoothness                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Smoothness)
#define _SmoothnessRemapMin                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SmoothnessRemapMin)
#define _SmoothnessRemapMax                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SmoothnessRemapMax)

#define _SpecularColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _SpecularColor)
#define _SpecularMultiplier                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularMultiplier)
#define _SpecularShift                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularShift)
#define _SecondarySpecularMultiplier        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SecondarySpecularMultiplier)
#define _SecondarySpecularShift             UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SecondarySpecularShift)

#define _TransmissionColor                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _TransmissionColor)
#define _TransmissionIntensity              UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _TransmissionIntensity)

#define _StaticLightColor                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _StaticLightColor)
#define _StaticLightVector                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _StaticLightVector)
#endif