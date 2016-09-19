using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkBufferCreateFlags : uint
	{
		BufferCreateSparseBinding = 0x1,
		BufferCreateSparseResidency = 0x2,
		BufferCreateSparseAliased = 0x4,
	}
}
