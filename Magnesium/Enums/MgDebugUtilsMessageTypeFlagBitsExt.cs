using System;

namespace Magnesium
{
	[Flags]
	public enum MgDebugUtilsMessageTypeFlagBitsExt : UInt32
	{
		GENERAL_BIT_EXT = 0x1,
		VALIDATION_BIT_EXT = 0x2,
		PERFORMANCE_BIT_EXT = 0x4,
	}
}
