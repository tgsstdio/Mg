using System;

namespace Magnesium
{
    [Flags]
    public enum MgSampleCountFlagBits : UInt32
    {
        /// <summary> 
        /// Sample count 1 supported
        /// </summary> 
        COUNT_1_BIT = 0x1,
        /// <summary> 
        /// Sample count 2 supported
        /// </summary> 
        COUNT_2_BIT = 0x2,
        /// <summary> 
        /// Sample count 4 supported
        /// </summary> 
        COUNT_4_BIT = 0x4,
        /// <summary> 
        /// Sample count 8 supported
        /// </summary> 
        COUNT_8_BIT = 0x8,
        /// <summary> 
        /// Sample count 16 supported
        /// </summary> 
        COUNT_16_BIT = 0x10,
        /// <summary> 
        /// Sample count 32 supported
        /// </summary> 
        COUNT_32_BIT = 0x20,
        /// <summary> 
        /// Sample count 64 supported
        /// </summary> 
        COUNT_64_BIT = 0x40,
    }
}
