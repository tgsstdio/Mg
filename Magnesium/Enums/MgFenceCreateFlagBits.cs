using System;

namespace Magnesium
{
    [Flags] 
	public enum MgFenceCreateFlagBits : byte
	{
		SIGNALED_BIT = 1 << 0,
	}
}

