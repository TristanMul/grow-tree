Shader "RadarShaders/Ghost Smooth clipping (beta)" {
	Properties{
		[Toggle]_UseFalloff("Smooth Falloff", Float) = 0

		_SmoothClipping("Smooth clipping border", Range(0 , 2)) = 0

		_RimFalloff("Rim Falloff", Range(1,10)) = 2
		_RimWidth("Rim Width", Range(0,1)) = 0.1
		_ColorMap("Color (RGB)", 2D) = "white" {}
		_NoiseMap("Noise (RGB)", 2D) = "white" {}
		_RimColor("Rim Color",Color) = (0.0,0.0,0,1)
		_RimSecondaryColor("Secondary Rim Color",Color) = (0.0,0.0,0,1)
		
			_EmissionColor("EmissionColor",Color) = (0.0,0.0,0,0.0)
			_EmissionMap("Emission (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "False" "RenderType" = "Transparent" }
		LOD 200

		ZTest LEqual
		Zwrite Off

			// extra pass that renders to depth buffer only
			Pass{
			ZWrite Off
			ColorMask 0
			}



			CGPROGRAM

			#pragma surface surf BlinnPhong alpha:fade keepalpha
			#pragma target 4.0

			sampler2D _ColorMap, _BumpMap, _NoiseMap, _EmissionMap;
			uniform sampler2D _CameraDepthTexture;

		struct Input {
			float2 uv_ColorMap;
			float2 uv_NoiseMap;
			float2 uv_EmissionMap;
			float3 worldPos;
			float4 screenPos;
		};
		
		fixed4 _RimColor;
		fixed4 _EmissionColor;
		fixed4 _RimSecondaryColor;
		half _RimWidth;
		half _RimFalloff;
		half _NormalPower;
		bool _UseRim;
		bool _UseFalloff;
		uniform half _SmoothClipping;

		void surf(Input IN, inout SurfaceOutput o) {

				half rim = 1 - saturate(abs(dot(normalize(_WorldSpaceCameraPos - IN.worldPos), normalize(o.Normal))));
				rim *= _RimFalloff;
				rim -= -1 + (_RimFalloff / 2);

				rim = clamp(rim, 0.0, 1.0);

				float4 pos = float4(IN.screenPos.xyz, IN.screenPos.w);
				float d = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(pos))));
				float s = abs((d - pos.w));
				float Falloff = lerp(0.0, 1.0, pow(saturate(pow(s, _SmoothClipping / 2)), _SmoothClipping));

				fixed4 g1 = (_RimColor * rim)*_RimColor.a;
				half srim = 1 - (rim*_RimColor.a);
				srim = clamp(srim, 0.0, 1.0);
				fixed4 g2 = (_RimSecondaryColor * srim)*_RimSecondaryColor.a;
				fixed4 c = (tex2D(_ColorMap, IN.uv_ColorMap).rgba * (g1 + g2));
				c.a = clamp(c.a * (Falloff * 2),0.0,1.0);

				o.Albedo = c;
				fixed4 cn = 1 * tex2D(_NoiseMap, IN.uv_NoiseMap);

				


				if (_UseFalloff) {
					
					o.Alpha = ((rim - (1 - (_RimWidth + 0.5) * 2))* c.a) * cn;
					//o.Alpha = lerpResult208 * 2;
				}
				else {
					o.Alpha = (((rim + _RimWidth) > 1 ? 1 : 0)* c.a) * cn;
				}

				o.Emission = _EmissionColor *tex2D(_EmissionMap, IN.uv_EmissionMap);;


		}
		ENDCG
	}

		FallBack "Diffuse"
}