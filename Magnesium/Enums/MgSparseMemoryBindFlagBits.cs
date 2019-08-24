using System;

namespace Magnesium
{
    [Flags]
    public enum MgSparseMemoryBindFlagBits : UInt32
    {
        /// <summary> 
        /// Operation binds resource metadata to memory
        /// </summary> 
        METADATA_BIT = 0x1,
    }
}
