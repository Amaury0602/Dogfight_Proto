Shader "Hidden/PPSeeThrough"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RenderTex("RenderTexture", 2D) = "white" {}
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
            sampler2D _RenderTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 renderTexCol = tex2D(_RenderTex, i.uv);
                float playerHere = step(renderTexCol.r, 1);
                //// just invert the colors
                //col.rgb = 1 - col.rgb;

                //col.rgb = playerHere - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
