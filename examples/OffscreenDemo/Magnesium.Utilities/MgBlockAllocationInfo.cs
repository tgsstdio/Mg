using Magnesium;

namespace Magnesium.Utilities
{
    public class MgBlockAllocationInfo
    {
        public MgBufferUsageFlagBits Usage { get; set; }
        public ulong Size { get; set; }
        public MgMemoryPropertyFlagBits MemoryPropertyFlags { get; set; }
        public uint ElementByteSize { get; set; }
    }
}
