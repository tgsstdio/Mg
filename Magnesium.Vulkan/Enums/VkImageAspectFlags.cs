using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkImageAspectFlags : uint
	{
		ImageAspectColor = 0x1,
		ImageAspectDepth = 0x2,
		ImageAspectStencil = 0x4,
		ImageAspectMetadata = 0x8,
	}
}
