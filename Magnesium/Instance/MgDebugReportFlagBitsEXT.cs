using System;

namespace Magnesium
{
    [Flags] 
	public enum MgDebugReportFlagBitsEXT : byte
	{
		INFORMATION_BIT_EXT = 1 << 0,
		WARNING_BIT_EXT = 1 << 1,
		PERFORMANCE_WARNING_BIT_EXT = 1 << 2,
		ERROR_BIT_EXT = 1 << 3,
		DEBUG_BIT_EXT = 1 << 4,
	}
}

