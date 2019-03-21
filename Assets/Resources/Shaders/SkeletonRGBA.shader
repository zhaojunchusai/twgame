// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spine/SkeletonRGBA" {
	Properties {
		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0
		_MainTex ("Texture to blend", 2D) = "black" {}
		_Mask ("Mask", 2D) = "white" {}
	}
	// 2 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Pass {
			
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			
			sampler2D _MainTex;
			sampler2D _Mask;
			float4 _MainTex_ST;
			float4 _Mask_ST;
			float _Cutoff;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord_1 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord_1 : TEXCOORD1;
			};
		
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord_1 = TRANSFORM_TEX(v.texcoord_1, _Mask);
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 c = tex2D(_MainTex,i.texcoord);
				c.a = tex2D(_Mask,i.texcoord_1).r;
				
				clip(c.a - _Cutoff);
				return c;
			}
			
			ENDCG
		}

		Pass {
			Name "Caster"
			Tags { "LightMode"="ShadowCaster" }
			Offset 1, 1
			
			Fog { Mode Off }
			ZWrite On
			ZTest LEqual
			Cull Off
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord_1 : TEXCOORD1;
			};
			
			struct v2f { 
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				float2 texcoord_1 : TEXCOORD2;
			};

			uniform float4 _MainTex_ST;
			uniform float4 _Mask_ST;

			v2f vert (appdata_t v) {
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord_1 = TRANSFORM_TEX(v.texcoord_1, _Mask);
				return o;
			}

			uniform sampler2D _MainTex;
			uniform sampler2D _Mask;
			uniform fixed _Cutoff;

			float4 frag (v2f i) : COLOR {
				fixed4 texcol = tex2D(_MainTex, i.uv);
				texcol.a = tex2D(_Mask, i.texcoord_1).r;
				clip(texcol.a - _Cutoff);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	// 1 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Pass {
			ColorMaterial AmbientAndDiffuse
			SetTexture [_MainTex] {
				Combine texture * primary DOUBLE, texture * primary
			}
		}
	}
}