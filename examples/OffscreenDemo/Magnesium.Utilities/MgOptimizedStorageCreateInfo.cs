using Magnesium;

namespace Magnesium.Utilities
{
    public class MgOptimizedStorageCreateInfo
    {
        public MgStorageBlockAllocationInfo[] Allocations { get; set; }
        public MgSharingMode SharingMode { get; internal set; }
        public uint[] QueueFamilyIndices { get; internal set; }
    }
}
