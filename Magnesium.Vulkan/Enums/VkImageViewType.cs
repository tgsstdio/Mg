using System;

namespace Magnesium.Vulkan
{
	internal enum VkImageViewType : uint
	{
		ImageViewType1D = 0,
		ImageViewType2D = 1,
		ImageViewType3D = 2,
		ImageViewTypeCube = 3,
		ImageViewType1DArray = 4,
		ImageViewType2DArray = 5,
		ImageViewTypeCubeArray = 6,
	}
}
