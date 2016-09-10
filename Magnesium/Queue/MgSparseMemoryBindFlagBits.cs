using System;

namespace Magnesium
{
    [Flags] 
	public enum MgSparseMemoryBindFlagBits : byte
	{
		// Operation binds resource metadata to memory
		METADATA_BIT = 1 << 0,
	}
}

