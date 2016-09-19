using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCommandPoolResetFlags : uint
	{
		CommandPoolResetReleaseResources = 0x1,
	}
}
