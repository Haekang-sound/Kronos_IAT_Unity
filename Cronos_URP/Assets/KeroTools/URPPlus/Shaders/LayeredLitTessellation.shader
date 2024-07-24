Shader "KeroTools/URP+/LayeredLitTessellation"
{
    Properties
    {
        ///////////////////////////////////////
        /***********Surface Options***********/
        ///////////////////////////////////////
        _WorkflowMode("WorkflowMode", Float) = 1.0
        
        // Blending state
        _Surface("__surface", Float) = 0.0
        _Blend("__blend", Float) = 0.0
        _Cull("__cull", Float) = 2.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _SrcBlendAlpha("__srcA", Float) = 1.0
        [HideInInspector] _DstBlendAlpha("__dstA", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _ZTest("ZTest", Int) = 4
        [HideInInspector] _BlendModePreserveSpecular("BlendModePreserveSpecular", Float) = 1.0
        
        // Normal Mode
        [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double sided normal mode", Float) = 1
        [HideInInspector] _DoubleSidedConstants("Double-Sided Constants", Vector) = (1.0, 1.0, 1.0, 0.0)
        
        // Alpha Clipping
        [ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0.0
        [ToggleUI] _UseShadowThreshold("UseShadowThreshold", Float) = 0.0
        _AlphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _AlphaCutoffShadow("AlphaCutoffShadow", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _AlphaToMask("__alphaToMask", Float) = 0.0

        // Geometric Specular AA
        [ToggleUI] _EnableGeometricSpecularAA("EnableGeometricSpecularAA", Float) = 0.0
        _SpecularAAScreenSpaceVariance("SpecularAAScreenSpaceVariance", Range(0.0, 1.0)) = 0.1
        _SpecularAAThreshold("SpecularAAThreshold", Range(0.0, 1.0)) = 0.2

        // Displacement Mode
        [Enum(None, 0, Tessellation, 1)] _DisplacementMode("DisplacementMode", Float) = 0.0
        [ToggleUI] _DisplacementLockObjectScale("Displacement lock object scale", Float) = 1.0
        [ToggleUI] _DisplacementLockTilingScale("Displacement lock tiling scale", Float) = 1.0
        [HideInInspector] _InvTilingScale("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1
        [HideInInspector] _InvTilingScale1("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1
        [HideInInspector] _InvTilingScale2("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1
        [HideInInspector] _InvTilingScale3("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1

        //////////////////////////////////////
        /********Tessellation Options********/
        //////////////////////////////////////
        [ToggleUI] _PhongTessellationMode("Phong Tessellation mode", Float) = 0.0
        [Enum(None, 0, Edge, 1, Distance, 2)] _TessellationMode("Tessellation mode", Float) = 0
        _TessellationFactor("Tessellation Factor", Range(1.0, 64.0)) = 4.0
        _TessellationEdgeLength("Tessellation Edge Length", Range(5, 100)) = 50
        _TessellationFactorMinDistance("Tessellation start fading distance", Float) = 20.0
        _TessellationFactorMaxDistance("Tessellation end fading distance", Float) = 50.0
        _TessellationShapeFactor("Tessellation shape factor", Range(0.0, 1.0)) = 0.75 // Only use with Phong
        _TessellationBackFaceCullEpsilon("Tessellation back face epsilon", Range(-1.0, 0.0)) = -0.25

        //////////////////////////////////////
        /***********Surface Inputs***********/
        //////////////////////////////////////
        [HideInInspector] _LayerCount("Layers", Range(2.0, 4.0)) = 2.0
        [Enum(None, 0, Multiply, 1, AddSubstract, 2)] _VertexColorMode("Vertex color mode", Float) = 0
        [ToggleUI] _UseMainLayerInfluence("UseMainLayerInfluence", Float) = 0.0
        [ToggleUI] _UseHeightBasedBlend("UseHeightBasedBlend", Float) = 0.0
        _LayerMaskMap("LayerMaskMap", 2D) = "white" {}
        _LayerInfluenceMaskMap("LayerInfluenceMaskMap", 2D) = "white" {}
        [ToggleUI] _UseHeightBasedBlend("UseHeightBasedBlend", Float) = 0.0
        _HeightTransition("Height Transition", Range(0, 1.0)) = 0.01
        [ToggleUI] _UseMainLayerInfluence("UseMainLayerInfluence", Float) = 0.0

        _InheritBaseNormal1("_InheritBaseNormal1", Range(0, 1.0)) = 0.0
        _InheritBaseNormal2("_InheritBaseNormal2", Range(0, 1.0)) = 0.0
        _InheritBaseNormal3("_InheritBaseNormal3", Range(0, 1.0)) = 0.0

        _InheritBaseHeight1("_InheritBaseHeight1", Range(0, 1.0)) = 0.0
        _InheritBaseHeight2("_InheritBaseHeight2", Range(0, 1.0)) = 0.0
        _InheritBaseHeight3("_InheritBaseHeight3", Range(0, 1.0)) = 0.0

        _InheritBaseColor1("_InheritBaseColor1", Range(0, 1.0)) = 0.0
        _InheritBaseColor2("_InheritBaseColor2", Range(0, 1.0)) = 0.0
        _InheritBaseColor3("_InheritBaseColor3", Range(0, 1.0)) = 0.0

        [ToggleUI] _OpacityAsDensity("_OpacityAsDensity", Float) = 0.0
        [ToggleUI] _OpacityAsDensity1("_OpacityAsDensity1", Float) = 0.0
        [ToggleUI] _OpacityAsDensity2("_OpacityAsDensity2", Float) = 0.0
        [ToggleUI] _OpacityAsDensity3("_OpacityAsDensity3", Float) = 0.0

        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _BaseColor1("BaseColor1", Color) = (1, 1, 1, 1)
        _BaseColor2("BaseColor2", Color) = (1, 1, 1, 1)
        _BaseColor3("BaseColor3", Color) = (1, 1, 1, 1)

        _BaseMap("BaseColorMap", 2D) = "white" {}
        _BaseMap1("BaseColorMap1", 2D) = "white" {}
        _BaseMap2("BaseColorMap2", 2D) = "white" {}
        _BaseMap3("BaseColorMap3", 2D) = "white" {}

        _AlphaRemapMin("AlphaRemapMin", Range(0.0, 1.0)) = 0.0
        _AlphaRemapMin1("AlphaRemapMin1", Range(0.0, 1.0)) = 0.0
        _AlphaRemapMin2("AlphaRemapMin2", Range(0.0, 1.0)) = 0.0
        _AlphaRemapMin3("AlphaRemapMin3", Range(0.0, 1.0)) = 0.0
        _AlphaRemapMax("AlphaRemapMax", Range(0.0, 1.0)) = 1.0
        _AlphaRemapMax1("AlphaRemapMax1", Range(0.0, 1.0)) = 1.0
        _AlphaRemapMax2("AlphaRemapMax2", Range(0.0, 1.0)) = 1.0
        _AlphaRemapMax3("AlphaRemapMax3", Range(0.0, 1.0)) = 1.0

        _MaskMap("MaskMap", 2D) = "white" {}
        _MaskMap1("MaskMap1", 2D) = "white" {}
        _MaskMap2("MaskMap2", 2D) = "white" {}
        _MaskMap3("MaskMap3", 2D) = "white" {}

        _Metallic("Metallic", Range(0.0, 1.0)) = 0
        _Metallic1("Metallic1", Range(0.0, 1.0)) = 0
        _Metallic2("Metallic2", Range(0.0, 1.0)) = 0
        _Metallic3("Metallic3", Range(0.0, 1.0)) = 0

        _MetallicRemapMin("MetallicRemapMin", Range(0.0, 1.0)) = 0.0
        _MetallicRemapMin1("MetallicRemapMin1", Range(0.0, 1.0)) = 0.0
        _MetallicRemapMin2("MetallicRemapMin2", Range(0.0, 1.0)) = 0.0
        _MetallicRemapMin3("MetallicRemapMin3", Range(0.0, 1.0)) = 0.0

        _MetallicRemapMax("MetallicRemapMax", Range(0.0, 1.0)) = 1.0
        _MetallicRemapMax1("MetallicRemapMax1", Range(0.0, 1.0)) = 1.0
        _MetallicRemapMax2("MetallicRemapMax2", Range(0.0, 1.0)) = 1.0
        _MetallicRemapMax3("MetallicRemapMax3", Range(0.0, 1.0)) = 1.0

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _Smoothness1("Smoothness1", Range(0.0, 1.0)) = 0.5
        _Smoothness2("Smoothness2", Range(0.0, 1.0)) = 0.5
        _Smoothness3("Smoothness3", Range(0.0, 1.0)) = 0.5

        _SmoothnessRemapMin("SmoothnessRemapMin", Range(0.0, 1.0)) = 0.0
        _SmoothnessRemapMin1("SmoothnessRemapMin1", Range(0.0, 1.0)) = 0.0
        _SmoothnessRemapMin2("SmoothnessRemapMin2", Range(0.0, 1.0)) = 0.0
        _SmoothnessRemapMin3("SmoothnessRemapMin3", Range(0.0, 1.0)) = 0.0

        _SmoothnessRemapMax("SmoothnessRemapMax", Range(0.0, 1.0)) = 1.0
        _SmoothnessRemapMax1("SmoothnessRemapMax1", Range(0.0, 1.0)) = 1.0
        _SmoothnessRemapMax2("SmoothnessRemapMax2", Range(0.0, 1.0)) = 1.0
        _SmoothnessRemapMax3("SmoothnessRemapMax3", Range(0.0, 1.0)) = 1.0

        _AORemapMin("AORemapMin", Range(0.0, 1.0)) = 0.0
        _AORemapMin1("AORemapMin1", Range(0.0, 1.0)) = 0.0
        _AORemapMin2("AORemapMin2", Range(0.0, 1.0)) = 0.0
        _AORemapMin3("AORemapMin3", Range(0.0, 1.0)) = 0.0

        _AORemapMax("AORemapMax", Range(0.0, 1.0)) = 1.0
        _AORemapMax1("AORemapMax1", Range(0.0, 1.0)) = 1.0
        _AORemapMax2("AORemapMax2", Range(0.0, 1.0)) = 1.0
        _AORemapMax3("AORemapMax3", Range(0.0, 1.0)) = 1.0

        [Normal] _NormalMap("NormalMap", 2D) = "bump" {}
        [Normal] _NormalMap1("NormalMap1", 2D) = "bump" {}
        [Normal] _NormalMap2("NormalMap2", 2D) = "bump" {}
        [Normal] _NormalMap3("NormalMap3", 2D) = "bump" {}

        _NormalScale("_NormalScale", Range(0.0, 8.0)) = 1.0
        _NormalScale1("_NormalScale1", Range(0.0, 8.0)) = 1.0
        _NormalScale2("_NormalScale2", Range(0.0, 8.0)) = 1.0
        _NormalScale3("_NormalScale3", Range(0.0, 8.0)) = 1.0

        [Normal] _BentNormalMap("BentNormalMap", 2D) = "bump" {}
        [Normal] _BentNormalMap1("BentNormalMap1", 2D) = "bump" {}
        [Normal] _BentNormalMap2("BentNormalMap2", 2D) = "bump" {}
        [Normal] _BentNormalMap3("BentNormalMap3", 2D) = "bump" {}

        _HeightMap("HeightMap", 2D) = "black" {}
        _HeightMap1("HeightMap1", 2D) = "black" {}
        _HeightMap2("HeightMap2", 2D) = "black" {}
        _HeightMap3("HeightMap3", 2D) = "black" {}

        // Caution: Default value of _HeightAmplitude must be (_HeightMax - _HeightMin) * 0.01
        // These two properties are computed from exposed properties by the UI block and are separated so we don't lose information by changing displacement mode in the UI block
        [HideInInspector] _HeightAmplitude("Height Scale", Float) = 0.02
        [HideInInspector] _HeightAmplitude1("Height Scale1", Float) = 0.02
        [HideInInspector] _HeightAmplitude2("Height Scale2", Float) = 0.02
        [HideInInspector] _HeightAmplitude3("Height Scale3", Float) = 0.02
        [HideInInspector] _HeightCenter("Height Bias0", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _HeightCenter1("Height Bias1", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _HeightCenter2("Height Bias2", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _HeightCenter3("Height Bias3", Range(0.0, 1.0)) = 0.5

        [Enum(MinMax, 0, Amplitude, 1)] _HeightMapParametrization("Heightmap Parametrization", Int) = 0
        [Enum(MinMax, 0, Amplitude, 1)] _HeightMapParametrization1("Heightmap Parametrization1", Int) = 0
        [Enum(MinMax, 0, Amplitude, 1)] _HeightMapParametrization2("Heightmap Parametrization2", Int) = 0
        [Enum(MinMax, 0, Amplitude, 1)] _HeightMapParametrization3("Heightmap Parametrization3", Int) = 0
        // These parameters are for vertex displacement/Tessellation
        _HeightOffset("Height Offset", Float) = 0
        _HeightOffset1("Height Offset1", Float) = 0
        _HeightOffset2("Height Offset2", Float) = 0
        _HeightOffset3("Height Offset3", Float) = 0
        // MinMax mode
        _HeightMin("Height Min", Float) = -1
        _HeightMin1("Height Min1", Float) = -1
        _HeightMin2("Height Min2", Float) = -1
        _HeightMin3("Height Min3", Float) = -1
        _HeightMax("Height Max", Float) = 1
        _HeightMax1("Height Max1", Float) = 1
        _HeightMax2("Height Max2", Float) = 1
        _HeightMax3("Height Max3", Float) = 1

        // Amplitude mode
        _HeightTessAmplitude("Amplitude", Float) = 2.0 // in Centimeters
        _HeightTessAmplitude1("Amplitude1", Float) = 2.0 // in Centimeters
        _HeightTessAmplitude2("Amplitude2", Float) = 2.0 // in Centimeters
        _HeightTessAmplitude3("Amplitude3", Float) = 2.0 // in Centimeters
        _HeightTessCenter("Height Bias", Range(0.0, 1.0)) = 0.5
        _HeightTessCenter1("Height Bias1", Range(0.0, 1.0)) = 0.5
        _HeightTessCenter2("Height Bias2", Range(0.0, 1.0)) = 0.5
        _HeightTessCenter3("Height Bias3", Range(0.0, 1.0)) = 0.5

        // These parameters are for pixel displacement
        _HeightPoMAmplitude("Height Amplitude", Float) = 2.0 // In centimeters
        _HeightPoMAmplitude1("Height Amplitude1", Float) = 2.0 // In centimeters
        _HeightPoMAmplitude2("Height Amplitude2", Float) = 2.0 // In centimeters
        _HeightPoMAmplitude3("Height Amplitude3", Float) = 2.0 // In centimeters

        /*************************************/
        /************Detail Inputs************/
        /*************************************/
        _DetailMap("DetailMap", 2D) = "linearGrey" {}
        _DetailMap1("DetailMap1", 2D) = "linearGrey" {}
        _DetailMap2("DetailMap2", 2D) = "linearGrey" {}
        _DetailMap3("DetailMap3", 2D) = "linearGrey" {}

        _DetailAlbedoScale("_DetailAlbedoScale", Range(0.0, 2.0)) = 1
        _DetailAlbedoScale1("_DetailAlbedoScale1", Range(0.0, 2.0)) = 1
        _DetailAlbedoScale2("_DetailAlbedoScale2", Range(0.0, 2.0)) = 1
        _DetailAlbedoScale3("_DetailAlbedoScale3", Range(0.0, 2.0)) = 1

        _DetailNormalScale("_DetailNormalScale", Range(0.0, 2.0)) = 1
        _DetailNormalScale1("_DetailNormalScale1", Range(0.0, 2.0)) = 1
        _DetailNormalScale2("_DetailNormalScale2", Range(0.0, 2.0)) = 1
        _DetailNormalScale3("_DetailNormalScale3", Range(0.0, 2.0)) = 1

        _DetailSmoothnessScale("_DetailSmoothnessScale", Range(0.0, 2.0)) = 1
        _DetailSmoothnessScale1("_DetailSmoothnessScale1", Range(0.0, 2.0)) = 1
        _DetailSmoothnessScale2("_DetailSmoothnessScale2", Range(0.0, 2.0)) = 1
        _DetailSmoothnessScale3("_DetailSmoothnessScale3", Range(0.0, 2.0)) = 1

        //////////////////////////////////////
        /***********Weather Inputs***********/
        //////////////////////////////////////
        [ToggleUI] _WeatherEnable("Weather Enable", Float) = 0.0
        [Enum(Planar, 0, Triplanar, 1)] _RainMode("Rain Mode", Float) = 0

        // Weather Profile
        [HideInInspector] _WeatherProfile("Obsolete, kept for migration purpose", Int) = 0
        [HideInInspector] _WeatherProfileAsset("Weather Profile Asset", Vector) = (0, 0, 0, 0)
        [HideInInspector] _WeatherProfileHash("Weather Profile Hash", Float) = 0
        
        [Normal] _PuddlesNormal("Puddles Normal", 2D) = "bump" {}
        _PuddlesNormalScale("_Puddles Normal Scale", Range(0.0, 8.0)) = 1.0
        _PuddlesFramesSize("Puddles Frames Size", Vector) = (1.0, 1.0, 0.0, 0.0)
        _PuddlesSize("Puddles Size", Float) = 1.0
        _PuddlesAnimationSpeed("Puddles Animation Speed", Float) = 1.0
        
        [Normal] _RainNormal("Rain Normal", 2D) = "bump" {}
        _RainNormalScale("Rain Normal Scale", Range(0.0, 8.0)) = 1.0
        _RainSize("Rain Size", Float) = 1.0
        _RainAnimationSpeed("Rain Animation Speed", Float) = 0.1
        _RainDistortionMap("Rain Distortion Map", 2D) = "black" {}
        _RainDistortionScale("Rain Distortion Scale", Range(0.0, 2.0)) = 0.5
        _RainDistortionSize("Rain Distortion Scale", Float) = 1.0
        _RainMaskMap("Rain Mask Map", 2D) = "white" {}
        _RainWetnessFactor("Rain Wetness Factor", Range(0.0, 1.0)) = 1.0
        
        ///////////////////////////////////////
        /***********Emission Inputs***********/
        ///////////////////////////////////////
        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}
        _EmissionScale("Emission Intensity", Float) = 1.0
        [ToggleUI] _AlbedoAffectEmissive("Albedo Affect Emissive", Float) = 0.0
        [ToggleUI] _EnableEmissionFresnel("Enable Emission Fresnel", Float) = 0.0
        _EmissionFresnelPower("Emission Fresnel Power", Float) = 1.0

        //////////////////////////////////////
        /**********Advanced Options**********/
        //////////////////////////////////////
        [ToggleUI] _CastShadows("Cast Shadows", Float) = 0.0
        [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
        _QueueOffset("Queue offset", Float) = 0.0

        // Specular Occlusion
        [ToggleOff] _HorizonOcclusion("Horizon Occlusion", Float) = 0.0
        _HorizonFade("HorizonFade", Range(0.0, 1.0)) = 0.5
        [Enum(Off, 0, From Ambient Occlusion, 1, From AO and Bent Normals, 2, From GI, 3)] _SpecularOcclusionMode("Specular Occlusion Mode", Int) = 0
        _GIOcclusionBias("GIOcclusion Bias", Range(0.0, 1.0)) = 0.0

        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
        [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
        [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0

        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }

    SubShader
    {
        // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Universal Render Pipeline and Builtin Unity Pipeline
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Lit"
            "IgnoreProjector" = "True"
        }
        LOD 300

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // -------------------------------------
            // Render State Commands
            Blend[_SrcBlend][_DstBlend], [_SrcBlendAlpha][_DstBlendAlpha]
            ZWrite[_ZWrite]
            ZTest[_ZTest]
            Cull[_Cull]
            AlphaToMask[_AlphaToMask]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define BASE_PASS_VARYINGS

            // -------------------------------------
            // Shader Stages
            #pragma vertex LitPassVertex
            #pragma hull Hull
            #pragma domain Domain
            #pragma fragment LitPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local_fragment _ENABLE_GEOMETRIC_SPECULAR_AA

            #pragma shader_feature_local _ _LAYER_MASK_VERTEX_COLOR_MUL _LAYER_MASK_VERTEX_COLOR_ADD
            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _DENSITY_MODE
            #pragma shader_feature_local _HEIGHT_BASED_BLEND
            #pragma shader_feature_local _LAYEREDLIT_3_LAYERS
            #pragma shader_feature_local _LAYEREDLIT_4_LAYERS

            #pragma shader_feature_local _MASKMAP
            #pragma shader_feature_local _MASKMAP1
            #pragma shader_feature_local _MASKMAP2
            #pragma shader_feature_local _MASKMAP3 

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _NORMALMAP1
            #pragma shader_feature_local _NORMALMAP2
            #pragma shader_feature_local _NORMALMAP3

            #pragma shader_feature_local _BENTNORMALMAP
            #pragma shader_feature_local _BENTNORMALMAP1
            #pragma shader_feature_local _BENTNORMALMAP2
            #pragma shader_feature_local _BENTNORMALMAP3

            #pragma shader_feature_local _DETAIL_MAP
            #pragma shader_feature_local _DETAIL_MAP1 
            #pragma shader_feature_local _DETAIL_MAP2 
            #pragma shader_feature_local _DETAIL_MAP3

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            #pragma shader_feature_local _TESSELLATION_DISPLACEMENT
            #pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
            #pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
            #pragma shader_feature_local_domain _TESSELLATION_PHONG
            #pragma shader_feature_local _TESSELLATION_EDGE
            #pragma shader_feature_local _TESSELLATION_DISTANCE

            #pragma shader_feature_local _WEATHER_ON
            #pragma shader_feature_local _RAIN_TRIPLANAR
            #pragma shader_feature_local _RAIN_NORMALMAP

            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _EMISSION_WITH_BASE
            #pragma shader_feature_local_fragment _EMISSION_FRESNEL

            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _BENTNORMAL_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _AO_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _GI_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _HORIZON_SPECULAR_OCCLUSION

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _FORWARD_PLUS
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            // -------------------------------------
            // ShaderQuality keywords
            #pragma multi_compile_fragment _ _WEATHER_ON
            #pragma multi_compile_fragment _ _SHADER_QUALITY_MICRO_SHADOWS
            
            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredLitForwardPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest[_ZTest]
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define SHADOW_PASS_VARYINGS

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma hull Hull
            #pragma domain ShadowDomain
            #pragma fragment ShadowPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SHADOW_CUTOFF

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _HEIGHT_BASED_BLEND

            #pragma shader_feature_local _TESSELLATION_DISPLACEMENT
            #pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
            #pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
            #pragma shader_feature_local_domain _TESSELLATION_PHONG
            #pragma shader_feature_local _TESSELLATION_EDGE
            #pragma shader_feature_local _TESSELLATION_DISTANCE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite[_ZWrite]
            ZTest[_ZTest]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define BASE_PASS_VARYINGS

            // Deferred Rendering Path does not support the OpenGL-based graphics API:
            // Desktop OpenGL, OpenGL ES 3.0, WebGL 2.0.
            #pragma exclude_renderers gles3 glcore

            // -------------------------------------
            // Shader Stages
            #pragma vertex LitGBufferPassVertex
            #pragma hull Hull
            #pragma domain Domain
            #pragma fragment LitGBufferPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            //#pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local_fragment _ENABLE_GEOMETRIC_SPECULAR_AA

            #pragma shader_feature_local _ _LAYER_MASK_VERTEX_COLOR_MUL _LAYER_MASK_VERTEX_COLOR_ADD
            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _DENSITY_MODE
            #pragma shader_feature_local _HEIGHT_BASED_BLEND
            #pragma shader_feature_local _LAYEREDLIT_3_LAYERS
            #pragma shader_feature_local _LAYEREDLIT_4_LAYERS

            #pragma shader_feature_local _MASKMAP
            #pragma shader_feature_local _MASKMAP1
            #pragma shader_feature_local _MASKMAP2
            #pragma shader_feature_local _MASKMAP3 

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _NORMALMAP1
            #pragma shader_feature_local _NORMALMAP2
            #pragma shader_feature_local _NORMALMAP3

            #pragma shader_feature_local _BENTNORMALMAP
            #pragma shader_feature_local _BENTNORMALMAP1
            #pragma shader_feature_local _BENTNORMALMAP2
            #pragma shader_feature_local _BENTNORMALMAP3

            #pragma shader_feature_local _DETAIL_MAP
            #pragma shader_feature_local _DETAIL_MAP1 
            #pragma shader_feature_local _DETAIL_MAP2 
            #pragma shader_feature_local _DETAIL_MAP3

            #pragma shader_feature_local _TESSELLATION_DISPLACEMENT
            #pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
            #pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
            #pragma shader_feature_local_domain _TESSELLATION_PHONG
            #pragma shader_feature_local _TESSELLATION_EDGE
            #pragma shader_feature_local _TESSELLATION_DISTANCE

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            #pragma shader_feature_local _WEATHER_ON
            #pragma shader_feature_local _RAIN_TRIPLANAR
            #pragma shader_feature_local _RAIN_NORMALMAP

            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _EMISSION_WITH_BASE
            #pragma shader_feature_local_fragment _EMISSION_FRESNEL

            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _BENTNORMAL_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _AO_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _GI_SPECULAR_OCCLUSION
            #pragma shader_feature_local_fragment _HORIZON_SPECULAR_OCCLUSION

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            // -------------------------------------
            // ShaderQuality keywords
            #pragma multi_compile_fragment _ _WEATHER_ON
            #pragma multi_compile_fragment _ _SHADER_QUALITY_MICRO_SHADOWS

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredLitGBufferPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest[_ZTest]
            ColorMask R
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define DEPTH_PASS_VARYINGS

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma hull Hull
            #pragma domain DepthDomain
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SHADOW_CUTOFF

            #pragma shader_feature_local _ _LAYER_MASK_VERTEX_COLOR_MUL _LAYER_MASK_VERTEX_COLOR_ADD
            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _DENSITY_MODE
            #pragma shader_feature_local _HEIGHT_BASED_BLEND
            #pragma shader_feature_local _LAYEREDLIT_3_LAYERS
            #pragma shader_feature_local _LAYEREDLIT_4_LAYERS

            #pragma shader_feature_local _TESSELLATION_DISPLACEMENT
            #pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
            #pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
            #pragma shader_feature_local_domain _TESSELLATION_PHONG
            #pragma shader_feature_local _TESSELLATION_EDGE
            #pragma shader_feature_local _TESSELLATION_DISTANCE

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredDepthOnlyPass.hlsl"
            ENDHLSL
        }

        // This pass is used when drawing to a _CameraNormalsTexture texture
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest[_ZTest]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define DEPTH_NORMALS_PASS_VARYINGS

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthNormalsVertex
            #pragma hull Hull
            #pragma domain DepthNormalsDomain
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SHADOW_CUTOFF
            #pragma shader_feature_local _DOUBLESIDED_ON

            #pragma shader_feature_local _ _LAYER_MASK_VERTEX_COLOR_MUL _LAYER_MASK_VERTEX_COLOR_ADD
            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _DENSITY_MODE
            #pragma shader_feature_local _HEIGHT_BASED_BLEND
            #pragma shader_feature_local _LAYEREDLIT_3_LAYERS
            #pragma shader_feature_local _LAYEREDLIT_4_LAYERS
            
            #pragma shader_feature_local _TESSELLATION_DISPLACEMENT
            #pragma shader_feature_local _VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE
            #pragma shader_feature_local _DISPLACEMENT_LOCK_TILING_SCALE
            #pragma shader_feature_local_domain _TESSELLATION_PHONG
            #pragma shader_feature_local _TESSELLATION_EDGE
            #pragma shader_feature_local _TESSELLATION_DISTANCE

            #pragma shader_feature_local _MASKMAP
            #pragma shader_feature_local _MASKMAP1 
            #pragma shader_feature_local _MASKMAP2 
            #pragma shader_feature_local _MASKMAP3

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _NORMALMAP1
            #pragma shader_feature_local _NORMALMAP2
            #pragma shader_feature_local _NORMALMAP3

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            #pragma shader_feature_local _DETAIL_MAP
            #pragma shader_feature_local _DETAIL_MAP1 
            #pragma shader_feature_local _DETAIL_MAP2 
            #pragma shader_feature_local _DETAIL_MAP3

            // -------------------------------------
            // ShaderQuality keywords
            #pragma multi_compile_fragment _ _SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // -------------------------------------
            // Universal Pipeline keywords
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredDepthNormalsPass.hlsl"
            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }

            // -------------------------------------
            // Render State Commands
            Cull Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma require tessellation tessHW

            #define LAYERED_LIT
            #define TESSELLATION_ON
            #define BASE_PASS_VARYINGS

            // -------------------------------------
            // Shader Stages
            #pragma vertex LayeredLitVertexMeta
            #pragma fragment LayeredLitFragmentMeta

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local _ _LAYER_MASK_VERTEX_COLOR_MUL _LAYER_MASK_VERTEX_COLOR_ADD
            #pragma shader_feature_local _MAIN_LAYER_INFLUENCE_MODE
            #pragma shader_feature_local _INFLUENCEMASK_MAP
            #pragma shader_feature_local _DENSITY_MODE
            #pragma shader_feature_local _HEIGHT_BASED_BLEND
            #pragma shader_feature_local _LAYEREDLIT_3_LAYERS
            #pragma shader_feature_local _LAYEREDLIT_4_LAYERS

            #pragma shader_feature_local _MASKMAP
            #pragma shader_feature_local _MASKMAP1
            #pragma shader_feature_local _MASKMAP2
            #pragma shader_feature_local _MASKMAP3

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _NORMALMAP1
            #pragma shader_feature_local _NORMALMAP2
            #pragma shader_feature_local _NORMALMAP3

            #pragma shader_feature_local _DETAIL_MAP
            #pragma shader_feature_local _DETAIL_MAP1 
            #pragma shader_feature_local _DETAIL_MAP2 
            #pragma shader_feature_local _DETAIL_MAP3

            #pragma shader_feature_local _HEIGHTMAP
            #pragma shader_feature_local _HEIGHTMAP1
            #pragma shader_feature_local _HEIGHTMAP2
            #pragma shader_feature_local _HEIGHTMAP3

            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _EMISSION_WITH_BASE

            #pragma shader_feature EDITOR_VISUALIZATION

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/LayeredLit/LayeredMetaPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "Universal2D"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // -------------------------------------
            // Render State Commands
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex vert
            #pragma fragment frag

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

            // -------------------------------------
            // Includes
            #include "ShaderLibrary/LayeredLit/LayeredLitInput.hlsl"
            #include "ShaderLibrary/Universal2D.hlsl"
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "KeroTools.URPPlus.Editor.ShaderGUI.LayeredLitTessellationGUI"
}
