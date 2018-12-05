using System;

namespace Magnesium
{
    [Flags]
    public enum MgImageAspectFlagBits : UInt32
    {
        COLOR_BIT = 0x1,
        DEPTH_BIT = 0x2,
        STENCIL_BIT = 0x4,
        METADATA_BIT = 0x8,
        PLANE_0_BIT = 0x10,
        PLANE_1_BIT = 0x20,
        PLANE_2_BIT = 0x40,
        PLANE_0_BIT_KHR = PLANE_0_BIT,
        PLANE_1_BIT_KHR = PLANE_1_BIT,
        PLANE_2_BIT_KHR = PLANE_2_BIT,
        MEMORY_PLANE_0_BIT_EXT = 0x80,
        MEMORY_PLANE_1_BIT_EXT = 0x100,
        MEMORY_PLANE_2_BIT_EXT = 0x200,
        MEMORY_PLANE_3_BIT_EXT = 0x400,
    }
}

