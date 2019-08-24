using System;

namespace Magnesium
{
    [Flags]
    public enum MgMemoryHeapFlagBits : UInt32
    {
        /// <summary> 
        /// If set, heap represents device memory
        /// </summary> 
        DEVICE_LOCAL_BIT = 0x1,
        /// <summary> 
        /// If set, heap allocations allocate multiple instances by default
        /// </summary> 
        MULTI_INSTANCE_BIT = 0x2,
        MULTI_INSTANCE_BIT_KHR = MULTI_INSTANCE_BIT,
    }
}

