using System;

namespace Magnesium
{
    [Flags] 
	public enum MgQueryControlFlagBits : byte
	{
        /// <summary> 
        /// Require precise results to be collected by the query
        /// </summary> 
        PRECISE_BIT = 1 << 0,
	}
}

