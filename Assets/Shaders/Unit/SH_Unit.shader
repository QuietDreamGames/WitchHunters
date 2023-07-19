Shader "Unlit/SH_Unit"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        
        _Alpha("Alpha", float) = 1
        
        _HitTint("Hit Tint", Color) = (1,1,1,1)
        _HitProgress("Hit Progress", float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;

            half _Alpha;

            fixed4 _HitTint;
            half _HitProgress;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                
                return OUT;
            }

            float4 GetTint(float4 source, float4 tint, half progress)
            {
                source.rgb = lerp(source.rgb, tint.rgb, progress);
                return source;
            }

            float4 frag(v2f IN) : SV_Target
            {
                float4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                color.a *= _Alpha;
                
                const half alpha = color.a;

                color = GetTint(color, _HitTint, _HitProgress);

                return color * alpha;
            }
            ENDCG
        }
    }
}
