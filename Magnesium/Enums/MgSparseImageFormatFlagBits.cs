using System;

namespace Magnesium
{
    [Flags]
    public enum MgSparseImageFormatFlagBits : UInt32
    {
        /// <summary> 
        /// Image uses a single mip tail region for all array layers
        /// </summary> 
        SINGLE_MIPTAIL_BIT = 0x1,
        /// <summary> 
        /// Image requires mip level dimensions to be an integer multiple of the sparse image block dimensions for non-tail mip levels.
        /// </summary> 
        ALIGNED_MIP_SIZE_BIT = 0x2,
        /// <summary> 
        /// Image uses a non-standard sparse image block dimensions
        /// </summary> 
        NONSTANDARD_BLOCK_SIZE_BIT = 0x4,
    }
}
