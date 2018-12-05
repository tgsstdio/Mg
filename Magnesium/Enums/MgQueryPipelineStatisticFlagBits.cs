using System;

namespace Magnesium
{
    [Flags]
    public enum MgQueryPipelineStatisticFlagBits : UInt32
    {
        /// <summary> 
        /// Optional
        /// </summary> 
        INPUT_ASSEMBLY_VERTICES_BIT = 0x1,
        /// <summary> 
        /// Optional
        /// </summary> 
        INPUT_ASSEMBLY_PRIMITIVES_BIT = 0x2,
        /// <summary> 
        /// Optional
        /// </summary> 
        VERTEX_SHADER_INVOCATIONS_BIT = 0x4,
        /// <summary> 
        /// Optional
        /// </summary> 
        GEOMETRY_SHADER_INVOCATIONS_BIT = 0x8,
        /// <summary> 
        /// Optional
        /// </summary> 
        GEOMETRY_SHADER_PRIMITIVES_BIT = 0x10,
        /// <summary> 
        /// Optional
        /// </summary> 
        CLIPPING_INVOCATIONS_BIT = 0x20,
        /// <summary> 
        /// Optional
        /// </summary> 
        CLIPPING_PRIMITIVES_BIT = 0x40,
        /// <summary> 
        /// Optional
        /// </summary> 
        FRAGMENT_SHADER_INVOCATIONS_BIT = 0x80,
        /// <summary> 
        /// Optional
        /// </summary> 
        TESSELLATION_CONTROL_SHADER_PATCHES_BIT = 0x100,
        /// <summary> 
        /// Optional
        /// </summary> 
        TESSELLATION_EVALUATION_SHADER_INVOCATIONS_BIT = 0x200,
        /// <summary> 
        /// Optional
        /// </summary> 
        COMPUTE_SHADER_INVOCATIONS_BIT = 0x400,
    }
}
