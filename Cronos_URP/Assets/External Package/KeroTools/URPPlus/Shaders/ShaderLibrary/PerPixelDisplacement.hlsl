#ifndef UNIVERSAL_PERPIXEL_DISPLACEMENT_INCLUDED
#define UNIVERSAL_PERPIXEL_DISPLACEMENT_INCLUDED

#if defined(_PIXEL_DISPLACEMENT) && defined(_DEPTHOFFSET_ON)
    #define _DEPTHOFFSET
#endif

float3 GetViewDirectionTangentSpace(float4 tangentWS, half3 normalWS, half3 viewDirWS)
{
    half3 unnormalizedNormalWS = normalWS;
    const half renormFactor = 1.0 / length(unnormalizedNormalWS);

    float crossSign = (tangentWS.w > 0.0 ? 1.0 : -1.0);
    half3 bitang = crossSign * cross(normalWS.xyz, tangentWS.xyz);

    half3 WorldSpaceNormal = renormFactor * normalWS.xyz;
    half3 WorldSpaceTangent = renormFactor * tangentWS.xyz;
    half3 WorldSpaceBiTangent = renormFactor * bitang;

    half3x3 tangentSpaceTransform = half3x3(WorldSpaceTangent, WorldSpaceBiTangent, WorldSpaceNormal);
    half3 viewDirTS = mul(tangentSpaceTransform, viewDirWS);

    return viewDirTS;
}

#if defined(_PIXEL_DISPLACEMENT)
float2 ParallaxOcclusionMapping(half lod, half lodThreshold, int numSteps, real3 viewDirTS, float2 uv, out half outHeight)
{
    half stepSize = 1.0 / (half)numSteps;
    float2 parallaxMaxOffsetTS = (viewDirTS.xy / -viewDirTS.z);
    float2 texOffsetPerStep = stepSize * parallaxMaxOffsetTS;

    // Do a first step before the loop to init all value correctly
    float2 texOffsetCurrent = float2(0.0, 0.0);
    half prevHeight = SAMPLE_TEXTURE2D_LOD(_HeightMap, sampler_HeightMap, uv, lod).r;
    texOffsetCurrent += texOffsetPerStep;
    half currHeight = SAMPLE_TEXTURE2D_LOD(_HeightMap, sampler_HeightMap, uv + texOffsetCurrent, lod).r;
    half rayHeight = 1.0 - stepSize; // Start at top less one sample

    // Linear search
    for (int stepIndex = 0; stepIndex < numSteps; ++stepIndex)
    {
        // Have we found a height below our ray height ? then we have an intersection
        if (currHeight > rayHeight)
            break; // end the loop

        prevHeight = currHeight;
        rayHeight -= stepSize;
        texOffsetCurrent += texOffsetPerStep;

        // Sample height map which in this case is stored in the alpha channel of the normal map:
        currHeight = SAMPLE_TEXTURE2D_LOD(_HeightMap, sampler_HeightMap, uv + texOffsetCurrent, lod).r;
    }

    // Found below and above points, now perform line interesection (ray) with piecewise linear heightfield approximation

    // Refine the search with secant method
#define POM_SECANT_METHOD 1
#if POM_SECANT_METHOD

    half pt0 = rayHeight + stepSize;
    half pt1 = rayHeight;
    half delta0 = pt0 - prevHeight;
    half delta1 = pt1 - currHeight;

    half delta;
    float2 offset;

    // Secant method to affine the search
    // Ref: Faster Relief Mapping Using the Secant Method - Eric Risser
    for (int i = 0; i < 3; ++i)
    {
        // intersectionHeight is the height [0..1] for the intersection between view ray and heightfield line
        half intersectionHeight = (pt0 * delta1 - pt1 * delta0) / (delta1 - delta0);
        // Retrieve offset require to find this intersectionHeight
        offset = (1 - intersectionHeight) * texOffsetPerStep * numSteps;

        currHeight = SAMPLE_TEXTURE2D_LOD(_HeightMap, sampler_HeightMap, uv + offset, lod).r;

        delta = intersectionHeight - currHeight;

        if (abs(delta) <= 0.01)
            break;

        // intersectionHeight < currHeight => new lower bounds
        if (delta < 0.0)
        {
            delta1 = delta;
            pt1 = intersectionHeight;
        }
        else
        {
            delta0 = delta;
            pt0 = intersectionHeight;
        }
    }

#else // regular POM intersection

    half delta0 = currHeight - rayHeight;
    half delta1 = (rayHeight + stepSize) - prevHeight;
    half ratio = delta0 / (delta0 + delta1);
    float2 offset = texOffsetCurrent - ratio * texOffsetPerStep;

    currHeight = SAMPLE_TEXTURE2D_LOD(_HeightMap, sampler_HeightMap, uv + offset, lod).r;

#endif

    outHeight = currHeight;

    offset *= (1.0 - saturate(lod - lodThreshold));

    return offset;
}

float ApplyPerPixelDisplacement(half3 viewDirTS, half3 viewDirWS, inout half3 positionWS, inout float2 uv)
{
#if defined(_PIXEL_DISPLACEMENT) && defined(_HEIGHTMAP)
    viewDirTS *= GetDisplacementObjectScale(false).xzy;
    
    half  maxHeight = GetMaxDisplacement();
    ApplyDisplacementTileScale(maxHeight);
    float2 minUvSize = GetMinUvSize(uv);
    half lod = ComputeTextureLOD(minUvSize);

    // TODO: precompute uvSpaceScale
    float2 uvSpaceScale = _InvPrimScale.xy * _BaseMap_ST.xy * maxHeight;
    
    half height = 0;

    float3 viewDirUV  = float3(viewDirTS.xy * uvSpaceScale, viewDirTS.z);
    float unitAngle = saturate(FastACosPos(viewDirUV.z) * INV_HALF_PI);
    int numSteps = (int)lerp(_PPDMinSamples, _PPDMaxSamples, unitAngle);
    float2 offset = ParallaxOcclusionMapping(lod, _PPDLodThreshold, numSteps, viewDirUV, uv, height);

    uv += offset;

    #ifdef _DEPTHOFFSET
        float verticalDisplacement = (maxHeight - height * maxHeight) / max(viewDirTS.z, 0.0001);
        positionWS += verticalDisplacement * (-viewDirWS);
        return ComputeNormalizedDeviceCoordinatesWithZ(positionWS, GetWorldToHClipMatrix()).z;
    #else
        return 0.0;
    #endif
#else
    return 0.0;
#endif
}
#endif

#endif