using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandBufferResetFlagBits : byte
	{
        /// <summary> 
        /// Release resources owned by the buffer
        /// </summary> 
        RELEASE_RESOURCES_BIT = 1 << 0,
	}
}

