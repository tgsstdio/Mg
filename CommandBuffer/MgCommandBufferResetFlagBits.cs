using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandBufferResetFlagBits : byte
	{
		// Release resources owned by the buffer
		RELEASE_RESOURCES_BIT = 1 << 0,
	}
}

