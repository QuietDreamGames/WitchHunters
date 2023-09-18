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
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }
 
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;

            half _Alpha;

            float4 _HitTint;
            half _HitProgress;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = TransformObjectToHClip(IN.vertex);
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
            ENDHLSL
        }
    }
}
