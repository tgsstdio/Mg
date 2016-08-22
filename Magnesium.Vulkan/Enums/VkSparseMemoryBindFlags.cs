using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkSparseMemoryBindFlags : uint
	{
		SparseMemoryBindMetadata = 0x1,
	}
}
