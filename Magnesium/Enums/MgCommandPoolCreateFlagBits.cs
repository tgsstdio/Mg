using System;

namespace Magnesium
{
    [Flags] 
	public enum MgCommandPoolCreateFlagBits : UInt32
	{
        /// <summary> 
        /// Command buffers have a short lifetime
        /// </summary> 
        TRANSIENT_BIT = 0x1,
        /// <summary> 
        /// Command buffers may release their memory individually
        /// </summary> 
        RESET_COMMAND_BUFFER_BIT = 0x2,
        /// <summary> 
        /// Command buffers allocated from pool are protected command buffers
        /// </summary> 
        PROTECTED_BIT = 0x4,
    }
}

