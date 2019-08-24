using System;

namespace Magnesium
{
    [Flags] 
	public enum MgBufferCreateFlagBits : byte
	{
        /// <summary> 
        /// Buffer should support sparse backing
        /// </summary> 
        SPARSE_BINDING_BIT = 0x1,
        /// <summary> 
        /// Buffer should support sparse backing with partial residency
        /// </summary> 
        SPARSE_RESIDENCY_BIT = 0x2,
        /// <summary> 
        /// Buffer should support constent data access to physical memory ranges mapped into multiple locations of sparse buffers
        /// </summary> 
        SPARSE_ALIASED_BIT = 0x4,
        /// <summary> 
        /// Buffer requires protected memory
        /// </summary> 
        PROTECTED_BIT = 0x8,
    }
}

