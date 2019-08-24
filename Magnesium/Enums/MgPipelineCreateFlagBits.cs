using System;

namespace Magnesium
{
    [Flags]
    public enum MgPipelineCreateFlagBits : UInt32
    {
        DISABLE_OPTIMIZATION_BIT = 0x1,
        ALLOW_DERIVATIVES_BIT = 0x2,
        DERIVATIVE_BIT = 0x4,
        VIEW_INDEX_FROM_DEVICE_INDEX_BIT = 0x8,
        DISPATCH_BASE = 0x10,
        VIEW_INDEX_FROM_DEVICE_INDEX_BIT_KHR = VIEW_INDEX_FROM_DEVICE_INDEX_BIT,
        DISPATCH_BASE_KHR = DISPATCH_BASE,
        DEFER_COMPILE_BIT_NV = 0x20,
    }
}

