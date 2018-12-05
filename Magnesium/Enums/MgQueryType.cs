using System;

namespace Magnesium
{
    public enum MgQueryType : UInt32
    {
        OCCLUSION = 0,
        /// <summary> 
        /// Optional
        /// </summary> 
        PIPELINE_STATISTICS = 1,
        TIMESTAMP = 2,
        TRANSFORM_FEEDBACK_STREAM_EXT = 1000028004,
        ACCELERATION_STRUCTURE_COMPACTED_SIZE_NV = 1000165000,
    }
}
