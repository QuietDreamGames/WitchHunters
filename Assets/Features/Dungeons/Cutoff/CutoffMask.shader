Shader "Unlit/CutoffMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0,1)) = 1
        
        _StencilRef ("Stencil Ref", Int) = 0
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
			"CanUseSpriteAtlas"="True"
        }
        
        Stencil
        {
            Ref [_StencilRef]
            Comp gequal
            Pass replace
        }
        
        Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            UNITY_INSTANCING_BUFFER_START(Props)

            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(half, _Alpha)
            
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_INSTANCE_ID(v, o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                fixed4 Color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                half Alpha = UNITY_ACCESS_INSTANCED_PROP(Props, _Alpha);
                
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 newCol = fixed4(0, 0, 0, 0);
                newCol.a = 1 - col.r;
                newCol.a = clamp(newCol.a + Alpha, 0, 1);
                col *= newCol;
                return newCol;
            }
            ENDCG
        }
    }
}
