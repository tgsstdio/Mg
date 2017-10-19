using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshAllocation
    {
        public uint Index { get; internal set; }
        public uint InstanceIndex { get; internal set; }
        public ulong Offset { get; internal set; }
        public MgBufferUsageFlagBits Usage { get; set; }
        public ulong Size { get; internal set; }
    }    
}
