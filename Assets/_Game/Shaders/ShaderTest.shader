Shader "Unlit/ShaderTest"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ColorA("Color A", Color) = (1,0,0,0)
        _ColorB("Color B", Color) = (1,0,0,0)
        _Radius("Radius", Float) = 0.1
        _Duration("Duration", Float) = 6.0
        _PosX("Pos X", Float) = 0
        _PosY("Pos Y", Float) = 0
        _Step("Step", Range(-0.5, 0.5)) = 0
        _LineWidth("Line Width", Float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma enable_d3d11_debug_symbols
            #pragma fragment frag

            #include "UnityCG.cginc"


            fixed4 _ColorA;
            fixed4 _ColorB;
            float _Radius;
            float _PosX;
            float _Step;
            float _PosY;
            float _LineWidth;
            float _Duration;
            sampler2D _MainTex;

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float4 position: TEXCOORD1;
                float3 normal : TEXCOORD3;
                float2 uv: TEXCOORD0;
                float4 screenPos : TEXCOORD2;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                //float delta = (_SinTime.w + 1.0) / 2;
                o.normal = v.normal;
                o.position = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.position.xy;
                float len = length(pos);
                float l = i.uv + pos  / len * cos(len * 10 + _Time.y * 4.0);

                float3 lightPos = _WorldSpaceLightPos0.xyz;

                float lit = max(0.15, dot(i.normal, lightPos));

                /*float len = length(pos);
                float2 ripple = i.uv + pos / len * 0.03 * cos(len * 12.0 - _Time.y * 4.0);
                float theta = fmod(_Time.y, _Duration) * (UNITY_TWO_PI / _Duration);
                float delta = (sin(theta) + 1.0) / 2;
                float2 uv = lerp(ripple, i.uv, delta);
                fixed3 col = tex2D(_MainTex, uv).rgb;*/

                fixed3 col = fixed3(1, 1, 0) * lit;
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
