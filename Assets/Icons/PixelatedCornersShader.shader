Shader "Custom/PixelatedCornersShader" {
    Properties {
        _MainTex("Base (RGB)", 2D) = "white" { }
        _PixelSize("Pixel Size", Range(1, 50)) = 10
        _CornerSize("Corner Size", Range(1, 50)) = 10
    }

    SubShader {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            uniform float _PixelSize;
            uniform float _CornerSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Apply pixelation
                o.pos.xy = floor(o.pos.xy / _PixelSize) * _PixelSize;

                // Apply pixel corners
                float2 cornerOffset = fmod(o.pos.xy, _CornerSize) - _CornerSize * 0.5;
                float cornerEffect = saturate(1 - abs(cornerOffset.x) * abs(cornerOffset.y) * 4);
                o.pos.xy += cornerOffset * cornerEffect;

                return o;
            }
            ENDCG

            SetTexture[_MainTex] {
                combine primary
            }
        }
    }
}
