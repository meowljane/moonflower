Shader"Custom/BorderBlackout"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 텍스처 프로퍼티
        _BorderSize ("Border Size", Range(0, 0.5)) = 0.1 // 테두리 두께
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Tags { "RenderType"="Transparent" }
//LOD100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex; // 메인 텍스처
float4 _MainTex_ST; // 텍스처 변환
float _BorderSize; // 테두리 두께 (0~0.5)

struct appdata_t
{
    float4 vertex : POSITION; // 버텍스 위치
    float2 texcoord : TEXCOORD0; // 텍스처 좌표
};

struct v2f
{
    float2 uv : TEXCOORD0; // 텍스처 좌표
    float4 vertex : SV_POSITION; // 클립 공간 위치
};

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex); // 오브젝트 공간 -> 클립 공간
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); // 텍스처 좌표 변환
    return o;
}

float4 frag(v2f i) : SV_Target
{
    float2 uv = i.uv;

                // 테두리 영역 계산
    float borderFactor = 0.0;
    borderFactor += step(1.0 - _BorderSize, uv.x); // 오른쪽 테두리
    borderFactor += step(1.0 - _BorderSize, uv.y); // 위쪽 테두리
    borderFactor += step(uv.x, _BorderSize); // 왼쪽 테두리
    borderFactor += step(uv.y, _BorderSize); // 아래쪽 테두리

                // 테두리 영역에 검정색 적용
    if (borderFactor > 0.0)
    {
        return float4(0, 0, 0, 1); // 검정색
    }

                // 테두리 이외 영역은 원래 텍스처 유지
    return tex2D(_MainTex, uv);
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
