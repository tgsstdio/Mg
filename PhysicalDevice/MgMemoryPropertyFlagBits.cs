using System;

namespace Magnesium
{
    [Flags] 
	public enum MgMemoryPropertyFlagBits : byte
	{
		/// <summary>
		/// If otherwise stated, then allocate memory on device
		/// </summary>
		DEVICE_LOCAL_BIT = 1 << 0,

		/// <summary>
		/// Memory is mappable by host
		/// </summary>
		HOST_VISIBLE_BIT = 1 << 1,

		/// <summary>
		/// Memory will have i/o coherency. If not set, application may need to use vkFlushMappedMemoryRanges and vkInvalidateMappedMemoryRanges to flush/invalidate host cache
		/// </summary>
		HOST_COHERENT_BIT = 1 << 2,

		/// <summary>
		/// Memory will be cached by the host
		/// </summary>
		HOST_CACHED_BIT = 1 << 3,

		/// <summary>
		/// Memory may be allocated by the driver when it is required
		/// </summary>
		LAZILY_ALLOCATED_BIT = 1 << 4,
	};
}

