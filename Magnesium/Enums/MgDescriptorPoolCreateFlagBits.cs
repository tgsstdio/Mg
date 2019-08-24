using System;

namespace Magnesium
{
    [Flags]
    public enum MgDescriptorPoolCreateFlagBits : UInt32
    {
        /// <summary> 
        /// Descriptor sets may be freed individually
        /// </summary> 
        FREE_DESCRIPTOR_SET_BIT = 0x1,
        UPDATE_AFTER_BIND_BIT_EXT = 0x2,
    }
}

