using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandBufferUsageFlagBits : byte
	{
		ONE_TIME_SUBMIT_BIT = 1 << 0,
		RENDER_PASS_CONTINUE_BIT = 1 << 1,
		// Command buffer may be submitted/executed more than once simultaneously
		SIMULTANEOUS_USE_BIT = 1 << 2,
	}
}

