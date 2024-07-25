#ifndef UNIVERSAL_SURFACE_DATA_INCLUDED
#define UNIVERSAL_SURFACE_DATA_INCLUDED

struct SurfaceData
{
    half3 albedo;
    half3 specular;
    half metallic;
    half smoothness;
    half anisotropy;
    half occlusion;

    half3 normalTS;
    half3 bentNormalTS;
    half3 tangentTS;

    half clearCoatMask;
    half clearCoatSmoothness;
    half3 coatNormalTS;

    half3 iridescenceTMS; // Iridescence Thickness/Mask/Shift

    half thickness;
    half curvature;
    half3 diffusionColor;
    half transmissionScale;
    half translucencyPower;
    half translucencyScale;
    half translucencyAmbient;
    half translucencyDistortion;
    half translucencyShadows;
    half translucencyDiffuseInfluence;

    half ior;
    half3 transmittanceColor;
    half chromaticAberration;

    half3 emission;
    half alpha;

    half horizonFade;
    half giOcclusionBias;
};

SurfaceData EmptyFill()
{
    SurfaceData data = (SurfaceData)0;

    data.tangentTS = half3(1.0, 1.0, 0.0);

    data.occlusion = 1.0h;

    return data;
}

#endif