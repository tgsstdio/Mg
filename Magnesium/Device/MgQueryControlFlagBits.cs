using System;

namespace Magnesium
{
    [Flags] 
	public enum MgQueryControlFlagBits : byte
	{
		// Require precise results to be collected by the query
		PRECISE_BIT = 1 << 0,
	}
}

