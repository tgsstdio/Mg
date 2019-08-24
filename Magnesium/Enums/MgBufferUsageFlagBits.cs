using System;

namespace Magnesium
{
    [Flags] 
	public enum MgBufferUsageFlagBits : UInt32
	{
        /// <summary> 
        /// Can be used as a source of transfer operations
        /// </summary> 
        TRANSFER_SRC_BIT = 0x1,
        /// <summary> 
        /// Can be used as a destination of transfer operations
        /// </summary> 
        TRANSFER_DST_BIT = 0x2,
        /// <summary> 
        /// Can be used as TBO
        /// </summary> 
        UNIFORM_TEXEL_BUFFER_BIT = 0x4,
        /// <summary> 
        /// Can be used as IBO
        /// </summary> 
        STORAGE_TEXEL_BUFFER_BIT = 0x8,
        /// <summary> 
        /// Can be used as UBO
        /// </summary> 
        UNIFORM_BUFFER_BIT = 0x10,
        /// <summary> 
        /// Can be used as SSBO
        /// </summary> 
        STORAGE_BUFFER_BIT = 0x20,
        /// <summary> 
        /// Can be used as source of fixed-function index fetch (index buffer)
        /// </summary> 
        INDEX_BUFFER_BIT = 0x40,
        /// <summary> 
        /// Can be used as source of fixed-function vertex fetch (VBO)
        /// </summary> 
        VERTEX_BUFFER_BIT = 0x80,
        /// <summary> 
        /// Can be the source of indirect parameters (e.g. indirect buffer, parameter buffer)
        /// </summary> 
        INDIRECT_BUFFER_BIT = 0x100,
        TRANSFORM_FEEDBACK_BUFFER_BIT_EXT = 0x800,
        TRANSFORM_FEEDBACK_COUNTER_BUFFER_BIT_EXT = 0x1000,
        /// <summary> 
        /// Specifies the buffer can be used as predicate in conditional rendering
        /// </summary> 
        CONDITIONAL_RENDERING_BIT_EXT = 0x200,
        RAY_TRACING_BIT_NV = 0x400,
    }
}

