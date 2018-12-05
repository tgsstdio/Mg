using System;

namespace Magnesium
{
    [Flags]
    public enum MgQueueFlagBits : UInt32
    {
        /// <summary> 
        /// Queue supports graphics operations
        /// </summary> 
        GRAPHICS_BIT = 0x1,
        /// <summary> 
        /// Queue supports compute operations
        /// </summary> 
        COMPUTE_BIT = 0x2,
        /// <summary> 
        /// Queue supports transfer operations
        /// </summary> 
        TRANSFER_BIT = 0x4,
        /// <summary> 
        /// Queue supports sparse resource memory management operations
        /// </summary> 
        SPARSE_BINDING_BIT = 0x8,
        /// <summary> 
        /// Queues may support protected operations
        /// </summary> 
        PROTECTED_BIT = 0x10,
    }
}
