#include "UnityCG.cginc"
#include "UnityStandardUtils.cginc"

// REPLACE_ORIGINAL + REPLACE_CUSTOM
float4    _ReplaceColor;
sampler2D _ReplaceTexture;
float2    _ReplaceTextureSize;

// BLUR + FLOW
float _Kernel;

void ContributeMaskedSample(float2 coord, inout float4 totalColor, inout float totalWeight)
{
	float weight = dot(SampleMip0(_LocalMaskTexture, coord), _LocalMaskChannel);

	totalColor  += weight * SampleMip0(_Buffer, coord);
	totalWeight += weight;
}

float4 DoBlend(float4 current, float4 color, float strength, float2 coord, float rot, float4 channels)
{
	float4 old = current;
	color.a *= strength;
	float str = 1.0f - color.a;
	float div = color.a + current.a * str;

	current.rgb = (color.rgb * color.a + current.rgb * current.a * str) / div;
	current.a   = div;
	return old + (current - old) * channels;
}

float4 Blend(float4 color, float strength, float2 coord, float rot, float4 channels)
{
	coord = SnapToPixel(coord, _BufferSize);
	float4 current = SampleMip0(_Buffer, coord);

	return DoBlend(current, color, strength, coord, rot, channels);
}

float4 Blend(float4 color, float strength, float2 coord, float4 channels)
{
	return Blend(color, strength, coord, 0.0f, channels);
}

float4 BlendMinimum(float4 color, float mask, float strength, float2 coord, float4 minimum, float4 channels)
{
	coord = SnapToPixel(coord, _BufferSize);
	float4 current = SampleMip0(_Buffer, coord);
	float4 result  = DoBlend(current, color, 1.0f, coord, 0.0f, channels);
	float4 change  = (result - current) * mask;
	float4 maximum = abs(change);

	return current + sign(change) * clamp(maximum * strength, 1.0f / 255.0f, maximum);
}