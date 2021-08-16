// Code originaly from Unity's built-in Sprites-Default.shader
// Modified by ComputerKim, modifications are commented

Shader "TileBlend/TileBlend" // Name changed
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
        

        // Custom properties
        _TransparencyRatio("Transparency Ratio", Range(0, 0.5)) = 0.1
        [KeywordEnum(N, NE, E, SE, S, SW, W, NW)] _Direction("Direction", Int) = 7
        _AtlasWidth("Atlas Width", Int) = 2
        _AtlasHeight("Atlas Height", Int) = 2
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

            Pass
            {
            CGPROGRAM
                #pragma vertex SpriteVert
                #pragma fragment CustomSpriteFrag // Originally SpriteFrag
                #pragma target 2.0
                #pragma multi_compile_instancing
                #pragma multi_compile_local _ PIXELSNAP_ON
                #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
                #include "UnitySprites.cginc"

            // Custom properties
            float _TransparencyRatio;
            int _Direction;
            int _AtlasWidth;
            int _AtlasHeight;

            fixed4 CustomSpriteFrag(v2f IN) : SV_Target // Copy of SpriteFrag from UnitySprites.cginc with modification
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                c.rgb *= c.a;

                float tileWidthRel = 1.0 / _AtlasWidth;
                float tileHeightRel = 1.0 / _AtlasHeight;

                int indexX = IN.texcoord.x / tileWidthRel;
                int indexY = IN.texcoord.y / tileHeightRel;
                float texX = (IN.texcoord.x - (indexX * tileWidthRel)) * _AtlasWidth;
                float texY = (IN.texcoord.y - (indexY * tileHeightRel)) * _AtlasHeight;

                // Apply transparency to edges
                float multiplier = (1 / _TransparencyRatio) * 0.5;

                if (_Direction == 0) { // N
                    if (texY > 1 - _TransparencyRatio) c *= (_TransparencyRatio - (1 - texY)) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 1) { // NE
                    if (texY > 1 - _TransparencyRatio && texX > 1 - _TransparencyRatio) c *= min(_TransparencyRatio - (1 - texY), _TransparencyRatio - (1 - texX)) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 2) { // E
                    if (texX > 1 - _TransparencyRatio) c *= (_TransparencyRatio - (1 - texX)) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 3) { // SE
                    if (texX > 1 - _TransparencyRatio && texY < _TransparencyRatio) c *= min(_TransparencyRatio - (1 - texX), _TransparencyRatio - texY) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 4) { // S
                    if (texY < _TransparencyRatio) c *= (_TransparencyRatio - texY) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 5) { // SW
                    if (texY < _TransparencyRatio && texX < _TransparencyRatio) c *= min(_TransparencyRatio - texY, _TransparencyRatio - texX) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 6) { // W
                    if (texX < _TransparencyRatio) c *= (_TransparencyRatio - texX) * multiplier;
                    else c = 0;
                }
                else if (_Direction == 7) { // NW
                    if (texX < _TransparencyRatio && texY > 1 - _TransparencyRatio) c *= min(_TransparencyRatio - texX, _TransparencyRatio - (1 - texY)) * multiplier;
                    else c = 0;
                }


                return c;
            }
        ENDCG
        }
        }
}