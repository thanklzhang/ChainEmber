Shader "Custom/CircularNineSlice"  
{  
    Properties  
    {  
        _MainTex("Texture", 2D) = "white" {}  
        _EdgeWidth("Edge Width", Float) = 0.1  
    }  
    SubShader  
    {  
        Tags { "RenderType"="Opaque" }  
        LOD 100  
        Blend SrcAlpha OneMinusSrcAlpha
  
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
  
            sampler2D _MainTex;  
            float4 _MainTex_ST;  
            float _EdgeWidth;  
  
            v2f vert(appdata v)  
            {  
                v2f o;  
                o.vertex = UnityObjectToClipPos(v.vertex);  
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  
                return o;  
            }  
  
            fixed4 frag(v2f i) : SV_Target  
            {  
                float2 uv = i.uv;  
                float edgeWidth = _EdgeWidth;  
                float2 center = float2(0.5, 0.5);  
                float radius = 0.5;  
                float distFromCenter = distance(uv, center);  
  
                // Calculate a normalized UV based on distance from the edge  
                float2 normalizedUV;  
                if (distFromCenter < edgeWidth)  
                {  
                    // Inside the edge, do not stretch  
                    normalizedUV = uv;  
                }  
                else if (distFromCenter > radius - edgeWidth)  
                {  
                    // Outside the main radius, but inside the outer edge, stretch towards the edge  
                    float stretchFactor = (distFromCenter - (radius - edgeWidth)) / edgeWidth;  
                    normalizedUV = center + (uv - center) * (1 - stretchFactor);  
                }  
                else  
                {  
                    // Inside the stretchable area, stretch  
                    normalizedUV = uv;  
                    // Optionally, you could add more complex stretching logic here  
                }  
  
                // Sample the texture  
                fixed4 col = tex2D(_MainTex, normalizedUV);  
                return col;  
            }  
            ENDCG  
        }  
    }  
}