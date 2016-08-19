using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkSurfaceTransformFlagsKhr : uint
	{
		SurfaceTransformIdentity = 0x1,
		SurfaceTransformRotate90 = 0x2,
		SurfaceTransformRotate180 = 0x4,
		SurfaceTransformRotate270 = 0x8,
		SurfaceTransformHorizontalMirror = 0x10,
		SurfaceTransformHorizontalMirrorRotate90 = 0x20,
		SurfaceTransformHorizontalMirrorRotate180 = 0x40,
		SurfaceTransformHorizontalMirrorRotate270 = 0x80,
		SurfaceTransformInherit = 0x100,
	}
}
