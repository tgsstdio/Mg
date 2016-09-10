using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandPoolResetFlagBits : byte
	{
		// Release resources owned by the pool
		RELEASE_RESOURCES_BIT = 1 << 0,
	}
}

