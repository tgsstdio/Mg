using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCommandPoolCreateFlags : uint
	{
		CommandPoolCreateTransient = 0x1,
		CommandPoolCreateResetCommandBuffer = 0x2,
	}
}
