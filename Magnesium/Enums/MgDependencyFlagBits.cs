using System;

namespace Magnesium
{
    [Flags]
    public enum MgDependencyFlagBits : UInt32
    {
        /// <summary> 
        /// Dependency is per pixel region 
        /// </summary> 
        BY_REGION_BIT = 0x1,
        /// <summary> 
        /// Dependency is across devices
        /// </summary> 
        DEVICE_GROUP_BIT = 0x4,
        VIEW_LOCAL_BIT = 0x2,
        VIEW_LOCAL_BIT_KHR = VIEW_LOCAL_BIT,
        DEVICE_GROUP_BIT_KHR = DEVICE_GROUP_BIT,
    }
}

