using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshInstance
    {
        public IMgBuffer Buffer { get; set; }
        public IMgDeviceMemory DeviceMemory { get; set; }
        public ulong AllocationSize { get; internal set; }
        public uint[] PackingOrder { get; internal set; }
        public MgBufferUsageFlagBits Usage { get; internal set; }
        public MgMemoryPropertyFlagBits MemoryPropertyFlags { get; internal set; }
        public ulong MemoryOffset { get; internal set; }
    }    
}
