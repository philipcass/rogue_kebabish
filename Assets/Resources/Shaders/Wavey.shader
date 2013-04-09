//from http://forum.unity3d.com/threads/68402-Making-a-2D-game-for-iPhone-iPad-and-need-better-performance

Shader "Wavey" 
{
 Properties 
 {
    _Color ("Main Color", Color) = (1,1,0,1)
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 }
 
 Category 
 {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     ZWrite Off
     //Alphatest Greater 0
     Blend SrcAlpha OneMinusSrcAlpha 
     Fog { Color(0,0,0,0) }
     Lighting Off
     Cull Off //we can turn backface culling off because we know nothing will be facing backwards

      SubShader   
     {
        Pass {
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
         
             struct v2f {
                 float4 pos : SV_POSITION;
                 float2 uv_MainTex : TEXCOORD0;
             };
         
             float4 _Color;
             float4 _MainTex_ST;
         
             v2f vert(appdata_base v) {
                 v2f o;    
                 float Waviness = 7.5f; 
                 v.vertex.x += sin(_Time.z + v.vertex.y * 0.5) * Waviness;

                 o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                 return o;
             }
         
             sampler2D _MainTex;
         
             float4 frag(v2f IN) : COLOR {
                 half4 c = tex2D (_MainTex, IN.uv_MainTex);
                 return c * _Color;
             }
         ENDCG
     }
         
     } 
 }
}



//Blend SrcAlpha OneMinusSrcAlpha 