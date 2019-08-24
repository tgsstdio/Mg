using System;

namespace Magnesium
{
	[Flags]
	public enum MgObjectEntryUsageFlagBitsNVX : UInt32
	{
		GRAPHICS_BIT_NVX = 0x1,
		COMPUTE_BIT_NVX = 0x2,
	}
}
