using System;

namespace Magnesium
{
    public enum MgPipelineBindPoint : UInt32
    {
        GRAPHICS = 0,
        COMPUTE = 1,
        RAY_TRACING_NV = 1000165000,
    }
}
