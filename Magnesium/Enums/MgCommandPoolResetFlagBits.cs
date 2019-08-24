using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandPoolResetFlagBits : byte
	{
        /// <summary> 
        /// Release resources owned by the pool
        /// </summary> 
        RELEASE_RESOURCES_BIT = 1 << 0,
	}
}

