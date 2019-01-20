using System;

namespace Magnesium
{
    public class MgCheckpointDataNV
    {
        public MgPipelineStageFlagBits Stage { get; set; }
        public IntPtr CheckpointMarker { get; set; }
    }
}