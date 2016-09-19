using System;

namespace Magnesium.Vulkan
{
	internal enum VkBlendFactor : uint
	{
		BlendFactorZero = 0,
		BlendFactorOne = 1,
		BlendFactorSrcColor = 2,
		BlendFactorOneMinusSrcColor = 3,
		BlendFactorDstColor = 4,
		BlendFactorOneMinusDstColor = 5,
		BlendFactorSrcAlpha = 6,
		BlendFactorOneMinusSrcAlpha = 7,
		BlendFactorDstAlpha = 8,
		BlendFactorOneMinusDstAlpha = 9,
		BlendFactorConstantColor = 10,
		BlendFactorOneMinusConstantColor = 11,
		BlendFactorConstantAlpha = 12,
		BlendFactorOneMinusConstantAlpha = 13,
		BlendFactorSrcAlphaSaturate = 14,
		BlendFactorSrc1Color = 15,
		BlendFactorOneMinusSrc1Color = 16,
		BlendFactorSrc1Alpha = 17,
		BlendFactorOneMinusSrc1Alpha = 18,
	}
}
