Shader "Unlit/Lava"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Step("Step", Range(0,1)) = 0
        _LightStep("Light Step", Range(0,1)) = 0
        _Smooth("Smooth", Range(0,0.2)) = 0
        _Shine("Shine", Float) = 0
        _RimColor("Rim Color", Color) = (0,0,0,0)
        _SpecColor("Spec Color", Color) = (0,0,0,0)
        _ShadowColor("Shadow Color", Color) = (0,0,0,0)
        _InColor("In Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _LightStep;
            float _Step;
            float _Shine;
            float _Smooth;
            float4 _RimColor;
            float4 _SpecColor;
            float4 _InColor;
            float4 _ShadowColor;

            float hash(float2 st)
            {
                float3 p3 = frac(float3(st.xyx) * .1031);
                p3 += dot(p3, p3.yzx + 19.19);
                return frac((p3.x + p3.y) * p3.z);
            }

            float noise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);

                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) +
                    (c - a) * u.y * (1.0 - u.x) +
                    (d - b) * u.x * u.y;
            }

            v2f vert (appdata v)
            {
                v2f o;

                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);


                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                
                //rim
                float rimDot = dot(i.viewDir, i.normal);
                float rim = smoothstep(rimDot - _Smooth, rimDot + _Smooth, _Step);

                //lambert light
                float lambert = dot(i.normal, _WorldSpaceLightPos0);
                float lightStep = step(lambert, _LightStep);

                float3 ramp = floor(lambert * 3) / 3; 

                //fixed4 lightCol = lerp(_InColor, _ShadowColor, ramp);
                fixed3 lightCol = ramp;


                
                //fixed3 col = lerp(lightCol, _RimColor, rim);

                //blinn phong
                float3 halfWay = normalize(i.viewDir + _WorldSpaceLightPos0);
                float spec = pow(max(dot(i.normal, halfWay), 0.0), _Shine);
                fixed4 specCol = _SpecColor * spec;



                return fixed4(ramp, 1);
            }
            ENDCG
        }
    }
}
