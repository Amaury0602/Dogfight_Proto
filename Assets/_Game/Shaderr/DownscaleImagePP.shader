Shader "Hidden/DownscaleImagePP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width ("Width", Float) = 400
        _Heiht ("Height", Float) = 200
        _Smoother ("Smooth", Float) = 2
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
            float _Width;
            float _Height;
            float _Smoother;

            float Texel()
            {
                return _MainTex_TexelSize;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 newUVPos;

                newUVPos.x = i.uv.x * _Smoother;
                newUVPos.y = i.uv.y * _Smoother;

                newUVPos.x = round(newUVPos.x);
                newUVPos.y = round(newUVPos.y);

                newUVPos.x /= _Smoother;
                newUVPos.y /= _Smoother;


                float2 size = float2(_MainTex_TexelSize.x * _Width, _MainTex_TexelSize.y * _Height);
                fixed4 col = tex2D(_MainTex, newUVPos);

               

                return col;
            }
            ENDCG
        }
    }
}
