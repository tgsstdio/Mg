using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCommandBufferResetFlags : uint
	{
		CommandBufferResetReleaseResources = 0x1,
	}
}
