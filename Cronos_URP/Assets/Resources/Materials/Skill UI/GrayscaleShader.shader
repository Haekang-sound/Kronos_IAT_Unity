Shader "KJYCustum/GrayscaleShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GrayscaleAmount("Grayscale Amount", Range(0, 1)) = 1.0 // 그레이스케일 정도를 조절하는 변수 추가
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
 
        Pass 
        {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
 
                #include "UnityCG.cginc"
 
                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
 
                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
 
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _GrayscaleAmount; // 그레이스케일 정도를 나타내는 변수 선언
 
                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }
 
                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
                    col.rgb = lerp(col.rgb, float3(grey, grey, grey), _GrayscaleAmount); // 컬러와 그레이스케일 값을 혼합
                    return fixed4(col.rgb, col.a);
                }
                ENDCG
        }        
    }
    FallBack "Diffuse"
}