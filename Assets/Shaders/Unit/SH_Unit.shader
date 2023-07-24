Shader "Unlit/SH_Unit"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _ColorProgress("Color Progress", float) = 0
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

            fixed4 _Color;
            fixed _ColorProgress;


            v2f vert(appdata_t IN)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                
                return OUT;
            }

            float4 frag(v2f IN) : SV_Target
            {
                float4 spriteColor = tex2D(_MainTex, IN.texcoord);
                spriteColor.rgb *= spriteColor.a * IN.color.a;
                spriteColor *= IN.color;

                return lerp(spriteColor, _Color, _ColorProgress);
            }
            ENDCG
        }
    }
}
