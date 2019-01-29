Shader "Custom/PengShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emissive("Emissive Map", 2D) = "black" {}
		_EmissiveCol("Emissive Color", color) = (1,1,1,1)
		_EmissiveMul("Emissive Mult", float) = 1
		_AmbientEmissive("Ambient Emissive", color) = (0,0,0,0)
		_Bump("Normal Map", 2D) = "normal" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0


        sampler2D _MainTex;
		sampler2D _Emissive;
		sampler2D _Bump;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
		half _Metallic, _EmissiveMul;
		fixed4 _Color, _EmissiveCol, _AmbientEmissive;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e = tex2D(_Emissive, IN.uv_MainTex) * _EmissiveCol;
			fixed3 n = UnpackNormal(tex2D(_Bump, IN.uv_MainTex));

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
			o.Normal = n;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
			o.Emission = _AmbientEmissive + (e.rgb * _EmissiveMul);

			clip(c.a - 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
