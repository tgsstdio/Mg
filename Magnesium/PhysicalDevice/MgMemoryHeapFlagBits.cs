using System;

namespace Magnesium
{
    [Flags] 
	public enum MgMemoryHeapFlagBits : byte
	{
		// If set, heap represents device memory
		DEVICE_LOCAL_BIT = 1 << 0,
	};
}

