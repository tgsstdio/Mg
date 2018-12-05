using System;

namespace Magnesium
{
    [Flags]
    public enum MgShaderStageFlagBits : UInt32
    {
        VERTEX_BIT = 0x1,
        TESSELLATION_CONTROL_BIT = 0x2,
        TESSELLATION_EVALUATION_BIT = 0x4,
        GEOMETRY_BIT = 0x8,
        FRAGMENT_BIT = 0x10,
        COMPUTE_BIT = 0x20,
        ALL_GRAPHICS = 0x0000001F,
        ALL = 0x7FFFFFFF,
        RAYGEN_BIT_NV = 0x100,
        ANY_HIT_BIT_NV = 0x200,
        CLOSEST_HIT_BIT_NV = 0x400,
        MISS_BIT_NV = 0x800,
        INTERSECTION_BIT_NV = 0x1000,
        CALLABLE_BIT_NV = 0x2000,
        TASK_BIT_NV = 0x40,
        MESH_BIT_NV = 0x80,
    }
}
