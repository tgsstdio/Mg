using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshAttribute
    {
        public uint Index { get; internal set; }
        public uint BlockIndex { get; internal set; }
        public ulong ByteOffset { get; internal set; }
        public MgBufferUsageFlagBits Usage { get; set; }
        public ulong Size { get; internal set; }
    }    
}
