using Magnesium;

namespace Magnesium.Utilities
{
    public class MgBufferInstance
    {
        public IMgBuffer Buffer { get; set; }
        public MgBufferUsageFlagBits Usage { get; set; }
        public MgMemoryPropertyFlagBits MemoryPropertyFlags { get; set; }
        public ulong AllocationSize { get; set; }
        public MgBufferOffsetMapping[] Mappings { get; set; }
        public uint TypeIndex { get; internal set; }
    }
}
