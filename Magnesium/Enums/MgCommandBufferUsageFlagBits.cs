using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandBufferUsageFlagBits : byte
	{
        ONE_TIME_SUBMIT_BIT = 0x1,
        RENDER_PASS_CONTINUE_BIT = 0x2,
        /// <summary> 
        /// Command buffer may be submitted/executed more than once simultaneously
        /// </summary> 
        SIMULTANEOUS_USE_BIT = 0x4,
    }
}

