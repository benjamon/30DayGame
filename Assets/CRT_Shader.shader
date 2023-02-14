Shader "Bentendo/CRT_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineHeight("LineHeight", Float) = 0.1
        _ScanSpeed("ScanlinesSpeed", Float) = .2
        _BumpSize("BumpSize", Float) = .2
        _Vignette("Vignette", Float) = .3
        _AbberationSize("AbbertationSize", Float) = .5
        _LineColor("LineColor", Color) = (0.8,0.8,0.8,1.0)
    }
    SubShader
    {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _LineHeight;
            float _ScanSpeed;
            float4 _LineColor;
            float _BumpSize;
            float _Vignette;
            float _AbberationSize;
            fixed4 frag(v2f i) : SV_Target
            {
                //crt bump
                float2 d = i.uv - float2(0.5,0.5);
                float edist = .5 - sqrt(d.x * d.x + d.y * d.y);
                float2 rv = lerp(i.uv, float2(0.5, 0.5), edist * _BumpSize);

                fixed4 col = tex2D(_MainTex, rv);
                
                //chromatic abberation
                fixed4 coll = tex2D(_MainTex, rv - float2(_AbberationSize * _LineHeight, 0.0));
                fixed4 colr = tex2D(_MainTex, rv + float2(_AbberationSize * _LineHeight, 0.0));
                col = fixed4(coll.x, col.y, colr.z, col.w);

                //scanlines
                float scanLine = ceil((rv.y + _Time.w * _LineHeight * _ScanSpeed) % _LineHeight - _LineHeight * 0.5);
                col.rgb *= scanLine + (1.0 - scanLine) * _LineColor;
                col.rgb *= lerp(1.0 - _Vignette, 1.0, .5 + edist);
                return col;
            }
            ENDCG
        }
    }
}
