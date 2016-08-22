using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkMemoryHeapFlags : uint
	{
		MemoryHeapDeviceLocal = 0x1,
	}
}
