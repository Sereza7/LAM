// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FlatTransparent" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags{ "Queue" = "Geometry+1" "IgnoreProjector" = "False" "RenderType" = "Transparent" "ForceNoShadowCasting" = "True"}

			/////////////////////////////////////////////////////////
			/// First Pass
			/////////////////////////////////////////////////////////

			Pass {
		// Only render alpha channel
		ColorMask A

		Blend SrcAlpha OneMinusSrcAlpha
		

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		fixed4 _Color;

		float4 vert(float4 vertex : POSITION) : SV_POSITION {
			return UnityObjectToClipPos(vertex);
		}

		fixed4 frag() : SV_Target {
			return _Color;
		}

		ENDCG
	}

		/////////////////////////////////////////////////////////
		/// Second Pass
		/////////////////////////////////////////////////////////

		Pass {
			// Now render color channel
			ColorMask RGB
			Blend OneMinusDstAlpha DstAlpha
			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				return _Color;
			}
			ENDCG
		}
	}

		Fallback "Diffuse"
}