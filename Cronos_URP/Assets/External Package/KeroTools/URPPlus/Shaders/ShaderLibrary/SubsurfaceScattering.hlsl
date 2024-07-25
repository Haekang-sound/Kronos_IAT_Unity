#ifndef DIFFUSION_INCLUDED
#define DIFFUSION_INCLUDED

#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING)
//ref: https://therealmjp.github.io/posts/sss-sg/
/*struct SG
{
    float3 Amplitude;
    float3 Axis;
    float Sharpness;
};

half3 ApproximateSGIntegral(in SG sg)
{
    return 2 * PI * (sg.Amplitude / sg.Sharpness);
}

half3 SGIrradianceFitted(in SG lightingLobe, in half3 normal)
{
    const half muDotN = dot(lightingLobe.Axis, normal);
    const half lambda = lightingLobe.Sharpness;

    const half c0 = 0.36h;
    const half c1 = 1.0h / (4.0h * c0);

    half eml  = exp(-lambda);
    half em2l = eml * eml;
    half rl   = rcp(lambda);

    half scale = 1.0h + 2.0h * em2l - rl;
    half bias  = (eml - em2l) * rl - em2l;

    half x  = sqrt(1.0h - scale);
    half x0 = c0 * muDotN;
    half x1 = c1 * x;

    half n = x0 + x1;

    half y = (abs(x0) <= x1) ? n * n / x : saturate(muDotN);

    half normalizedIrradiance = scale * y + bias;

    return normalizedIrradiance * ApproximateSGIntegral(lightingLobe);
}

SG MakeNormalizedSG(in half3 axis, in half sharpness)
{
    SG sg;
    sg.Axis = axis;
    sg.Sharpness = sharpness;
    sg.Amplitude = 1.0h;
    sg.Amplitude = rcp(ApproximateSGIntegral(sg));

    return sg;
}*/

/*#if !defined(_SHADER_QUALITY_PREINTEGRATED_SSS)
    // Represent the diffusion profiles as spherical gaussians
    SG redKernel = MakeNormalizedSG(light.direction, 1.0f / max(surfaceData.curvature * surfaceData.diffusionColor.x, 0.0001f));
    SG greenKernel = MakeNormalizedSG(light.direction, 1.0f / max(surfaceData.curvature * surfaceData.diffusionColor.y, 0.0001f));
    SG blueKernel = MakeNormalizedSG(light.direction, 1.0f / max(surfaceData.curvature * surfaceData.diffusionColor.z, 0.0001f));

    // Compute the irradiance that would result from convolving a punctual light source
    // with the SG filtering kernels
    half3 diffuse = half3(SGIrradianceFitted(redKernel, normalWS).x,
                     SGIrradianceFitted(greenKernel, normalWS).x,
                     SGIrradianceFitted(blueKernel, normalWS).x);
#else
    //...diffusion-lut code
#endif*/

TEXTURE2D(_DiffusionLUT);             SAMPLER(sampler_DiffusionLUT);
half3 SubSurfaceScattering(SurfaceData surfaceData, BRDFData brdfData, Light light, half3 normalWS)
{
    half NdotL = dot(normalWS, light.direction);
    #ifdef _DIFFUSION_LUT
        half3 diffuse = SAMPLE_TEXTURE2D(_DiffusionLUT, sampler_DiffusionLUT, real2(NdotL * 0.5 + 0.5, surfaceData.curvature)).rgb;
    #else
        half3 diffuse = saturate(NdotL);
    #endif

    return diffuse;
}

half3 Transmission(Light light, half3 normalWS, half3 subsurfaceColor, half thickness, half scale)
{
    half NdotL = max(0, -dot(light.direction, normalWS));
    half backLight = NdotL * (1.0h - thickness);
    half3 result = backLight * light.color * scale * subsurfaceColor;
    
    return result;
}
#endif

//////////////////////////////////////////
/**************Translucency**************/
//////////////////////////////////////////
//ref: https://colinbarrebrisebois.com/2012/04/09/approximating-translucency-revisited-with-simplified-spherical-gaussian/
//ref: https://github.com/google/filament/blob/24b88219fa6148b8004f230b377f163e6f184d65/shaders/src/shading_model_subsurface.fs
half3 Translucency(SurfaceData surfaceData, Light light, half3 diffuse, half3 normalWS, half3 viewDirWS)
{
    half invThickness = 1.0h - surfaceData.thickness;
    half lightAttenuation = light.distanceAttenuation * lerp(1.0h, light.shadowAttenuation, surfaceData.translucencyShadows);

    half NdotL = max(0.0h, dot(normalWS, light.direction));
    half3 translucencyAttenuation = surfaceData.diffusionColor * light.color * lightAttenuation;

    half3 H = normalize(-light.direction + normalWS * surfaceData.translucencyDistortion);
    half nonNormalizedVdotH = dot(viewDirWS, H);
    half VdotH = max(0.0h, nonNormalizedVdotH);
    half forwardScatter = exp2(VdotH * surfaceData.translucencyPower - surfaceData.translucencyPower) * surfaceData.translucencyScale;
    
    half backScatter = max(0.0h, NdotL * surfaceData.thickness + invThickness) * 0.5h;
    half subsurface = lerp(backScatter, 1.0h, forwardScatter) * invThickness;
    
    half3 fLT = (0.3183h * subsurface + surfaceData.translucencyAmbient) * translucencyAttenuation;
    half3 cLT = fLT * lerp(1.0h, diffuse, surfaceData.translucencyDiffuseInfluence);

    return cLT * invThickness;
}
#endif
