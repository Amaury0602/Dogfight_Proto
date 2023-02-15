Shader "Hidden/AfterOutlinePass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Step ("Step", Range(0,1)) = 0
        _Color("Color", Color) = (0,0,0,0)
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
            float _Step;
            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 pixel = tex2D(_MainTex, i.uv);
                float s = 1 - step(_Step, pixel.x);
                
                fixed3 color = _Color * s;

                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}
