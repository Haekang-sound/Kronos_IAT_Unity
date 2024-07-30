#ifndef COMPLEX_LIT_INPUT_INCLUDED
#define COMPLEX_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#include "ShaderLibrary/ComplexLit/CompleLitProperties.hlsl"
#include "ShaderLibrary/ComplexLit/ComplexLitMaps.hlsl"
#include "ShaderLibrary/Weather.hlsl"

inline void InitializeSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = EmptyFill();

    half2 alphaRemap = half2(_AlphaRemapMin, _AlphaRemapMax);
    half4 albedoAlpha = SampleAlbedoAlpha(_BaseColor, alphaRemap, uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.albedo = albedoAlpha.rgb;
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _AlphaCutoff);
    outSurfaceData.albedo = AlphaModulate(outSurfaceData.albedo, outSurfaceData.alpha);

    half4 MAODS = half4(_Metallic, 1.0, 1.0, _Smoothness);
    half4 metallicGlossMinMax = half4(_MetallicRemapMin, _MetallicRemapMax, _SmoothnessRemapMin, _SmoothnessRemapMax);
    half2 aoMinMax = half2(_AORemapMin, _AORemapMax);
    half4 maskMap = SampleMaskMap(uv, TEXTURE2D_ARGS(_MaskMap, sampler_MaskMap), metallicGlossMinMax, aoMinMax, MAODS);

    #if _SPECULAR_SETUP
        outSurfaceData.metallic = half(1.0);
        outSurfaceData.specular = SAMPLE_TEXTURE2D(_SpecularColorMap, sampler_SpecularColorMap, uv).rgb * _SpecularColor.rgb;
    #else
        outSurfaceData.metallic = maskMap.r;
        outSurfaceData.specular = half3(0.0, 0.0, 0.0);
    #endif

    outSurfaceData.smoothness = maskMap.a;
    outSurfaceData.occlusion = maskMap.g;

    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_NormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    outSurfaceData.bentNormalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BentNormalMap, SAMPLER_NORMALMAP_IDX), _NormalScale);
    outSurfaceData.coatNormalTS = SampleNormal(uv, TEXTURE2D_ARGS(_CoatNormalMap, SAMPLER_NORMALMAP_IDX), _CoatNormalScale);

    outSurfaceData.anisotropy = 1.0;
    #ifdef _ANISOTROPYMAP
        outSurfaceData.anisotropy = SAMPLE_TEXTURE2D(_AnisotropyMap, sampler_AnisotropyMap, uv).r;
    #endif
    outSurfaceData.anisotropy *= _Anisotropy;

    half3 tangentMap = half3(1.0, 1.0, 0.0);
    #ifdef _TANGENTMAP
        tangentMap = UnpackNormalmapRGorAG(SAMPLE_TEXTURE2D(_TangentMap, sampler_TangentMap, uv));
    #endif
    outSurfaceData.tangentTS = tangentMap;

    #ifdef _MATERIAL_FEATURE_IRIDESCENCE
        half iridescenceThickness = _IridescenceThickness;
        #if defined(_IRIDESCENCE_THICKNESSMAP)
            iridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * SAMPLE_TEXTURE2D(_IridescenceThicknessMap, sampler_IridescenceThicknessMap, uv).r;
        #endif

        half iridescenceMask = _IridescenceMaskScale * SAMPLE_TEXTURE2D(_IridescenceMaskMap, sampler_IridescenceMaskMap, uv).r;

        outSurfaceData.iridescenceTMS = half3(iridescenceThickness, iridescenceMask, _IridescenceShift);
    #else
        outSurfaceData.iridescenceTMS = half3(1.0, 0.0, 0.0);
    #endif

    outSurfaceData.thickness = _Thickness;
    outSurfaceData.curvature = _Curvature;
    outSurfaceData.diffusionColor = _DiffusionColor.rgb;
    outSurfaceData.transmissionScale = _TransmissionScale;
    outSurfaceData.translucencyScale = _TranslucencyScale;
    outSurfaceData.translucencyPower = 100.0 * _TranslucencyPower;
    outSurfaceData.translucencyAmbient = _TranslucencyAmbient;
    outSurfaceData.translucencyDistortion = _TranslucencyDistortion;
    outSurfaceData.translucencyShadows = _TranslucencyShadows;
    outSurfaceData.translucencyDiffuseInfluence = _TranslucencyDiffuseInfluence;
    #ifdef _THICKNESS_CURVATUREMAP
        #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING)
        half2 thicknessCurvature = SAMPLE_TEXTURE2D(_ThicknessCurvatureMap, sampler_ThicknessCurvatureMap, uv).rg;
        outSurfaceData.thickness = _ThicknessCurvatureRemap.x + thicknessCurvature.r * (_ThicknessCurvatureRemap.y - _ThicknessCurvatureRemap.x);
        outSurfaceData.curvature = _ThicknessCurvatureRemap.z + thicknessCurvature.g * (_ThicknessCurvatureRemap.w - _ThicknessCurvatureRemap.z);
        #elif defined(_MATERIAL_FEATURE_TRANSLUCENCY)
        half thickness = SAMPLE_TEXTURE2D(_ThicknessCurvatureMap, sampler_ThicknessCurvatureMap, uv).r;
        outSurfaceData.thickness = _ThicknessCurvatureRemap.x + thickness.r * (_ThicknessCurvatureRemap.y - _ThicknessCurvatureRemap.x);
        #endif
    #endif

    #if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        half2 clearCoat = SampleClearCoat(uv, TEXTURE2D_ARGS(_ClearCoatMap, sampler_ClearCoatMap), half2(_ClearCoatMask, _ClearCoatSmoothness));
        outSurfaceData.clearCoatMask = clearCoat.r;
        outSurfaceData.clearCoatSmoothness = clearCoat.g;
    #endif

    outSurfaceData.ior = _Ior;
    outSurfaceData.transmittanceColor = _TransmittanceColor.rgb * SAMPLE_TEXTURE2D(_TransmittanceColorMap, sampler_TransmittanceColorMap, uv).rgb;
    outSurfaceData.chromaticAberration = _ChromaticAberration;

    #if defined(_DETAIL)
        float2 detailUV = TRANSFORM_TEX(uv, _DetailMap);
        half4 detail = SAMPLE_TEXTURE2D(_DetailMap, sampler_DetailMap, detailUV);
        half3 detailRemap = half3(_DetailAlbedoScale, _DetailNormalScale, _DetailSmoothnessScale);
        
        ApplyHDRPDetailMapping(detail, detailRemap, maskMap.b, 
                                outSurfaceData.albedo, outSurfaceData.normalTS, outSurfaceData.smoothness);
    #endif

    outSurfaceData.emission = SampleEmission(uv, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap),
                                                outSurfaceData.albedo, _EmissionColor.rgb, _EmissionScale);

    outSurfaceData.horizonFade = _HorizonFade;
    outSurfaceData.giOcclusionBias = _GIOcclusionBias;
}

#endif