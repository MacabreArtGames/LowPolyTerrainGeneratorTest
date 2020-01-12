// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TerrainShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}

	SubShader
	{

		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			float4 _Color;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float3 vertex : TEXCOORD1;
				float4 color : COLOR;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float light : TEXCOORD1;
				float4 color : COLOR;
			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				g2f o;

				// Compute the normal
				float3 vecA = IN[1].vertex - IN[0].vertex;
				float3 vecB = IN[2].vertex - IN[0].vertex;
				float3 normal = cross(vecA, vecB);
				normal = normalize(mul(normal, (float3x3) unity_WorldToObject));

				// Compute diffuse light
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				o.light = max(0., dot(normal, lightDir));

				for (int i = 0; i < 3; i++)
				{
					o.pos = IN[i].pos;
					o.color = IN[i].color;
					triStream.Append(o);
				}
			}

			half4 frag(g2f i) : COLOR
			{
				float4 col = i.light * _Color * i.color;

				return col;
			}

			ENDCG
		}
	}
		Fallback "Diffuse"
}
