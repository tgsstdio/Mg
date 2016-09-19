using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCommandBufferUsageFlags : uint
	{
		CommandBufferUsageOneTimeSubmit = 0x1,
		CommandBufferUsageRenderPassContinue = 0x2,
		CommandBufferUsageSimultaneousUse = 0x4,
	}
}
