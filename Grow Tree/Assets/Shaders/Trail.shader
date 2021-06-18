Shader "Unlit/Trail"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
    }
    Category
    {
        Lighting On
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" "IgnoreProjector" = "True" "Queue" = "Overlay"}
        LOD 100
        Cull Back

        ZTest Always
		Zwrite On

        SubShader{
            Material {
                Emission [_Color]
            }
            Pass
            {
                SetTexture [_MainTex] {
                    Combine Texture * Primary, Texture * Primary
                }
            }
        }

    }
}
