using System;

namespace Magnesium
{
    [Flags] 
	public enum MgSampleCountFlagBits : byte
	{
		// Sample count 1 supported
		COUNT_1_BIT = 1 << 0,
		// Sample count 2 supported
		COUNT_2_BIT = 1 << 1,
		// Sample count 4 supported
		COUNT_4_BIT = 1 << 2,
		// Sample count 8 supported
		COUNT_8_BIT = 1 << 3,
		// Sample count 16 supported
		COUNT_16_BIT = 1 << 4,
		// Sample count 32 supported
		COUNT_32_BIT = 1 << 5,
		// Sample count 64 supported
		COUNT_64_BIT = 1 << 6,
	}
}

