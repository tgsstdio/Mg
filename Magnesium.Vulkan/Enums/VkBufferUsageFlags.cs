using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkBufferUsageFlags : uint
	{
		BufferUsageTransferSrc = 0x1,
		BufferUsageTransferDst = 0x2,
		BufferUsageUniformTexelBuffer = 0x4,
		BufferUsageStorageTexelBuffer = 0x8,
		BufferUsageUniformBuffer = 0x10,
		BufferUsageStorageBuffer = 0x20,
		BufferUsageIndexBuffer = 0x40,
		BufferUsageVertexBuffer = 0x80,
		BufferUsageIndirectBuffer = 0x100,
	}
}
