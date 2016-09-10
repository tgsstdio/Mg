using System;

namespace Magnesium
{
    [Flags] 
	public enum MgDisplayPlaneAlphaFlagBitsKHR : byte
	{
		OPAQUE_BIT_KHR = 1 << 0,
		GLOBAL_BIT_KHR = 1 << 1,
		PER_PIXEL_BIT_KHR = 1 << 2,
		PER_PIXEL_PREMULTIPLIED_BIT_KHR = 1 << 3,
	}
}

