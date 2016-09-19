using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkImageUsageFlags : uint
	{
		ImageUsageTransferSrc = 0x1,
		ImageUsageTransferDst = 0x2,
		ImageUsageSampled = 0x4,
		ImageUsageStorage = 0x8,
		ImageUsageColorAttachment = 0x10,
		ImageUsageDepthStencilAttachment = 0x20,
		ImageUsageTransientAttachment = 0x40,
		ImageUsageInputAttachment = 0x80,
	}
}
