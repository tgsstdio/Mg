using System;

namespace Magnesium
{
    [Flags]
    public enum MgPipelineStageFlagBits : UInt32
    {
        /// <summary> 
        /// Before subsequent commands are processed
        /// </summary> 
        TOP_OF_PIPE_BIT = 0x1,
        /// <summary> 
        /// Draw/DispatchIndirect command fetch
        /// </summary> 
        DRAW_INDIRECT_BIT = 0x2,
        /// <summary> 
        /// Vertex/index fetch
        /// </summary> 
        VERTEX_INPUT_BIT = 0x4,
        /// <summary> 
        /// Vertex shading
        /// </summary> 
        VERTEX_SHADER_BIT = 0x8,
        /// <summary> 
        /// Tessellation control shading
        /// </summary> 
        TESSELLATION_CONTROL_SHADER_BIT = 0x10,
        /// <summary> 
        /// Tessellation evaluation shading
        /// </summary> 
        TESSELLATION_EVALUATION_SHADER_BIT = 0x20,
        /// <summary> 
        /// Geometry shading
        /// </summary> 
        GEOMETRY_SHADER_BIT = 0x40,
        /// <summary> 
        /// Fragment shading
        /// </summary> 
        FRAGMENT_SHADER_BIT = 0x80,
        /// <summary> 
        /// Early fragment (depth and stencil) tests
        /// </summary> 
        EARLY_FRAGMENT_TESTS_BIT = 0x100,
        /// <summary> 
        /// Late fragment (depth and stencil) tests
        /// </summary> 
        LATE_FRAGMENT_TESTS_BIT = 0x200,
        /// <summary> 
        /// Color attachment writes
        /// </summary> 
        COLOR_ATTACHMENT_OUTPUT_BIT = 0x400,
        /// <summary> 
        /// Compute shading
        /// </summary> 
        COMPUTE_SHADER_BIT = 0x800,
        /// <summary> 
        /// Transfer/copy operations
        /// </summary> 
        TRANSFER_BIT = 0x1000,
        /// <summary> 
        /// After previous commands have completed
        /// </summary> 
        BOTTOM_OF_PIPE_BIT = 0x2000,
        /// <summary> 
        /// Indicates host (CPU) is a source/sink of the dependency
        /// </summary> 
        HOST_BIT = 0x4000,
        /// <summary> 
        /// All stages of the graphics pipeline
        /// </summary> 
        ALL_GRAPHICS_BIT = 0x8000,
        /// <summary> 
        /// All stages supported on the queue
        /// </summary> 
        ALL_COMMANDS_BIT = 0x10000,
        TRANSFORM_FEEDBACK_BIT_EXT = 0x1000000,
        /// <summary> 
        /// A pipeline stage for conditional rendering predicate fetch
        /// </summary> 
        CONDITIONAL_RENDERING_BIT_EXT = 0x40000,
        COMMAND_PROCESS_BIT_NVX = 0x20000,
        SHADING_RATE_IMAGE_BIT_NV = 0x400000,
        RAY_TRACING_SHADER_BIT_NV = 0x200000,
        ACCELERATION_STRUCTURE_BUILD_BIT_NV = 0x2000000,
        TASK_SHADER_BIT_NV = 0x80000,
        MESH_SHADER_BIT_NV = 0x100000,
    }
}
