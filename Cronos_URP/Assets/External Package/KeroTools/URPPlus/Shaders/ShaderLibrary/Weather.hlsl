#ifndef WEATHER_INCLUDED
#define WEATHER_INCLUDED

#include "ShaderLibrary/TriplanarMapping.hlsl"

TEXTURE2D(_PuddlesNormal);      SAMPLER(sampler_PuddlesNormal);
TEXTURE2D(_RainNormal);         SAMPLER(sampler_RainNormal);
TEXTURE2D(_RainDistortionMap);  SAMPLER(sampler_RainDistortionMap);
TEXTURE2D(_RainMaskMap);        SAMPLER(sampler_RainMaskMap);

half _RainMultiplier = 1.0;
half _Wetness = 0.0;
half4 _WetnessColor = half4(0.0, 0.0, 1.0, 0.0);

float2 FlipbookAnimation(float2 uv, uint2 tilesSize, float speed) 
{
    float tileCountX = 1.0 / tilesSize.x;
    float tileCountY = 1.0 / tilesSize.y;

    uint frameIndex = speed % (tilesSize.x * tilesSize.y);

    uint indexX = frameIndex % tilesSize.x;
    uint indexY = tilesSize.y - 1 - (frameIndex / tilesSize.x);

    float2 offset = float2(tileCountX * indexX, tileCountY * indexY);

    return uv * float2(tileCountX, tileCountY) + offset;
}

float2 CalculateRainDistortion(float2 uv)
{
    float2 rainDistortionUV = uv * _RainDistortionSize;

    return SAMPLE_TEXTURE2D(_RainDistortionMap, sampler_RainDistortionMap, rainDistortionUV).rg * _RainDistortionScale;
}

half3 ApplyTriplanarRain(UVMapping uvMapping, float2 rainDistortion, half3 puddlesNormal)
{
    // Perform rain animation
    float2 rainSpeed = float2(0.0, frac(_Time.y * _RainAnimationSpeed));
    float2 rainANormalUV = (uvMapping.uvXY + rainDistortion + rainSpeed) * _RainSize;
    float2 rainBNormalUV = (uvMapping.uvZY + rainDistortion + rainSpeed) * _RainSize;

    half rainANormalIntensity = _RainNormalScale * uvMapping.triplanarWeights.z;
    half rainBNormalIntensity = _RainNormalScale * uvMapping.triplanarWeights.x;

    half3 rainANormal = SampleNormal(rainANormalUV, TEXTURE2D_ARGS(_RainNormal, sampler_RainNormal), rainANormalIntensity);
    half3 rainBNormal = SampleNormal(rainBNormalUV, TEXTURE2D_ARGS(_RainNormal, sampler_RainNormal), rainBNormalIntensity);

    // Final result
    return BlendNormalRNM(puddlesNormal, BlendNormalRNM(rainANormal, rainBNormal));
}

void ApplyWeather(float3 positionWS, float3 normalWS, float2 uv, inout half3 albedo, inout half3 normalTS, inout half smoothness)
{
    // Initialize UV data
    UVMapping uvMapping = InitializeUVData(positionWS, normalWS, uv);

    half rainMask = SAMPLE_TEXTURE2D(_RainMaskMap, sampler_RainMaskMap, uvMapping.uv).r * _RainMultiplier;
    half wetnessFactor = _Wetness * _RainWetnessFactor * rainMask;
    
    if(rainMask > 0 || wetnessFactor > 0)
    {
        float2 rainDistortion = CalculateRainDistortion(uvMapping.uv);
        // Perform puddles animation
        float2 puddlesUV = FlipbookAnimation(uvMapping.uvXZ * _PuddlesSize, _PuddlesFramesSize.xy, (_Time.y * _PuddlesAnimationSpeed));
        half3 puddlesNormal = SampleNormal(puddlesUV, TEXTURE2D_ARGS(_PuddlesNormal, sampler_PuddlesNormal), _PuddlesNormalScale * saturate(normalWS.y));
    
        half3 rainNormal = puddlesNormal;
        #ifdef _RAIN_TRIPLANAR
            rainNormal = ApplyTriplanarRain(uvMapping, rainDistortion, puddlesNormal);
        #endif

        albedo = lerp(albedo, _WetnessColor.rgb, wetnessFactor * _WetnessColor.a);
        normalTS = lerp(normalTS, BlendNormalRNM(rainNormal, normalTS), rainMask);
    }

    smoothness = lerp(smoothness, 1.0h, wetnessFactor);
}

#endif