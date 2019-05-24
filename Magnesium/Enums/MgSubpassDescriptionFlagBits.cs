using System;

namespace Magnesium
{
    [Flags]
    public enum MgSubpassDescriptionFlagBits : UInt32
    {
        PER_VIEW_ATTRIBUTES_BIT_NVX = 1 << 0,
        PER_VIEW_POSITION_X_ONLY_BIT_NVX = 1 << 1,
    }
}
