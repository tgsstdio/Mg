using System;

namespace Magnesium
{
    [Flags] 
	public enum MgMemoryPropertyFlagBits : byte
	{
		// If otherwise stated, then allocate memory on device
		DEVICE_LOCAL_BIT = 1 << 0,
		// Memory is mappable by host
		HOST_VISIBLE_BIT = 1 << 1,
		// Memory will have i/o coherency. If not set, application may need to use vkFlushMappedMemoryRanges and vkInvalidateMappedMemoryRanges to flush/invalidate host cache
		HOST_COHERENT_BIT = 1 << 2,
		// Memory will be cached by the host
		HOST_CACHED_BIT = 1 << 3,
		// Memory may be allocated by the driver when it is required
		LAZILY_ALLOCATED_BIT = 1 << 4,
	};
}

