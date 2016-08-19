using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkCullModeFlags : uint
	{
		CullModeNone = 0,
		CullModeFront = 0x1,
		CullModeBack = 0x2,
		CullModeFrontAndBack = 0x00000003,
	}
}
