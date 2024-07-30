Shader "Sphere"
{
	Properties
	{
		_ReplaceTexture("Replace Texture", 2D) = "white" {}
		_TileTexture("Tile Texture", 2D) = "white" {}
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_LocalMaskTexture("Local Mask Texture", 2D) = "white" {}
		_DepthTexture("Depth Texture", 2D) = "white" {}
	}

	SubShader
	{
		Blend Off
		Cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 0 // ALPHA_BLEND
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 1 // ALPHA_BLEND_INVERSE
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 2 // PREMULTIPLIED
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 3 // ADDITIVE
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 4 // ADDITIVE_SOFT
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 5 // SUBTRACTIVE
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 6 // SUBTRACTIVE_SOFT
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 7 // REPLACE
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 8 // REPLACE_ORIGINAL
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 9 // REPLACE_CUSTOM
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 10 // MULTIPLY_INVERSE_RGB
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 11 // BLUR
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 12 // NORMAL_BLEND
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 13 // NORMAL_REPLACE
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD LINE_CLIP QUAD_CLIP
			#define BLEND_MODE_INDEX 14 // FLOW
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD
			#define BLEND_MODE_INDEX 15 // NORMAL_REPLACE_ORIGINAL
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD
			#define BLEND_MODE_INDEX 16 // NORMAL_REPLACE_CUSTOM
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD
			#define BLEND_MODE_INDEX 17 // MIN
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ LINE QUAD
			#define BLEND_MODE_INDEX 18 // MAX
			#include "Assets/Assets/Shaders/Sphere.cginc"
			ENDCG
		}
	} // SubShader
} // Shader