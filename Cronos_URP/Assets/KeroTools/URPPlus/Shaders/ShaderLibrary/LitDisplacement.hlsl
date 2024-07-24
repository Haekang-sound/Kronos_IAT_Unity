#ifndef UNIVERSAL_DISPLACEMENT_INCLUDED
#define UNIVERSAL_DISPLACEMENT_INCLUDED

float3 GetDisplacementObjectScale(bool vertexDisplacement)
{
    float3 objectScale = float3(1.0, 1.0, 1.0);
    float4x4 worldTransform;

    if (vertexDisplacement)
    {
        worldTransform = GetObjectToWorldMatrix();
    }
    else
    {
        worldTransform = GetWorldToObjectMatrix();
    }

    objectScale.x = length(float3(worldTransform._m00, worldTransform._m01, worldTransform._m02));
    #if !defined(_PIXEL_DISPLACEMENT) || (defined(_PIXEL_DISPLACEMENT_LOCK_OBJECT_SCALE))
        objectScale.y = length(float3(worldTransform._m10, worldTransform._m11, worldTransform._m12));
    #endif
    objectScale.z = length(float3(worldTransform._m20, worldTransform._m21, worldTransform._m22));

    return objectScale;
}

float GetMaxDisplacement()
{
    float maxDisplacement = 0.0;
    #if defined(_HEIGHTMAP)
    maxDisplacement = abs(_HeightAmplitude); // _HeightAmplitude can be negative if min and max are inverted, but the max displacement must be positive
    #endif
    return maxDisplacement;
}

float2 GetMinUvSize(float2 uv)
{
    float2 minUvSize = float2(FLT_MAX, FLT_MAX);

    #if defined(_HEIGHTMAP)
    minUvSize = min(uv * _HeightMap_TexelSize.zw, minUvSize);
    #endif

    return minUvSize;
}

void ApplyDisplacementTileScale(inout float height)
{
    // Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y)
    // Inverse tiling scale *= (1 / _TexWorldScale) if planar or triplanar
    #ifdef _DISPLACEMENT_LOCK_TILING_SCALE
    height *= _InvTilingScale;
    #endif
}

float3 ComputePerVertexDisplacement(TEXTURE2D_PARAM(heightMap, sampler_heightMap), float2 uv, float lod)
{
    #ifdef _HEIGHTMAP
    float height = (SAMPLE_TEXTURE2D_LOD(heightMap, sampler_heightMap, uv, lod).r - _HeightCenter) * _HeightAmplitude;

    #if defined(LOD_FADE_CROSSFADE) && defined(_TESSELLATION_DISPLACEMENT)
        height *= unity_LODFade.x;
    #endif
    #else
    float height = 0.0;
    #endif

    // Height is affected by tiling property and by object scale (depends on option).
    // Apply scaling from tiling properties (TexWorldScale and tiling from BaseColor)
    ApplyDisplacementTileScale(height);
    // Applying scaling of the object if requested
    #ifdef _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
    float3 objectScale = GetDisplacementObjectScale(true);
    return height.xxx * objectScale;
    #else
    return height.xxx;
    #endif
}

#include "ShaderLibrary/PerPixelDisplacement.hlsl"
#include "ShaderLibrary/Tessellation/Tessellation.hlsl"
#endif
