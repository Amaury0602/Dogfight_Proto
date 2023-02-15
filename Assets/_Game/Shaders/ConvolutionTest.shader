Shader "Hidden/ConvolutionTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeWidth("Width" ,Float) = 0
        _Step("ColorStep" , Range(0, 1)) = 0

        _FirstColor("First", Color) = (0,0,0,0)
        _SecondColor("Second", Color) = (0,0,0,0)
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
                float2 screenPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float grayScale(float3 col) 
            {
                return col.r * 0.299 + col.g * 0.587 + col.b * 0.114;
            }

            int3x3 fMatrix = { 
                1, 2, 1,// row 1
                2, 4, 2,// row 2
                1, 2, 1,// row 1
            };

            fixed4 _FirstColor;
            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_TexelSize;
            float4 _MainTex_TexelSize;
            float _Step;
            float _EdgeWidth;



            fixed4 frag(v2f i) : SV_Target
            {
                float2 texSize = float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * _EdgeWidth;
                float depthValue = tex2D(_CameraDepthTexture, i.uv);

                float bottomLeft = tex2D(_CameraDepthTexture, i.uv + float2(-texSize.x, -texSize.y));
                float bottom = tex2D(_CameraDepthTexture, i.uv + float2(0, -texSize.y));
                float bottomRight = tex2D(_CameraDepthTexture, i.uv + float2(texSize.x, -texSize.y));
                float left = tex2D(_CameraDepthTexture, i.uv + float2(-texSize.x, 0));
                float center = depthValue;
                float right = tex2D(_CameraDepthTexture, i.uv + float2(texSize.x, 0));
                float topLeft = tex2D(_CameraDepthTexture, i.uv + float2(-texSize.x, texSize.y));
                float top = tex2D(_CameraDepthTexture, i.uv + float2(0, texSize.y));
                float topRight = tex2D(_CameraDepthTexture, i.uv + float2(texSize.x, texSize.y));

                float SobelHorizontal = bottomRight + (2.0 * right) + topRight - (bottomLeft + (2.0 * left) + topLeft);

                float SobelVertical = bottomLeft + (2.0 * bottom) + bottomRight - (topLeft + (2.0 * top) + topRight);

                float sobel = sqrt((SobelHorizontal * SobelHorizontal) + (SobelVertical * SobelVertical));

                float s = step(_Step, sobel);

                fixed3 ss = fixed3(1, 1, 1) * s;

                return fixed4(ss, 1);
            }
            ENDCG
        }
    }
}
