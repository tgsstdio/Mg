using Magnesium;

namespace Magnesium.Utilities
{
    public class MgStorageBufferInstance
    {
        public IMgBuffer Buffer { get; set; }
        public MgBufferUsageFlagBits Usage { get; set; }
        public MgMemoryPropertyFlagBits MemoryPropertyFlags { get; set; }
        public ulong AllocationSize { get; set; }
        public MgStorageBufferOffset[] Mappings { get; set; }
        public uint TypeIndex { get; internal set; }
    }
}
