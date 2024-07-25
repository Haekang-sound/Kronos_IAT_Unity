#ifndef UNIVERSAL_HAIR_META_PASS_INCLUDED
#define UNIVERSAL_HAIR_META_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"

half4 FragmentMetaHair(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    HairData hairData;
    InitializeSurfaceData(input.uv, surfaceData, hairData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness,
                       surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = surfaceData.emission;
    return UniversalFragmentMeta(input, metaInput);
}

#endif