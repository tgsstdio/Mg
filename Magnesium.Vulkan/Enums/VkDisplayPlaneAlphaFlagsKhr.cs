using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkDisplayPlaneAlphaFlagsKhr : uint
	{
		DisplayPlaneAlphaOpaque = 0x1,
		DisplayPlaneAlphaGlobal = 0x2,
		DisplayPlaneAlphaPerPixel = 0x4,
		DisplayPlaneAlphaPerPixelPremultiplied = 0x8,
	}
}
