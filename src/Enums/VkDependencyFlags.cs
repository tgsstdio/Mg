using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkDependencyFlags : uint
	{
		DependencyByRegion = 0x1,
	}
}
