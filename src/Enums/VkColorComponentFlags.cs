using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkColorComponentFlags : uint
	{
		ColorComponentR = 0x1,
		ColorComponentG = 0x2,
		ColorComponentB = 0x4,
		ColorComponentA = 0x8,
	}
}
