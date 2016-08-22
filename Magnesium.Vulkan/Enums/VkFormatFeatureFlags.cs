using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkFormatFeatureFlags : uint
	{
		FormatFeatureSampledImage = 0x1,
		FormatFeatureStorageImage = 0x2,
		FormatFeatureStorageImageAtomic = 0x4,
		FormatFeatureUniformTexelBuffer = 0x8,
		FormatFeatureStorageTexelBuffer = 0x10,
		FormatFeatureStorageTexelBufferAtomic = 0x20,
		FormatFeatureVertexBuffer = 0x40,
		FormatFeatureColorAttachment = 0x80,
		FormatFeatureColorAttachmentBlend = 0x100,
		FormatFeatureDepthStencilAttachment = 0x200,
		FormatFeatureBlitSrc = 0x400,
		FormatFeatureBlitDst = 0x800,
		FormatFeatureSampledImageFilterLinear = 0x1000,
		FormatFeatureSampledImageFilterCubicImg = 0x2000,
	}
}
