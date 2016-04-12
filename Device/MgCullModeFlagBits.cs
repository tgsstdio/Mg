using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCullModeFlagBits : byte
	{
		NONE = 0,
		FRONT_BIT = 1 << 0,
		BACK_BIT = 1 << 1,
		FRONT_AND_BACK = 0x3,
	}
}

