using System;

namespace Magnesium
{
    [Flags] 
	public enum MgSparseImageFormatFlagBits : byte
	{
		// Image uses a single miptail region for all array layers
		SINGLE_MIPTAIL_BIT = 1 << 0,
		// Image requires mip levels to be an exact multiple of the sparse image block size for non-miptail levels.
		ALIGNED_MIP_SIZE_BIT = 1 << 1,
		// Image uses a non-standard sparse block size
		NONSTANDARD_BLOCK_SIZE_BIT = 1 << 2,
	}
}

