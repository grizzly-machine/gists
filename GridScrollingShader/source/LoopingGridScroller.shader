Shader "Grizzly Machine/Unlit Looping Grid Scroller"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollPosition ("Scroll Position", Float) = 0
        _RowCount ("Row Count", Float) = 1
        _ColumnCount ("Column Count", Float) = 1
        [KeywordEnum(Vertical,Horizontal)]_Direction("Direction", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _DIRECTION_VERTICAL _DIRECTION_HORIZONTAL

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollPosition;
            float _RowCount;
            float _ColumnCount;

            #ifdef _DIRECTION_VERTICAL
                #define CELL_WIDTH _MainTex_ST.x
                #define CELL_HEIGHT _MainTex_ST.y
                #define SCROLL_AXIS y
                #define NON_SCROLL_AXIS x
                #define SCROLL_AXIS_HEIGHT _RowCount * CELL_HEIGHT
                #define SCROLL_DIR_COUNT _ColumnCount
            #endif
            #ifdef _DIRECTION_HORIZONTAL
                #define CELL_WIDTH _MainTex_ST.y
                #define CELL_HEIGHT _MainTex_ST.x
                #define SCROLL_AXIS x
                #define NON_SCROLL_AXIS y
                #define SCROLL_AXIS_HEIGHT _ColumnCount * CELL_HEIGHT
                #define SCROLL_DIR_COUNT _RowCount
            #endif
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float numCells = _RowCount * _ColumnCount;
                v.uv.SCROLL_AXIS += (numCells - ((_ScrollPosition * -1) % numCells));
                o.uv = v.uv * _MainTex_ST.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float scrollHeight = SCROLL_AXIS_HEIGHT;
				i.uv.NON_SCROLL_AXIS += CELL_WIDTH * (trunc(i.uv.SCROLL_AXIS / scrollHeight) % SCROLL_DIR_COUNT);
                i.uv.SCROLL_AXIS %= scrollHeight;
                i.uv += _MainTex_ST.zw;
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
