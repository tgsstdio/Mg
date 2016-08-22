using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkStencilFaceFlags : uint
	{
		StencilFaceFront = 0x1,
		StencilFaceBack = 0x2,
		StencilFrontAndBack = 0x00000003,
	}
}
