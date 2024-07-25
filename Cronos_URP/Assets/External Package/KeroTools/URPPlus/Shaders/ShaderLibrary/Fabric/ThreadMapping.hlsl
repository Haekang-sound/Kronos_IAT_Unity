#ifndef UNIVERSAL_THREAD_MAPPING
#define UNIVERSAL_THREAD_MAPPING

TEXTURE2D(_ThreadMap);  SAMPLER(sampler_ThreadMap);

half ThreadSmoothness(half smoothness, half threadSmoothness, half scale)
{
    return saturate(smoothness + lerp(0.0h, (-1.0h + threadSmoothness * 2.0h), scale));
}

half ThreadAO(half occlusion, half threadAO, half scale)
{
    return occlusion * lerp(1.0h, threadAO, scale);
}

half3 ThreadNormal(half2 threadAG, half3 normalTS, half scale)
{
    half3 threadNormal = UnpackNormalmapRGorAG(half4(threadAG.y, threadAG.x, 1.0h, 1.0h), scale);

    return BlendNormalRNM(normalTS, normalize(threadNormal));
}

// threadRemap:
// x - ThreadAOScale
// y - ThreadNormalScale
// z - ThreadSmoothnessScale

void ApplyThreadMapping(float2 uv, half3 threadRemap, inout SurfaceData surfaceData)
{
    half4 thread = SAMPLE_TEXTURE2D(_ThreadMap, sampler_ThreadMap, uv);
    half occlusion = ThreadAO(surfaceData.occlusion, thread.r, threadRemap.x);

    surfaceData.albedo *= occlusion;
    surfaceData.smoothness = ThreadSmoothness(surfaceData.smoothness, thread.b, threadRemap.z);
    surfaceData.normalTS = ThreadNormal(thread.ag, surfaceData.normalTS, threadRemap.y);
    surfaceData.occlusion = occlusion;
}

#endif