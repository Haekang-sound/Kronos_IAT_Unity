#ifndef UNIVERSAL_REFRACTION_INCLUDED
#define UNIVERSAL_REFRACTION_INCLUDED

#ifndef UNITY_SPECCUBE_LOD_STEPS
    // This is actuall the last mip index, we generate 7 mips of convolution
    #define UNITY_SPECCUBE_LOD_STEPS 6
#endif

#ifdef _REFRACTION
half _SSRefractionInvScreenWeightDistance;

TEXTURE2D_X(_CameraOpaqueTexture);
SAMPLER(sampler_CameraOpaqueTexture);

half4 CalculateRefractionFinalColor(LightingData lightingData, half alpha)
{
    half3 finalColor = CalculateLightingColor(lightingData, 1.0h);

    return half4(finalColor, 1.0h); // Removed redundant 'alpha' since it's not used.
}

half EdgeOfScreenFade(half2 coordNDC, half fadeRcpLength)
{
    half2 coordCS = coordNDC * 2 - 1;
    half2 t = saturate((1 - abs(coordCS)) * fadeRcpLength);

    return Smoothstep01(t.x * t.y);
}

half3 FetchRefractedColor(half2 screenUV, half chromaticAberration, half perceptualRoughness)
{
    //half mip = PositivePow(perceptualRoughness, 1.3) * uint(max(UNITY_SPECCUBE_LOD_STEPS - 1, 0));
    #ifdef _CHROMATIC_ABERRATION
        half3 rgb;
        half2 offset = half2(0.025 * chromaticAberration, 0);
        rgb.r = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV).r;
        rgb.g = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV - offset).g;
        rgb.b = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV + offset).b;
        
        return rgb;
    #else
        return SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV).rgb;
    #endif
}

// Big thanks for reference for AE Tuts
// Ref: https://www.youtube.com/watch?v=VMsOPUUj0JA&t=106s
half3 SimpleRefraction(InputData inputData, SurfaceData surfaceData, half perceptualRoughness)
{
    half3 R = normalize(refract(inputData.viewDirectionWS, inputData.normalWS, surfaceData.ior));
    half4 RVP = ComputeScreenPos(mul(UNITY_MATRIX_VP, half4(R + inputData.positionWS, 1.0h)));
    half2 screenUV = RVP.xy / RVP.w;

    half3 rgb = FetchRefractedColor(screenUV, surfaceData.chromaticAberration, perceptualRoughness);

    half weight = EdgeOfScreenFade(screenUV, _SSRefractionInvScreenWeightDistance);
    half3 indirectSpecular = GlossyEnvironmentReflection(R, inputData.positionWS, perceptualRoughness, 1.0h, inputData.normalizedScreenSpaceUV);

    return lerp(indirectSpecular, rgb, weight) * surfaceData.transmittanceColor;
}

#endif

void ApplyRefractionBRDF(InputData inputData, SurfaceData surfaceData, half perceptualRoughness, inout half3 diffuse)
{
    #if defined(_REFRACTION)
        half3 refractionColor = SimpleRefraction(inputData, surfaceData, perceptualRoughness);
        diffuse = lerp(refractionColor, diffuse, surfaceData.alpha);
    #endif
}

#endif