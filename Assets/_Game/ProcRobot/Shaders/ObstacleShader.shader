Shader "Unlit/ObstacleShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        ZWrite Off
        ColorMask 0

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
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 normal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            float4 _BaseColor;

            fixed4 frag(v2f i) : SV_Target
            {
                return _BaseColor;
            }
            ENDCG
        }
    }
}
