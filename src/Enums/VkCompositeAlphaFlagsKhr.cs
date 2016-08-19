using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCompositeAlphaFlagsKhr : uint
	{
		CompositeAlphaOpaque = 0x1,
		CompositeAlphaPreMultiplied = 0x2,
		CompositeAlphaPostMultiplied = 0x4,
		CompositeAlphaInherit = 0x8,
	}
}
