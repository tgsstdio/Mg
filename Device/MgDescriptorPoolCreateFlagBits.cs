using System;

namespace Magnesium
{
    [Flags] 
	public enum MgDescriptorPoolCreateFlagBits : byte
	{
		// Descriptor sets may be freed individually
		DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT = 1 << 0,
	}
}

