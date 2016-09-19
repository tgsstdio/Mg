using System;

namespace Magnesium
{
    [Flags] 
	public enum MgSurfaceTransformFlagBitsKHR : ushort
	{
		IDENTITY_BIT_KHR = 1 << 0,
		ROTATE_90_BIT_KHR = 1 << 1,
		ROTATE_180_BIT_KHR = 1 << 2,
		ROTATE_270_BIT_KHR = 1 << 3,
		HORIZONTAL_MIRROR_BIT_KHR = 1 << 4,
		HORIZONTAL_MIRROR_ROTATE_90_BIT_KHR = 1 << 5,
		HORIZONTAL_MIRROR_ROTATE_180_BIT_KHR = 1 << 6,
		HORIZONTAL_MIRROR_ROTATE_270_BIT_KHR = 1 << 7,
		INHERIT_BIT_KHR = 1 << 8,
	};
}

