using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkSparseImageFormatFlags : uint
	{
		SparseImageFormatSingleMiptail = 0x1,
		SparseImageFormatAlignedMipSize = 0x2,
		SparseImageFormatNonstandardBlockSize = 0x4,
	}
}
