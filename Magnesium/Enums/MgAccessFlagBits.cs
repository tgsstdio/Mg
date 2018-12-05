using System;

namespace Magnesium
{
    [Flags]
    public enum MgAccessFlagBits : UInt32
    {
        /// <summary> 
        /// Controls coherency of indirect command reads
        /// </summary> 
        INDIRECT_COMMAND_READ_BIT = 0x1,
        /// <summary> 
        /// Controls coherency of index reads
        /// </summary> 
        INDEX_READ_BIT = 0x2,
        /// <summary> 
        /// Controls coherency of vertex attribute reads
        /// </summary> 
        VERTEX_ATTRIBUTE_READ_BIT = 0x4,
        /// <summary> 
        /// Controls coherency of uniform buffer reads
        /// </summary> 
        UNIFORM_READ_BIT = 0x8,
        /// <summary> 
        /// Controls coherency of input attachment reads
        /// </summary> 
        INPUT_ATTACHMENT_READ_BIT = 0x10,
        /// <summary> 
        /// Controls coherency of shader reads
        /// </summary> 
        SHADER_READ_BIT = 0x20,
        /// <summary> 
        /// Controls coherency of shader writes
        /// </summary> 
        SHADER_WRITE_BIT = 0x40,
        /// <summary> 
        /// Controls coherency of color attachment reads
        /// </summary> 
        COLOR_ATTACHMENT_READ_BIT = 0x80,
        /// <summary> 
        /// Controls coherency of color attachment writes
        /// </summary> 
        COLOR_ATTACHMENT_WRITE_BIT = 0x100,
        /// <summary> 
        /// Controls coherency of depth/stencil attachment reads
        /// </summary> 
        DEPTH_STENCIL_ATTACHMENT_READ_BIT = 0x200,
        /// <summary> 
        /// Controls coherency of depth/stencil attachment writes
        /// </summary> 
        DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 0x400,
        /// <summary> 
        /// Controls coherency of transfer reads
        /// </summary> 
        TRANSFER_READ_BIT = 0x800,
        /// <summary> 
        /// Controls coherency of transfer writes
        /// </summary> 
        TRANSFER_WRITE_BIT = 0x1000,
        /// <summary> 
        /// Controls coherency of host reads
        /// </summary> 
        HOST_READ_BIT = 0x2000,
        /// <summary> 
        /// Controls coherency of host writes
        /// </summary> 
        HOST_WRITE_BIT = 0x4000,
        /// <summary> 
        /// Controls coherency of memory reads
        /// </summary> 
        MEMORY_READ_BIT = 0x8000,
        /// <summary> 
        /// Controls coherency of memory writes
        /// </summary> 
        MEMORY_WRITE_BIT = 0x10000,
        TRANSFORM_FEEDBACK_WRITE_BIT_EXT = 0x2000000,
        TRANSFORM_FEEDBACK_COUNTER_READ_BIT_EXT = 0x4000000,
        TRANSFORM_FEEDBACK_COUNTER_WRITE_BIT_EXT = 0x8000000,
        /// <summary> 
        /// read access flag for reading conditional rendering predicate
        /// </summary> 
        CONDITIONAL_RENDERING_READ_BIT_EXT = 0x100000,
        COMMAND_PROCESS_READ_BIT_NVX = 0x20000,
        COMMAND_PROCESS_WRITE_BIT_NVX = 0x40000,
        COLOR_ATTACHMENT_READ_NONCOHERENT_BIT_EXT = 0x80000,
        SHADING_RATE_IMAGE_READ_BIT_NV = 0x800000,
        ACCELERATION_STRUCTURE_READ_BIT_NV = 0x200000,
        ACCELERATION_STRUCTURE_WRITE_BIT_NV = 0x400000,
    };

}
