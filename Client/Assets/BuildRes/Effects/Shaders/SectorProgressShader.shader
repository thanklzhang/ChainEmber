Shader "Custom/SectorProgressShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Progress ("Progress", Range(0, 1)) = 0.5  // 控制从内到外的进度
        _Angle ("Angle", Range(0, 1)) = 0.5   // 范围0-1，表示0-180度扩散
        _Radius ("Radius", Range(0, 1)) = 0.5
        _InnerRadius ("Inner Radius", Range(0, 1)) = 0.0
        _Smoothness ("Edge Smoothness", Range(0, 0.1)) = 0.01
    }
    
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Progress;
            float _Angle;
            float _Radius;
            float _InnerRadius;
            float _Smoothness;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                // 将UV坐标转换为中心点位-0.5到0.5
                float2 uv = i.uv - 0.5;
                
                // 直接计算与-y轴(下方)的角度
                // 计算点到-y轴的夹角（范围0-PI/2）
                float xDist = abs(uv.x);
                float yDist = -uv.y;  // 注意这里取负值，使0度指向下方
                float angleFromDown = atan2(xDist, yDist);  // 这将给出与下方向的夹角
                
                // 如果-y为负(即原来的y为正)，意味着点在上半部分，直接设置角度为最大
                if (yDist < 0) {
                    angleFromDown = UNITY_PI;
                }
                
                // 使用_Angle参数(0-1)来控制扇形从下方向两侧扩散的最大角度差(0-90度)
                float maxAngleDiff = _Angle * UNITY_PI / 2.0;
                
                // 判断当前点是否在扇形角度范围内
                float sectorMask = step(angleFromDown, maxAngleDiff);
                
                // 计算距离中心点的距离
                float dist = length(uv);
                
                // 计算当前进度对应的半径值
                float currentRadius = lerp(_InnerRadius, _Radius, _Progress);
                
                // 创建基于进度的半径遮罩
                float radiusEdge = currentRadius + _Smoothness;
                float progressRadiusMask = 1.0 - smoothstep(currentRadius, radiusEdge, dist);
                
                // 创建内圆遮罩（一直不变）
                float innerEdge = _InnerRadius - _Smoothness;
                float innerMask = smoothstep(innerEdge, _InnerRadius, dist);
                
                // 组合遮罩：内圆遮罩 + 进度半径遮罩
                float radiusMask = innerMask * progressRadiusMask;
                
                // 应用扇形角度遮罩和半径遮罩
                float finalMask = sectorMask * radiusMask;
                
                // 采样纹理
                fixed4 col = tex2D(_MainTex, i.uv) * _Color * i.color;
                
                // 应用最终遮罩
                col.a *= finalMask;
                
                return col;
            }
            ENDCG
        }
    }
} 