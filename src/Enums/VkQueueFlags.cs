using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkQueueFlags : uint
	{
		QueueGraphics = 0x1,
		QueueCompute = 0x2,
		QueueTransfer = 0x4,
		QueueSparseBinding = 0x8,
	}
}
