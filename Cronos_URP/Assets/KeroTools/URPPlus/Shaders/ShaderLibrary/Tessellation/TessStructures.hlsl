//BasePass Tessellation Structures
#if defined(BASE_PASS_VARYINGS)
struct TessellationControlPoint
{
    float4 positionOS         : INTERNALTESSPOS;
    float3 normalOS           : NORMAL;
    float4 tangentOS          : TANGENT;
    float2 texcoord           : TEXCOORD0;
    float2 staticLightmapUV   : TEXCOORD1;
    float2 dynamicLightmapUV  : TEXCOORD2;
#if defined(_VERTEX_COLOR)
    half4 vertexColor         : COLOR;
#endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif

//DepthPass Tessellation Structures
#if defined(DEPTH_PASS_VARYINGS)
struct TessellationControlPoint
{
    float4 positionOS : INTERNALTESSPOS;
    float3 normalOS   : NORMAL;
    float2 texcoord   : TEXCOORD0;
    float4 positionCS : SV_POSITION;
#if defined(_VERTEX_COLOR)
    half4 vertexColor : COLOR;
#endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif

//ShadowPass Tessellation Structures
#if defined(SHADOW_PASS_VARYINGS)
struct TessellationControlPoint
{
    float4 positionOS : INTERNALTESSPOS;
    float3 normalOS   : NORMAL;
    float2 texcoord   : TEXCOORD0;
#if defined(_VERTEX_COLOR)
    half4 vertexColor : COLOR;
#endif
    
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif

//DepthNormalsPass Tessellation Structures
#if defined(DEPTH_NORMALS_PASS_VARYINGS)
struct TessellationControlPoint
{
    float4 positionOS : INTERNALTESSPOS;
    float3 normalOS   : NORMAL;
    float4 tangentOS  : TANGENT;
    float2 texcoord   : TEXCOORD0;
#if defined(_VERTEX_COLOR)
    half4 vertexColor : COLOR;
#endif
    
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif

//MetaPass Tessellation Structures
#if defined(META_PASS_VARYINGS)
struct TessellationControlPoint
{
    float4 positionOS   : INTERNALTESSPOS;
    float3 normalOS     : NORMAL;
    float2 uv0          : TEXCOORD0;
    float2 uv1          : TEXCOORD1;
    float2 uv2          : TEXCOORD2;
#if defined(_VERTEX_COLOR)
    half4 vertexColor   : COLOR;
#endif
};
#endif