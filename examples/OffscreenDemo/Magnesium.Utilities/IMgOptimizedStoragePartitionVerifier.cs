namespace Magnesium.Utilities
{
    public interface IMgOptimizedStoragePartitionVerifier
    {
        MgStorageBufferInstance[] Revise(MgOptimizedStorageCreateInfo createInfo, MgStorageBlockInfo[] segments);
    }
}
