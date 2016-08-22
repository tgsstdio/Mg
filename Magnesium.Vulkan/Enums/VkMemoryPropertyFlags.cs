using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkMemoryPropertyFlags : uint
	{
		MemoryPropertyDeviceLocal = 0x1,
		MemoryPropertyHostVisible = 0x2,
		MemoryPropertyHostCoherent = 0x4,
		MemoryPropertyHostCached = 0x8,
		MemoryPropertyLazilyAllocated = 0x10,
	}
}
