using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkDescriptorPoolCreateFlags : uint
	{
		DescriptorPoolCreateFreeDescriptorSet = 0x1,
	}
}
