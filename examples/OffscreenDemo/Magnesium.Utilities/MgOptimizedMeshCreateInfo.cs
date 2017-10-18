using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshCreateInfo
    {
        public MgBlockAllocationInfo[] Allocations { get; set; }
        public IMgAllocationCallbacks AllocationCallbacks { get; set; }
        public MgSharingMode SharingMode { get; internal set; }
        public uint QueueFamilyIndexCount { get; internal set; }
        public uint[] QueueFamilyIndices { get; internal set; }
    }
}
