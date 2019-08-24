using System;

namespace Magnesium
{
	[Flags]
	public enum MgDebugUtilsMessageSeverityFlagBitsEXT : UInt32
	{
		VERBOSE_BIT_EXT = 0x1,
		INFO_BIT_EXT = 0x10,
		WARNING_BIT_EXT = 0x100,
		ERROR_BIT_EXT = 0x1000,
	}
}
