Shader "Hidden/DowscalePostProces"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Ratio("Ratio", Float) = 0.25
        _X("X", Float) = 1
        _Y("Y", Float) = 1
        _PixelSize("Pixel Size", Float) = 0.5
        _Threshold("Threshold", Float) = 0.5
        _QuantizeLevels("_QuantizeLevels", Float) = 8
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma enable_d3d11_debug_symbols

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Ratio;
            float _X;
            float _Y;
            float _PixelSize;
            float _Threshold;
            float _QuantizeLevels;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenSize = _ScreenParams.xy / _PixelSize;

                float2 newUV = (floor((i.uv + 0.5) * screenSize)) / screenSize - 0.5;

                float4 col = tex2D(_MainTex, newUV);

                /*float2 uv = floor(i.uv / _PixelSize) * _PixelSize + _PixelSize * 0.5;
                float4 col = tex2D(_MainTex, uv);*/
                /*float4 dither = frac(sin(dot(i.uv, float2(12.9898, 78.233))) * 43758.5453) - 0.5;
                return lerp(col, 0.5, dither);*/

                //col = floor(col * _QuantizeLevels) / (_QuantizeLevels - 1);


                //col = step(col, _Threshold);

                return col;

                /*float2 aspectRatioAdjustedUV = i.uv;
                aspectRatioAdjustedUV.x *= _ScreenParams.y / _ScreenParams.x;
                aspectRatioAdjustedUV = floor(aspectRatioAdjustedUV * _PixelSize) / _PixelSize;
                float4 col = tex2D(_MainTex, aspectRatioAdjustedUV);
                return col;*/



            }
            ENDCG
        }
    }
}
