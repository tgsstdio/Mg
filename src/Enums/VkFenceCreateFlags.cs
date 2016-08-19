using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkFenceCreateFlags : uint
	{
		FenceCreateSignaled = 0x1,
	}
}
