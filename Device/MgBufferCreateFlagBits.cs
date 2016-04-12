using System;

namespace Magnesium
{
    [Flags] 
	public enum MgBufferCreateFlagBits : byte
	{
		// Buffer should support sparse backing
		BINDING_BIT = 1 << 0,
		// Buffer should support sparse backing with partial residency
		RESIDENCY_BIT = 1 << 1,
		// Buffer should support constent data access to physical memory blocks mapped into multiple locations of sparse buffers
		ALIASED_BIT = 1 << 2,
	}
}

