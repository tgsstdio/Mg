using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCompositeAlphaFlagBitsKHR : UInt32
	{
        OPAQUE_BIT_KHR = 0x1,
        PRE_MULTIPLIED_BIT_KHR = 0x2,
        POST_MULTIPLIED_BIT_KHR = 0x4,
        INHERIT_BIT_KHR = 0x8,
    }
}

