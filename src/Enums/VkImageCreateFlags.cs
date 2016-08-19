using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkImageCreateFlags : uint
	{
		ImageCreateSparseBinding = 0x1,
		ImageCreateSparseResidency = 0x2,
		ImageCreateSparseAliased = 0x4,
		ImageCreateMutableFormat = 0x8,
		ImageCreateCubeCompatible = 0x10,
	}
}
