// NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
CBUFFER_START(UnityPerMaterial)
half _Surface;
half4 _DoubleSidedConstants;
half _AlphaCutoff;
half _AlphaCutoffShadow;
half _SpecularAAScreenSpaceVariance;
half _SpecularAAThreshold;

half _SunSensitivity;
half _LightSensitivity;
half _PupilFactorMin;
half _PupilFactorMax;

float4 _BaseMap_ST;
half4 _BaseColor;
half _AlphaRemapMin;
half _AlphaRemapMax;

half _ScleraNormalScale;
half _IrisNormalScale;
half4 _IrisClampColor;
half _PupilRadius;
half _PupilAperture;
half _MinimalPupilAperture;
half _MaximalPupilAperture;
half _ScleraSmoothness;
half _CorneaSmoothness;
half _IrisOffset;

half _LimbalRingSizeIris;
half _LimbalRingSizeSclera;
half _LimbalRingFade;
half _LimbalRingIntensity;

half4 _EmissionColor;
half _EmissionScale;

half _MeshScale;
half4 _PositionOffset;
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

    UNITY_DOTS_INSTANCED_PROP(float , _SunSensitivity)
    UNITY_DOTS_INSTANCED_PROP(float , _LightSensitivity)
    UNITY_DOTS_INSTANCED_PROP(float , _PupilFactorMin)
    UNITY_DOTS_INSTANCED_PROP(float , _PupilFactorMax)

    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _SpecularColor)
    UNITY_DOTS_INSTANCED_PROP(float , _ScleraNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float , _IrisNormalScale)
    UNITY_DOTS_INSTANCED_PROP(float4, _IrisClampColor)
    UNITY_DOTS_INSTANCED_PROP(float , _PupilRadius)
    UNITY_DOTS_INSTANCED_PROP(float , _PupilAperture)
    UNITY_DOTS_INSTANCED_PROP(float , _MinimalPupilAperture)
    UNITY_DOTS_INSTANCED_PROP(float , _MaximalPupilAperture)
    UNITY_DOTS_INSTANCED_PROP(float , _ScleraSmoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _CorneaSmoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _IrisOffset)
    UNITY_DOTS_INSTANCED_PROP(float , _LimbalRingSizeIris)
    UNITY_DOTS_INSTANCED_PROP(float , _LimbalRingSizeSclera)
    UNITY_DOTS_INSTANCED_PROP(float , _LimbalRingFade)
    UNITY_DOTS_INSTANCED_PROP(float , _LimbalRingIntensity)

    UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
    UNITY_DOTS_INSTANCED_PROP(float , _EmissionScale)
    
    UNITY_DOTS_INSTANCED_PROP(float, _MeshScale)
    UNITY_DOTS_INSTANCED_PROP(float4, _PositionOffset)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _Surface                            UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _Surface)
#define _DoubleSidedConstants               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _DoubleSidedConstants)
#define _AlphaCutoff                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoff)
#define _AlphaCutoffShadow                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaCutoffShadow)
#define _SpecularAAThreshold                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAThreshold)
#define _SpecularAAScreenSpaceVariance      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SpecularAAScreenSpaceVariance)

#define _SunSensitivity                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _SunSensitivity)
#define _LightSensitivity                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _LightSensitivity)
#define _PupilFactorMin                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PupilFactorMin)
#define _PupilFactorMax                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PupilFactorMax)

#define _BaseMap_ST                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseMap_ST)
#define _BaseColor                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
#define _AlphaRemapMin                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMin)
#define _AlphaRemapMax                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _AlphaRemapMax)

#define _ScleraNormalScale                  UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ScleraNormalScale)
#define _IrisNormalScale                    UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _IrisNormalScale)

#define _IrisClampColor                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _IrisClampColor)
#define _PupilRadius                        UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PupilRadius)
#define _PupilAperture                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _PupilAperture)
#define _MinimalPupilAperture               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _MinimalPupilAperture)
#define _MaximalPupilAperture               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _MaximalPupilAperture)
#define _ScleraSmoothness                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _ScleraSmoothness)
#define _CorneaSmoothness                   UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _CorneaSmoothness)
#define _IrisOffset                         UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _IrisOffset)

#define _LimbalRingSizeIris                 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _LimbalRingSizeIris)
#define _LimbalRingSizeSclera               UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _LimbalRingSizeSclera)
#define _LimbalRingFade                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _LimbalRingFade)
#define _LimbalRingIntensity                UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _LimbalRingIntensity)

#define _EmissionColor                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _EmissionColor)
#define _EmissionScale                      UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _EmissionScale)

#define _MeshScale                          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float  , _MeshScale)
#define _PositionOffset                     UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _PositionOffset)
#endif