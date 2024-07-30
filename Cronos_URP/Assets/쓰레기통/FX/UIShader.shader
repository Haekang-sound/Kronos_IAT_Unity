Shader "Unlit/UIShader"
{
    Properties
    {
        [HDR]_Color01("Color01", Color) = (0.7924528, 0, 0.09362093, 1)
        [HDR]_Color02("Color02", Color) = (0, 0.04494321, 0.5943396, 1)
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _MainTexSpeed("MainTexSpeed", Vector) = (-0.1, 0, 0, 0)
        _DissolveScale("DissolveScale", Float) = 35
        _DissolveSpeed("DissolveSpeed", Vector) = (-0.5, 0, 0, 0)
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
        float4 _Color01;
        float4 _Color02;
        float4 _MainTex_TexelSize;
        float2 _MainTexSpeed;
        float _DissolveScale;
        float2 _DissolveSpeed;
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
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
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
            float4 _Property_a8b33c0f63b0477ba9f5fbe448f40907_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color01) : _Color01;
            float4 _Property_b47fb6eb78d74531abad8af47666a4ed_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color02) : _Color02;
            float4 _UV_781cdd35a9864c6fbca886d1f98319da_Out_0_Vector4 = IN.uv0;
            float _Split_0cb656e52a2f45cbb9e4b99813248db3_R_1_Float = _UV_781cdd35a9864c6fbca886d1f98319da_Out_0_Vector4[0];
            float _Split_0cb656e52a2f45cbb9e4b99813248db3_G_2_Float = _UV_781cdd35a9864c6fbca886d1f98319da_Out_0_Vector4[1];
            float _Split_0cb656e52a2f45cbb9e4b99813248db3_B_3_Float = _UV_781cdd35a9864c6fbca886d1f98319da_Out_0_Vector4[2];
            float _Split_0cb656e52a2f45cbb9e4b99813248db3_A_4_Float = _UV_781cdd35a9864c6fbca886d1f98319da_Out_0_Vector4[3];
            float4 _Lerp_9d22fa03c255477ca007161e5f8640ff_Out_3_Vector4;
            Unity_Lerp_float4(_Property_a8b33c0f63b0477ba9f5fbe448f40907_Out_0_Vector4, _Property_b47fb6eb78d74531abad8af47666a4ed_Out_0_Vector4, (_Split_0cb656e52a2f45cbb9e4b99813248db3_R_1_Float.xxxx), _Lerp_9d22fa03c255477ca007161e5f8640ff_Out_3_Vector4);
            float2 _Property_46483bbbf0be471fabd55a566d726f4e_Out_0_Vector2 = _DissolveSpeed;
            float2 _Multiply_d669a6dc669f43a08c142e858ea07bfa_Out_2_Vector2;
            Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Property_46483bbbf0be471fabd55a566d726f4e_Out_0_Vector2, _Multiply_d669a6dc669f43a08c142e858ea07bfa_Out_2_Vector2);
            float2 _TilingAndOffset_c41503e9d5e947b29c94da9da5625280_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_d669a6dc669f43a08c142e858ea07bfa_Out_2_Vector2, _TilingAndOffset_c41503e9d5e947b29c94da9da5625280_Out_3_Vector2);
            float _Property_683c5b9ccea142e784a98d2b8fd8f244_Out_0_Float = _DissolveScale;
            float _SimpleNoise_7333dafed8564d83a2e64e6756d2c14d_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_c41503e9d5e947b29c94da9da5625280_Out_3_Vector2, _Property_683c5b9ccea142e784a98d2b8fd8f244_Out_0_Float, _SimpleNoise_7333dafed8564d83a2e64e6756d2c14d_Out_2_Float);
            float4 _UV_a38f606c99984e439d4174c67f8479ca_Out_0_Vector4 = IN.uv0;
            float _Split_65989f823fd14aa683c9c2269b0268ac_R_1_Float = _UV_a38f606c99984e439d4174c67f8479ca_Out_0_Vector4[0];
            float _Split_65989f823fd14aa683c9c2269b0268ac_G_2_Float = _UV_a38f606c99984e439d4174c67f8479ca_Out_0_Vector4[1];
            float _Split_65989f823fd14aa683c9c2269b0268ac_B_3_Float = _UV_a38f606c99984e439d4174c67f8479ca_Out_0_Vector4[2];
            float _Split_65989f823fd14aa683c9c2269b0268ac_A_4_Float = _UV_a38f606c99984e439d4174c67f8479ca_Out_0_Vector4[3];
            float _OneMinus_6966fee487b54f16aed3bfb9235cd611_Out_1_Float;
            Unity_OneMinus_float(_Split_65989f823fd14aa683c9c2269b0268ac_R_1_Float, _OneMinus_6966fee487b54f16aed3bfb9235cd611_Out_1_Float);
            float _Add_35e0a0104924426cb8ab6b2ad0a4b9d4_Out_2_Float;
            Unity_Add_float(_SimpleNoise_7333dafed8564d83a2e64e6756d2c14d_Out_2_Float, _OneMinus_6966fee487b54f16aed3bfb9235cd611_Out_1_Float, _Add_35e0a0104924426cb8ab6b2ad0a4b9d4_Out_2_Float);
            float _Subtract_62046fa611524e448959f247f1968194_Out_2_Float;
            Unity_Subtract_float(_Add_35e0a0104924426cb8ab6b2ad0a4b9d4_Out_2_Float, _Split_65989f823fd14aa683c9c2269b0268ac_R_1_Float, _Subtract_62046fa611524e448959f247f1968194_Out_2_Float);
            UnityTexture2D _Property_6cd99a49e900471aaa791d42e55b6da8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_fdccc9b2bad74015852f83ce8fecb77c_Out_0_Vector2 = _MainTexSpeed;
            float2 _Multiply_2e9016737e18479fb87f84963965ca3f_Out_2_Vector2;
            Unity_Multiply_float2_float2((IN.TimeParameters.x.xx), _Property_fdccc9b2bad74015852f83ce8fecb77c_Out_0_Vector2, _Multiply_2e9016737e18479fb87f84963965ca3f_Out_2_Vector2);
            float2 _TilingAndOffset_cb8aebc297344512966e7c1ed9625b98_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_2e9016737e18479fb87f84963965ca3f_Out_2_Vector2, _TilingAndOffset_cb8aebc297344512966e7c1ed9625b98_Out_3_Vector2);
            float4 _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_6cd99a49e900471aaa791d42e55b6da8_Out_0_Texture2D.tex, _Property_6cd99a49e900471aaa791d42e55b6da8_Out_0_Texture2D.samplerstate, _Property_6cd99a49e900471aaa791d42e55b6da8_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_cb8aebc297344512966e7c1ed9625b98_Out_3_Vector2) );
            float _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_R_4_Float = _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4.r;
            float _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_G_5_Float = _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4.g;
            float _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_B_6_Float = _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4.b;
            float _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_A_7_Float = _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4.a;
            float4 _Multiply_41238df941a04a858c0f0dc2e5c8ee90_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Subtract_62046fa611524e448959f247f1968194_Out_2_Float.xxxx), _SampleTexture2D_cfddffc0d9b0469c9cecb97dadd5e408_RGBA_0_Vector4, _Multiply_41238df941a04a858c0f0dc2e5c8ee90_Out_2_Vector4);
            float4 _Multiply_45fb29f8c06c4f80b5b0b5f81c52a559_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Lerp_9d22fa03c255477ca007161e5f8640ff_Out_3_Vector4, _Multiply_41238df941a04a858c0f0dc2e5c8ee90_Out_2_Vector4, _Multiply_45fb29f8c06c4f80b5b0b5f81c52a559_Out_2_Vector4);
            float4 _Clamp_39f3118cc27c473fb4e7446f18d431c4_Out_3_Vector4;
            Unity_Clamp_float4(_Multiply_41238df941a04a858c0f0dc2e5c8ee90_Out_2_Vector4, float4(0, 0, 0, 0), float4(1, 1, 1, 1), _Clamp_39f3118cc27c473fb4e7446f18d431c4_Out_3_Vector4);
            surface.BaseColor = (_Multiply_45fb29f8c06c4f80b5b0b5f81c52a559_Out_2_Vector4.xyz);
            surface.Alpha = (_Clamp_39f3118cc27c473fb4e7446f18d431c4_Out_3_Vector4).x;
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
