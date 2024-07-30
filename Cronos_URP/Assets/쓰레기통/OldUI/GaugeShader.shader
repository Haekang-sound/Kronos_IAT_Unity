Shader "Shader Graphs/UI_CP"
{
    Properties
    {
        [NoScaleOffset]_mainTex("mainTex", 2D) = "white" {}
        _CPAmount("CPAmount", Range(0, 1)) = 0.7
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHATEST_ON 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _mainTex_TexelSize;
        float _CPAmount;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_mainTex);
        SAMPLER(sampler_mainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Ellipse_float(float2 UV, float Width, float Height, out float Out)
        {
        #if defined(SHADER_STAGE_RAY_TRACING)
            Out = saturate((1.0 - length((UV * 2 - 1) / float2(Width, Height))) * 1e7);
        #else
            float d = length((UV * 2 - 1) / float2(Width, Height));
            Out = saturate((1 - d) / fwidth(d));
        #endif
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_ColorMask_float(float3 In, float3 MaskColor, float Range, out float Out, float Fuzziness)
        {
            float Distance = distance(MaskColor, In);
            Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Arctangent2_float(float A, float B, out float Out)
        {
            Out = atan2(A, B);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_aadb977045054b94b6cb7c2eeb8c541d_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_mainTex);
            float4 _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_aadb977045054b94b6cb7c2eeb8c541d_Out_0_Texture2D.tex, _Property_aadb977045054b94b6cb7c2eeb8c541d_Out_0_Texture2D.samplerstate, _Property_aadb977045054b94b6cb7c2eeb8c541d_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_R_4_Float = _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4.r;
            float _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_G_5_Float = _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4.g;
            float _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_B_6_Float = _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4.b;
            float _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_A_7_Float = _SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4.a;
            float _Ellipse_0bec068b28374668a9ed02bf70545388_Out_4_Float;
            Unity_Ellipse_float(IN.uv0.xy, 1, 1, _Ellipse_0bec068b28374668a9ed02bf70545388_Out_4_Float);
            float4 _Multiply_95decb668db44b0ab97d813757c3f3ad_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4, (_Ellipse_0bec068b28374668a9ed02bf70545388_Out_4_Float.xxxx), _Multiply_95decb668db44b0ab97d813757c3f3ad_Out_2_Vector4);
            float _ColorMask_3218eb8a61fe478d8c317f75ff682f65_Out_3_Float;
            Unity_ColorMask_float((_SampleTexture2D_d9280dff93ec4ff988f875aa5821836a_RGBA_0_Vector4.xyz), IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0)), 0, _ColorMask_3218eb8a61fe478d8c317f75ff682f65_Out_3_Float, 0.14);
            float _InvertColors_d51fdabb32e949b08e2771ad4b271132_Out_1_Float;
            float _InvertColors_d51fdabb32e949b08e2771ad4b271132_InvertColors = float (1);
            Unity_InvertColors_float(_ColorMask_3218eb8a61fe478d8c317f75ff682f65_Out_3_Float, _InvertColors_d51fdabb32e949b08e2771ad4b271132_InvertColors, _InvertColors_d51fdabb32e949b08e2771ad4b271132_Out_1_Float);
            float2 _Rotate_98641a6415f04c14864d5134bdf67f59_Out_3_Vector2;
            Unity_Rotate_Radians_float(IN.uv0.xy, float2 (0.5, 0.5), 3.14, _Rotate_98641a6415f04c14864d5134bdf67f59_Out_3_Vector2);
            float2 _TilingAndOffset_9de9540eed8443c28cce0160e89548d9_Out_3_Vector2;
            Unity_TilingAndOffset_float(_Rotate_98641a6415f04c14864d5134bdf67f59_Out_3_Vector2, float2 (2, 2), float2 (-1, -1), _TilingAndOffset_9de9540eed8443c28cce0160e89548d9_Out_3_Vector2);
            float _Split_3f9bc507124846e283b292084c1072fb_R_1_Float = _TilingAndOffset_9de9540eed8443c28cce0160e89548d9_Out_3_Vector2[0];
            float _Split_3f9bc507124846e283b292084c1072fb_G_2_Float = _TilingAndOffset_9de9540eed8443c28cce0160e89548d9_Out_3_Vector2[1];
            float _Split_3f9bc507124846e283b292084c1072fb_B_3_Float = 0;
            float _Split_3f9bc507124846e283b292084c1072fb_A_4_Float = 0;
            float _Arctangent2_5da904297b014a689c2ea45173b1b4f9_Out_2_Float;
            Unity_Arctangent2_float(_Split_3f9bc507124846e283b292084c1072fb_R_1_Float, _Split_3f9bc507124846e283b292084c1072fb_G_2_Float, _Arctangent2_5da904297b014a689c2ea45173b1b4f9_Out_2_Float);
            float Constant_1acb628881784f33a449faa35c2d8b31 = 3.141593;
            float _Multiply_fb8e14fe2128428dbbee56b884fbc941_Out_2_Float;
            Unity_Multiply_float_float(Constant_1acb628881784f33a449faa35c2d8b31, -1, _Multiply_fb8e14fe2128428dbbee56b884fbc941_Out_2_Float);
            float2 _Vector2_da352f5dd4754993956d817ab28c5e5e_Out_0_Vector2 = float2(_Multiply_fb8e14fe2128428dbbee56b884fbc941_Out_2_Float, Constant_1acb628881784f33a449faa35c2d8b31);
            float _Remap_69fac6b911034ee99e4d873111137b33_Out_3_Float;
            Unity_Remap_float(_Arctangent2_5da904297b014a689c2ea45173b1b4f9_Out_2_Float, _Vector2_da352f5dd4754993956d817ab28c5e5e_Out_0_Vector2, float2 (0, 1), _Remap_69fac6b911034ee99e4d873111137b33_Out_3_Float);
            float _Property_540c328a01634f02aa377868d8334447_Out_0_Float = _CPAmount;
            float _Step_d275a3e90ba9454f8d03475ed19bacbe_Out_2_Float;
            Unity_Step_float(_Remap_69fac6b911034ee99e4d873111137b33_Out_3_Float, _Property_540c328a01634f02aa377868d8334447_Out_0_Float, _Step_d275a3e90ba9454f8d03475ed19bacbe_Out_2_Float);
            float _Multiply_41363508a1c74a33a325f0ac5ca7d8f1_Out_2_Float;
            Unity_Multiply_float_float(_Ellipse_0bec068b28374668a9ed02bf70545388_Out_4_Float, _Step_d275a3e90ba9454f8d03475ed19bacbe_Out_2_Float, _Multiply_41363508a1c74a33a325f0ac5ca7d8f1_Out_2_Float);
            float _Multiply_432fd5d8c4dc464cacfc0a751cb8c042_Out_2_Float;
            Unity_Multiply_float_float(_InvertColors_d51fdabb32e949b08e2771ad4b271132_Out_1_Float, _Multiply_41363508a1c74a33a325f0ac5ca7d8f1_Out_2_Float, _Multiply_432fd5d8c4dc464cacfc0a751cb8c042_Out_2_Float);
            surface.BaseColor = (_Multiply_95decb668db44b0ab97d813757c3f3ad_Out_2_Vector4.xyz);
            surface.Alpha = _Multiply_432fd5d8c4dc464cacfc0a751cb8c042_Out_2_Float;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}