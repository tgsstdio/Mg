using System;

namespace Magnesium.Vulkan
{
	internal enum VkSubpassContents : uint
	{
		SubpassContentsInline = 0,
		SubpassContentsSecondaryCommandBuffers = 1,
	}
}
