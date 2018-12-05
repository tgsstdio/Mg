using System;

namespace Magnesium
{
    [Flags]
    public enum MgMemoryPropertyFlagBits : UInt32
    {
        /// <summary> 
        /// If otherwise stated, then allocate memory on device
        /// </summary> 
        DEVICE_LOCAL_BIT = 0x1,
        /// <summary> 
        /// Memory is mappable by host
        /// </summary> 
        HOST_VISIBLE_BIT = 0x2,
        /// <summary> 
        /// Memory will have i/o coherency. If not set, application may need to use vkFlushMappedMemoryRanges and vkInvalidateMappedMemoryRanges to flush/invalidate host cache
        /// </summary> 
        HOST_COHERENT_BIT = 0x4,
        /// <summary> 
        /// Memory will be cached by the host
        /// </summary> 
        HOST_CACHED_BIT = 0x8,
        /// <summary> 
        /// Memory may be allocated by the driver when it is required
        /// </summary> 
        LAZILY_ALLOCATED_BIT = 0x10,
        /// <summary> 
        /// Memory is protected
        /// </summary> 
        PROTECTED_BIT = 0x20,
    }
}
