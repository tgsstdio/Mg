namespace Magnesium.Utilities
{
    public class MgOptimizedStorageAllocation
    {
        public uint AllocationIndex { get; internal set; }
        public uint BlockIndex { get; internal set; }
        public ulong Offset { get; internal set; }
        public ulong Size { get; internal set; }
        public MgBufferUsageFlagBits Usage { get; set; }
    }    
}
