Shader "Unlit/ImageShader"
{
    Properties
    {
        _DissolveSpeed("DissolveSpeed", Vector) = (0.5, 0, 0, 0)
        _DissolveScale("DissolveScale", Float) = 35
        _MainTexSpeed("MainTexSpeed", Vector) = (0.1, 0, 0, 0)
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _Color01("Color01", Color) = (1, 0.06640472, 0, 0)
        _Color02("Color02", Color) = (1, 0.7722036, 0, 0)
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
             float3 TimeParameters;
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
        float2 _DissolveSpeed;
        float _DissolveScale;
        float2 _MainTexSpeed;
        float4 _MainTex_TexelSize;
        float4 _Color01;
        float4 _Color02;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
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
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Clamp_float4(float4 In, float4 Min, float4 Max, out float4 Out)
        {
            Out = clamp(In, Min, Max);
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
            float4 _Property_8c71829d523b4e9484f1b8a7f7bc6019_Out_0_Vector4 = _Color01;
            float4 _Property_f0ffcd05593048b08b2f3722516a1dd7_Out_0_Vector4 = _Color02;
            float4 _UV_09fb1a31000d43e2bc71305d0d9a5880_Out_0_Vector4 = IN.uv0;
            float _Split_80e5dd565aa2413684f78f590421158f_R_1_Float = _UV_09fb1a31000d43e2bc71305d0d9a5880_Out_0_Vector4[0];
            float _Split_80e5dd565aa2413684f78f590421158f_G_2_Float = _UV_09fb1a31000d43e2bc71305d0d9a5880_Out_0_Vector4[1];
            float _Split_80e5dd565aa2413684f78f590421158f_B_3_Float = _UV_09fb1a31000d43e2bc71305d0d9a5880_Out_0_Vector4[2];
            float _Split_80e5dd565aa2413684f78f590421158f_A_4_Float = _UV_09fb1a31000d43e2bc71305d0d9a5880_Out_0_Vector4[3];
            float4 _Lerp_2e69739165104601a113800959468bed_Out_3_Vector4;
            Unity_Lerp_float4(_Property_8c71829d523b4e9484f1b8a7f7bc6019_Out_0_Vector4, _Property_f0ffcd05593048b08b2f3722516a1dd7_Out_0_Vector4, (_Split_80e5dd565aa2413684f78f590421158f_R_1_Float.xxxx), _Lerp_2e69739165104601a113800959468bed_Out_3_Vector4);
            float2 _Property_a6e05a0f547d48edb3862bb2474d374c_Out_0_Vector2 = _DissolveSpeed;
            float2 _Multiply_49a2a7cc0d444b56923def3b103825cf_Out_2_Vector2;
            Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Property_a6e05a0f547d48edb3862bb2474d374c_Out_0_Vector2, _Multiply_49a2a7cc0d444b56923def3b103825cf_Out_2_Vector2);
            float2 _TilingAndOffset_b00d3537adae42aa960353562bfddb0c_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_49a2a7cc0d444b56923def3b103825cf_Out_2_Vector2, _TilingAndOffset_b00d3537adae42aa960353562bfddb0c_Out_3_Vector2);
            float _Property_327c3ed5200b4005b460783e576d5dd7_Out_0_Float = _DissolveScale;
            float _SimpleNoise_7c1cb8ac777d41e88604edd9efcbabf9_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_b00d3537adae42aa960353562bfddb0c_Out_3_Vector2, _Property_327c3ed5200b4005b460783e576d5dd7_Out_0_Float, _SimpleNoise_7c1cb8ac777d41e88604edd9efcbabf9_Out_2_Float);
            float _Add_09290358605243028d841bef5df0d5ca_Out_2_Float;
            Unity_Add_float(_SimpleNoise_7c1cb8ac777d41e88604edd9efcbabf9_Out_2_Float, 0, _Add_09290358605243028d841bef5df0d5ca_Out_2_Float);
            float _Subtract_b005f2b60153484fbb9120bf033b73a7_Out_2_Float;
            Unity_Subtract_float(_Add_09290358605243028d841bef5df0d5ca_Out_2_Float, 0.62, _Subtract_b005f2b60153484fbb9120bf033b73a7_Out_2_Float);
            UnityTexture2D _Property_14a5bfe2b3c2453daa6445643a0507e4_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_ec47529a5d5648a09208cdead8acab27_Out_0_Vector2 = _MainTexSpeed;
            float2 _Multiply_3e95ac9c6bc348968ffa4b950c363957_Out_2_Vector2;
            Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Property_ec47529a5d5648a09208cdead8acab27_Out_0_Vector2, _Multiply_3e95ac9c6bc348968ffa4b950c363957_Out_2_Vector2);
            float2 _TilingAndOffset_3ce15352e3b6411fb674b8037aac0789_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_3e95ac9c6bc348968ffa4b950c363957_Out_2_Vector2, _TilingAndOffset_3ce15352e3b6411fb674b8037aac0789_Out_3_Vector2);
            float4 _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_14a5bfe2b3c2453daa6445643a0507e4_Out_0_Texture2D.tex, _Property_14a5bfe2b3c2453daa6445643a0507e4_Out_0_Texture2D.samplerstate, _Property_14a5bfe2b3c2453daa6445643a0507e4_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_3ce15352e3b6411fb674b8037aac0789_Out_3_Vector2) );
            float _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_R_4_Float = _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4.r;
            float _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_G_5_Float = _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4.g;
            float _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_B_6_Float = _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4.b;
            float _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_A_7_Float = _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4.a;
            float4 _Add_33cec18af20f4ab3956cbbe2cb3ef1e9_Out_2_Vector4;
            Unity_Add_float4((_Subtract_b005f2b60153484fbb9120bf033b73a7_Out_2_Float.xxxx), _SampleTexture2D_7d057347dae84d61b8ea8234d23dd1b1_RGBA_0_Vector4, _Add_33cec18af20f4ab3956cbbe2cb3ef1e9_Out_2_Vector4);
            float4 _Multiply_3b13d59dcfc54fdba3d7ed57a0e6e854_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Lerp_2e69739165104601a113800959468bed_Out_3_Vector4, _Add_33cec18af20f4ab3956cbbe2cb3ef1e9_Out_2_Vector4, _Multiply_3b13d59dcfc54fdba3d7ed57a0e6e854_Out_2_Vector4);
            float4 _Clamp_23652b5742d44d4ebe2901b5b1d84266_Out_3_Vector4;
            Unity_Clamp_float4(_Add_33cec18af20f4ab3956cbbe2cb3ef1e9_Out_2_Vector4, float4(0, 0, 0, 0), float4(1, 1, 1, 1), _Clamp_23652b5742d44d4ebe2901b5b1d84266_Out_3_Vector4);
            surface.BaseColor = (_Multiply_3b13d59dcfc54fdba3d7ed57a0e6e854_Out_2_Vector4.xyz);
            surface.Alpha = (_Clamp_23652b5742d44d4ebe2901b5b1d84266_Out_3_Vector4).x;
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
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
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