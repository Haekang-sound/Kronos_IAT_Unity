#ifndef UNIVERSAL_HAIR_DATA_INCLUDED
#define UNIVERSAL_HAIR_DATA_INCLUDED

struct HairData
{
    half3 specularTint;
    half3 secondarySpecularTint;
    half specularShift;
    half secondarySpecularShift;
    half perceptualSmoothness;
    half secondaryPerceptualSmoothness;
    half3 transmissionColor;
    half transmissionIntensity;
};

#endif